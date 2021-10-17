using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character : MonoBehaviour
{
    [SerializeField] public float Running;
    [SerializeField] public float FastRunning;
    [SerializeField] public float DefaultReachDistance;
    [SerializeField] public bool isRightSide;
    [SerializeField] public CameraBehaviour Camera;
    [SerializeField] public GameObject QuestionMark;
    [SerializeField] public float Delay;

    public Animator animator { get; private set; }

    public float CurrentMovingSpeed { get; set; }
    public Vector2 TargetPosition { get; set; }
    public bool Idling { get; private set; } = true;

    private bool idlingBuffer = true;

    private Floor currentFloor;
    private Stack<Vector2> TargetPositionsStack { get; set; } = new Stack<Vector2>();
    private Stack<Interactable> ItemsStack { get; set; } = new Stack<Interactable>();


    public List<Weapon> WeaponList { get; private set; }


    void Start()
    {
        WeaponList = new List<Weapon>();
        foreach (Transform transform in gameObject.transform.parent.parent.transform)
        {
            if (transform.gameObject.TryGetComponent<Floor>(out Floor floor))
            {
                foreach (Transform layer in floor.transform)
                {
                    foreach (Transform obj in layer.transform)
                    {
                        if (obj.gameObject.TryGetComponent<Weapon>(out Weapon weapon))
                        {
                            WeaponList.Add(weapon);
                        }
                    }
                }
            }
        }

        if (Camera == null)
        {
            throw new System.ArgumentException("SerializeField Camera is not set!");
        }
        QuestionMark.SetActive(false);
        animator = gameObject.GetComponent<Animator>();
        CurrentMovingSpeed = Running;
        TargetPosition = transform.position;
    }

    public void ChangeFloor(Floor floor, Vector2 offset, bool setPosition)
    {
        gameObject.transform.parent = floor.transform;
        if (!setPosition)
        {
            gameObject.transform.position += floor.transform.position - currentFloor.transform.position;
            gameObject.transform.position += new Vector3(offset.x, offset.y);
        } else
        {
            gameObject.transform.position = new Vector3(offset.x, offset.y, gameObject.transform.position.z);
        }
        TargetPosition = gameObject.transform.position;
        currentFloor = floor;
        currentFloor.gameObject.transform.hasChanged = true;
    }

    public void UpdateAnimation()
    {
        if (Idling != idlingBuffer)
        {
            animator.SetInteger("MovingState", System.Convert.ToInt32(!Idling));
            idlingBuffer = Idling;
        }
    }

    void UpdateParent()
    {
        if (transform.hasChanged || currentFloor == null)
        {
            currentFloor = transform.parent.GetComponent<Floor>();
            if(currentFloor == null)
            {
                throw new System.ArgumentException("The player should be a child of Floor object!");
            }
            transform.hasChanged = false;
        }
    }


    private Interactable.Interaction interactionState = new Interactable.Interaction();
    private bool interacted;
    private float _stopwatch = 0;

    private float idlingStart = 0;
    // This function is called whenever the left mouse button is held
    public void ClearStack()
    {
        TargetPositionsStack.Clear();
        ItemsStack.Clear();
    }

    void PushCurrentTarget()
    {
        TargetPositionsStack.Push(TargetPosition);
        ItemsStack.Push(Camera.SelectedItem);
    }

    // This function is called immediately after interaction with the current target
    // this function updates 
    private void TargetMet()
    {
        if (TargetPositionsStack.Count > 0)
        {
            var bufferedTargetPosition = TargetPositionsStack.Pop();
            var bufferedItem = ItemsStack.Pop();
            if (bufferedTargetPosition.x != 0 || bufferedTargetPosition.y != 0)
            {
                TargetPosition = bufferedTargetPosition;
                Camera.SetSelectedItem(bufferedItem);
            }
        }
    }



    // TODO:
    // refactor this hell
    void Update()
    {
        UpdateParent();

        if (idlingStart + 4 < Time.realtimeSinceStartup)
        {
            QuestionMark.SetActive(true);
        }

        // swap target position with the staircases' one if the target position is outside current floor
        // CurrentFloor.StaircaseUp if          the target position is above our floor
        // CurrentFloor.StaircaseDown if below
        if (currentFloor.StaircaseDown != null &&
            currentFloor.PreviousFloor != null && 
            currentFloor.transform.position.y > TargetPosition.y && // check if the target position is on the floor below
            (Vector2)currentFloor.StaircaseDown.transform.position != TargetPosition &&
            ((Camera.SelectedItem == null) || (currentFloor.PreviousFloor.StaircaseUp.transform != Camera.SelectedItem.transform)))
        {
            PushCurrentTarget();
            TargetPosition = currentFloor.StaircaseDown.transform.position;
            Camera.SetSelectedItem(currentFloor.StaircaseDown);
        }
        else if (currentFloor.StaircaseUp != null &&
          currentFloor.NextFloor != null &&
          currentFloor.NextFloor.transform.position.y <= TargetPosition.y && // check if the target position is on the floor above
          (Vector2)currentFloor.StaircaseUp.transform.position != TargetPosition &&
          ((Camera.SelectedItem == null) || (currentFloor.NextFloor.StaircaseDown.transform != Camera.SelectedItem.transform)))
        {
            PushCurrentTarget();
            TargetPosition = currentFloor.StaircaseUp.transform.position;
            Camera.SetSelectedItem(currentFloor.StaircaseUp);
        }


        Transform obstacle = currentFloor.GetObstacle(transform.position.x, TargetPosition.x);
        if (obstacle != null && Camera.SelectedItem != null && obstacle == Camera.SelectedItem.transform)
        {
            obstacle = null;
        }

        if (obstacle != null && obstacle.TryGetComponent<Door>(out Door door))
        {
            PushCurrentTarget();
            TargetPosition = obstacle.transform.position;
            Camera.SetSelectedItem(door);
            obstacle = null;
        }


        float distance = TargetPosition.x - transform.position.x;

        // interaction and idling section.
        if (_stopwatch > Time.realtimeSinceStartup)
        { 
            if (Time.realtimeSinceStartup - _stopwatch > -Delay)
            {
                // if we haven't interacted, selectedObject is on this floor and is not null
                // and the object is within reach distance — we interact with it
                if (!interacted)
                {
                    if (Camera.SelectedItem != null &&
                        Camera.SelectedItem.Floor == currentFloor &&
                        Mathf.Abs(distance) <= Camera.SelectedItem.GetReachDistance())
                    {
                        Camera.SelectedItem.Interact(interactionState);
                        TargetMet();
                        _stopwatch = Time.realtimeSinceStartup - 1;
                        interacted = true;
                    }
                    else if (Camera.SelectedItem == null)
                    {
                        TargetMet();
                    }
                }
                
                Idling = true;
            }
            return;
        }


        float reachDistance = Camera.SelectedItem == null ? DefaultReachDistance : Camera.SelectedItem.GetReachDistance();
        if (Mathf.Abs(distance) > reachDistance && obstacle == null)
        {

            interacted = false;
            interactionState.RightSide = isRightSide = distance > 0;

            gameObject.transform.localScale = new Vector3(isRightSide ? 1 : -1, 1, 1);
            // neglect the changes above
            QuestionMark.transform.localScale = new Vector3(isRightSide ? 1 : -1, 1, 1);

            int wayToGo = transform.position.x > TargetPosition.x ? -1 : 1;
            transform.Translate(new Vector3(wayToGo, 0, 0) * CurrentMovingSpeed * Time.deltaTime);
            Idling = false;
            idlingStart = Time.realtimeSinceStartup;
            QuestionMark.SetActive(false);
            Camera.View.ShakeCameraSmoothly(0.1f);
        }
        else
        {
            _stopwatch = Time.realtimeSinceStartup + Delay * 2;
        }

        UpdateAnimation();
    }
}

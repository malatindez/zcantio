using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character : MonoBehaviour
{
    [SerializeField] public float Running = 5;
    [SerializeField] public float FastRunning = 10;
    [SerializeField] public float MaxStamina = 60;
    [SerializeField] public float MinStaminaToRun = 15;
    [SerializeField] public float DefaultReachDistance = 0.1f;
    [SerializeField] public float Delay = 0.025f;
    [SerializeField] public GameObject QuestionMark;
    [SerializeField] public CameraBehaviour Camera;
    [SerializeField] CameraView view;

    public Animator animator { get; private set; }
    public float CurrentMovingSpeed { get; set; }
    public Vector2 TargetPosition { get; set; }
    public bool Idling { get; private set; } = true;
    public float Stamina { get; private set; } = 60;

    private bool isRightSide;
    private bool idlingBuffer = true;

    private Floor currentFloor;
    private Stack<Vector2> TargetPositionsStack { get; set; } = new Stack<Vector2>();
    private Stack<Interactable> ItemsStack { get; set; } = new Stack<Interactable>();



    void Start()
    {
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
            if (currentFloor == null)
            {
                throw new System.ArgumentException("The player should be a child of Floor object!");
            }
            transform.hasChanged = false;
        }
    }
    
    void UpdateStamina(float addStamina = 0)
    {
        Stamina += Time.deltaTime + addStamina;
        if (Stamina > MaxStamina)
        {
            Stamina = MaxStamina;
        }
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
    private void UpdateInteract(float distance)
    {
        // interaction and idling section.
        if (_stopwatch > Time.realtimeSinceStartup)
        {
            if (Time.realtimeSinceStartup - _stopwatch > -CurrentDelay)
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
    }

    private void UpdateTargets()
    {
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
    }
    private Transform GetObstacle()
    {
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
        }
        return obstacle;
    }

    private float CurrentDelay = 0.1f;

    // TODO:
    // refactor this mess
    void Update()
    {
        UpdateAnimation();
        UpdateParent();
        UpdateStamina(Mathf.Log((float)System.Math.E + Time.realtimeSinceStartup - idlingStart));

        if (idlingStart + 4 < Time.realtimeSinceStartup)
        {
            QuestionMark.SetActive(true);
        }

        float distance = TargetPosition.x - transform.position.x;

        UpdateInteract(distance);
        UpdateTargets();

        Transform obstacle = GetObstacle();


        float reachDistance = Camera.SelectedItem == null ? DefaultReachDistance : Camera.SelectedItem.GetReachDistance();
        if (Mathf.Abs(distance) > reachDistance && obstacle == null)
        {
            if (CurrentMovingSpeed == FastRunning && Stamina <= MinStaminaToRun)
            {
                CurrentMovingSpeed = Running;
            }
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
            Stamina -= Time.deltaTime * 2;
            if (CurrentMovingSpeed == FastRunning)
            {
                Stamina -= Time.deltaTime * 3;
            }
            
            if (Stamina < 0)
            {
                _stopwatch = Time.realtimeSinceStartup + MinStaminaToRun;
                CurrentDelay = MinStaminaToRun - Delay;
            }
            view.ShakeCameraSmoothly(Time.deltaTime * 2);
        }
        else
        {
            _stopwatch = Time.realtimeSinceStartup + Delay * 2;
            CurrentDelay = Delay;
        }

    }
}

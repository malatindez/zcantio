using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    [SerializeField] public float Running;
    [SerializeField] public float FastRunning;
    [SerializeField] public float DefaultReachDistance;
    [SerializeField] public bool isRightSide;
    [SerializeField] public CameraBehaviour Camera;
    [SerializeField] public float Delay;

    public Animator animator { get; private set; }

    public float CurrentMovingSpeed { get; set; }
    public Vector2 TargetPosition { get; set; }
    public bool Idling { get; private set; } = true;

    private bool idlingBuffer = true;

    private Floor currentFloor;

    void Start()
    {
        interactionState = new Interactable.Interaction();
        animator = gameObject.GetComponent<Animator>();
        CurrentMovingSpeed = Running;
        TargetPosition = transform.position;
        if (Camera == null)
        {
            throw new System.ArgumentException("SerializeField Camera is not set!");
        }
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


    private Interactable.Interaction interactionState;
    private bool interacted;
    private float _stopwatch = 0;
    void Update()
    {
        UpdateParent();

        float distance = TargetPosition.x - transform.position.x;
        if (_stopwatch > Time.realtimeSinceStartup)
        {
            if (Time.realtimeSinceStartup - _stopwatch > -Delay)
            {
                if (!interacted && 
                    Camera.SelectedItem != null && 
                    Camera.SelectedItem.Floor == currentFloor &&
                    Mathf.Abs(distance) <= Camera.SelectedItem.GetReachDistance())
                {
                    Camera.SelectedItem.Interact(interactionState);
                    interacted = true;
                }
                Idling = true;
            }
            return;
        }

        float reachDistance = Camera.SelectedItem == null ? DefaultReachDistance : Camera.SelectedItem.GetReachDistance();
        if (Mathf.Abs(distance) > reachDistance && 
            !currentFloor.checkForObstacles(
                transform.position.x, 
                TargetPosition.x, 
                Camera.SelectedItem == null ? null : Camera.SelectedItem.transform))
        {
            interacted = false;
            interactionState.RightSide = isRightSide = distance > 0;

            gameObject.transform.localScale = new Vector3(isRightSide ? 1 : -1, 1, 1);
            int wayToGo = transform.position.x > TargetPosition.x ? -1 : 1;
            transform.Translate(new Vector3(wayToGo, 0, 0) * CurrentMovingSpeed * Time.deltaTime);
            Idling = false;
        }
        else
        {
            _stopwatch = Time.realtimeSinceStartup + Delay * 2;
        }

        UpdateAnimation();
    }
}

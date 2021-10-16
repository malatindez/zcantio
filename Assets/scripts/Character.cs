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
    void Start()
    {   
        animator = gameObject.GetComponent<Animator>();
        CurrentMovingSpeed = Running;
        TargetPosition = transform.position;

        if (Camera == null)
        {
            throw new System.ArgumentException("SerializeField Camera is not set!");
        }
    }

    public void UpdateAnimation()
    {
        if (Idling != idlingBuffer)
        {
            animator.SetInteger("MovingState", System.Convert.ToInt32(!Idling));
            idlingBuffer = Idling;
        }
    }


    private float _stopwatch = 0;
    void Update()
    {
        if(_stopwatch > Time.realtimeSinceStartup)
        {
            if (Time.realtimeSinceStartup - _stopwatch > -Delay)
            {
                if (Camera.SelectedItem != null)
                    Camera.SelectedItem.Interact();
                Idling = true;
            }
            return;
        }
        float distance = TargetPosition.x - transform.position.x;
        float reachDistance = Camera.SelectedItem == null ? DefaultReachDistance : Camera.SelectedItem.GetReachDistance();
        if (Mathf.Abs(distance) > reachDistance)
        {
            isRightSide = distance > 0;
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

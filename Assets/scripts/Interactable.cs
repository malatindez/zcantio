using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : SpriteOutline
{
    public class Interaction
    {
        // Side from which the character came from.
        // False for left, True for right.
        public bool RightSide { get; set; } = false;
        public Interaction()
        {
        }

        public Interaction(bool rightSide)
        {
            this.RightSide = rightSide;
        }
    }
    
    [SerializeField] protected float ReachDistance = 0.1f;

    public Floor Floor;
    public float GetReachDistance()
    {
        return ReachDistance;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        if (ReachDistance <= 0)
        {
            throw new System.ArgumentException("ReachDistance cannot be less or equal to zero!");
        }
        base.Start();
        Floor = gameObject.transform.parent.parent.GetComponent<Floor>();
    }


    public virtual void Interact(Interaction i)
    {

    }
}

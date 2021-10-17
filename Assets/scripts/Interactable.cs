using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : SpriteOutline
{
    [SerializeField] protected float ReachDistance = 0.1f;
    public float GetReachDistance()
    {
        return ReachDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ReachDistance <= 0)
        {
            throw new System.ArgumentException("ReachDistance cannot be less or equal to zero!");
        }
        base.Start();
    }


    public virtual void Interact()
    {

    }
    void Update()
    {
        base.Update();
    }
}

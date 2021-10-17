using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase : Interactable
{
    [SerializeField] Floor TargetFloor;
    [SerializeField] private Character _character;
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool setPosition;

    // Start is called before the first frame update
    new void Start()
    {
        if(TargetFloor == null)
        {
            throw new System.ArgumentException("SerializeField TargetFloor cannot be null!");
        }
        base.Start();
    }
    public override void Interact(Interaction i)
    {
        _character.ChangeFloor(TargetFloor, offset, setPosition);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] CameraView view;


    Animator animator;

    void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();
    }

    public override void Interact(Interaction i)
    {
        if (i.RightSide)
        {
            gameObject.transform.rotation = new Quaternion(0,180,0,0);
        }
        else
        {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        animator.SetBool("isOpened", true);
        view.ShakeCamera(0.1f,1);
        gameObject.transform.parent = Floor.transform.Find("Background").transform;
        Floor.transform.hasChanged = true;
    }


}

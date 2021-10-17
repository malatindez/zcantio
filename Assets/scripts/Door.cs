using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    CameraView view;


    Animator animator;

    void Start()
    {
        base.Start();
        view = Camera.main.GetComponent<CameraView>();
        animator = gameObject.GetComponent<Animator>();
    }

    public override void Interact(Interaction i)
    {
        if (i.RightSide)
        {
            gameObject.transform.rotation = new Quaternion(0, 0,0,0);
        }
        else
        {
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        animator.SetBool("isOpened", true);
        view.ShakeCameraRoughly(0.1f,1);
        gameObject.transform.parent = Floor.transform.Find("Background").transform;
        Floor.transform.hasChanged = true;
        ReachDistance = 1.5f;
    }


}

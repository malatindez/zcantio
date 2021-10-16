using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public GameObject CharacterObject;

    Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    [ContextMenu("Interact")]
    public override void Interact()
    {
        if (CharacterObject.transform.position.x > gameObject.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        animator.SetBool("isOpened", true);
    }


    void Update()
    {

    }
}

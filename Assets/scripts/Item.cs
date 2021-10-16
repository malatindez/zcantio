using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    void Start()
    {
        var outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;
    }
    public override void Interact()
    {

    }

    void Update()
    {
        
    }
}

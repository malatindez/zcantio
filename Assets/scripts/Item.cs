using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        var outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

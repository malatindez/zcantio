using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    Outline Outline;
    // Start is called before the first frame update
    void Start()
    {
        Outline = gameObject.AddComponent<Outline>();

        Outline.OutlineMode = Outline.Mode.OutlineAll;
        Outline.OutlineColor = Color.yellow;
        Outline.OutlineWidth = 5f;
        Outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

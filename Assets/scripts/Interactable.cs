using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    Outline Outline;
    [SerializeField] protected float ReachDistance;
    [SerializeField] protected Vector2 Coordinates;

    public float GetReachDistance()
    {
        return ReachDistance;
    }
    public Vector2 GetCoordinates()
    {
        return Coordinates;
    }

    // Start is called before the first frame update
    void Start()
    {
        Outline = gameObject.AddComponent<Outline>();

        Outline.OutlineMode = Outline.Mode.OutlineAll;
        Outline.OutlineColor = Color.yellow;
        Outline.OutlineWidth = 5f;
        Outline.enabled = false;
    }
    public virtual void Interact()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

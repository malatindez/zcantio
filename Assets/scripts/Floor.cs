using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] public Floor PreviousFloor = null;
    [SerializeField] public Floor NextFloor = null;
    [SerializeField] public Staircase StaircaseUp = null;
    [SerializeField] public Staircase StaircaseDown = null;

    private List<Transform> foregroundObjects;
    
    void Start()
    {
        foregroundObjects = new List<Transform>();
        transform.hasChanged = true;
    }

    public bool checkForObstacles(float position, float target, Transform skip = null)
    {
        float temp = target - position;
        foreach(Transform foregroundObject in foregroundObjects)
        {
            bool flagA = foregroundObject.position.x - position > 0;
            bool flagB = foregroundObject.position.x - position < temp;
            if ((skip == null || skip != foregroundObject) &&
                ((temp > 0 && flagA && flagB) ||
                (temp <= 0 && !flagA && !flagB ))
                )
                   return true;
        }
        return false;
    }
    private void Update()
    {
        if (transform.hasChanged)
        {
            foregroundObjects.Clear();

            foreach (Transform transform in transform.Find("Foreground").transform)
            {
                foregroundObjects.Add(transform);
            }
            transform.hasChanged = false;
        }
    }
}

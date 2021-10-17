using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector3.left * 0.01f);
    }
}

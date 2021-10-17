using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector3.left * 0.01f);
        if (gameObject.transform.position.x < -40)
            gameObject.transform.position = new Vector3(0, gameObject.transform.position.y, gameObject.transform.position.z);

    }
}

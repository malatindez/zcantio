using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Outline SelectedItem { get; private set; } = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GetComponent<UnityEngine.Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.TryGetComponent<Item>(out Item item))
            {
                if (SelectedItem != null)
                {
                    SelectedItem.enabled = false;
                }
                SelectedItem = item.GetComponent<Outline>();
                SelectedItem.enabled = true;
            }
            else if (SelectedItem != null)
            {
                SelectedItem.enabled = false;
            }
        } 
    }
}

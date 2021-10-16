using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Character _character;


    int clickCounter = 0;
    float lastClickTime = 0;

    public Interactable SelectedItem { get; private set; } = null;
    public Interactable HoveredItem { get; private set; } = null;

    void Start()
    {
        if (_character == null)
        {
            throw new System.ArgumentException("SerializeField _character cannot be null!");
        }
    }


    void OnMultiClick(RaycastHit hit, Interactable item)
    {
        if(clickCounter <= 0)
        {
            return;
        }

        if (SelectedItem != null)
        {
            SelectedItem.Selected = false;
            SelectedItem = null;
        }
        if (item != null)
        {
            SelectedItem = item;
            SelectedItem.Selected = true;
        }
        if (hit.collider == null)
        {
            return;
        }

        _character.CurrentMovingSpeed = clickCounter >= 2
                                            ? _character.FastRunning
                                            : _character.Running;
        _character.TargetPosition = hit.point;
    }





    void Update()
    {
        if (HoveredItem != null)
        {
            HoveredItem.Hovered = false;
            HoveredItem = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Interactable item = null;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.TryGetComponent<Interactable>(out item))
        {
            HoveredItem = item;
            HoveredItem.Hovered = true;
        }
        if (Input.GetMouseButton(0))
        {
            var currentTime = Time.realtimeSinceStartup;
            if (Input.GetMouseButtonDown(0))
            {
                if (currentTime - lastClickTime > 0.2f)
                {
                    clickCounter = 1;
                }
                else
                {
                    clickCounter += 1;
                }
            }
            lastClickTime = currentTime;
            OnMultiClick(hit, item);
        }
    }
}

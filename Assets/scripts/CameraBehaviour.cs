using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] public CameraView View;


    int clickCounter = 0;
    float lastClickTime = 0;

    bool highlighted = false;

    public Interactable SelectedItem { get; private set; } = null;
    public Interactable HoveredItem { get; private set; } = null;

    private Startup CameraStartup;

    void Start()
    {
        View = GetComponent<CameraView>();
        CameraStartup = GetComponent<Startup>();
        if (_character == null)
        {
            throw new System.ArgumentException("SerializeField _character cannot be null!");
        }
    }

    public void SetSelectedItem(Interactable item)
    {
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
    }

    void OnMultiClick(RaycastHit2D hit, Interactable item)
    {
        if(clickCounter <= 0)
        {
            return;
        }
        SetSelectedItem(item);

        if (hit.collider != null)
        {
            _character.CurrentMovingSpeed = clickCounter >= 2
                                                ? _character.FastRunning
                                                : _character.Running;
            _character.TargetPosition = hit.point;
        }

    }


    void HighlightWeapons()
    {
        foreach (Weapon weapon in _character.WeaponList)
        {
            weapon.Highlight = true;
        }
    }
    void RemoveHighlighting()
    {
        foreach (Weapon weapon in _character.WeaponList)
        {
            weapon.Highlight = false;
        }
    }

    void LateUpdate()
    {
        if(!CameraStartup.isStarted)
        {
            if (!highlighted)
            {
                HighlightWeapons();
                highlighted = true;
            }
            return;
        }
        if(highlighted)
        {
            RemoveHighlighting();
            highlighted = true;
        }

        if (HoveredItem != null)
        {
            HoveredItem.Hovered = false;
            HoveredItem = null;
        }

                    
        Interactable item = null;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<Interactable>(out item))
        {
            HoveredItem = item;
            HoveredItem.Hovered = true;
        }
        if (Input.GetMouseButton(0))
        {
            _character.ClearStack();
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

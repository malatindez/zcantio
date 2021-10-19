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
            SelectedItem.Deselect();
        }
        SelectedItem = item;
        if (SelectedItem != null)
        {
            SelectedItem.Select();
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
            weapon.transform.localScale *= 1.5f;
            weapon.EnableHighlighting(Color.red);
        }
    }
    void RemoveHighlighting()
    {
        foreach (Weapon weapon in _character.WeaponList)
        {
            weapon.DisableHighlighting(Color.red);
            weapon.transform.localScale *= 1f/1.5f;
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
            highlighted = false;
        }

                    
        Interactable item = null;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if(hit.collider != null)
        {
            item = hit.collider.gameObject.GetComponent<Interactable>();
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

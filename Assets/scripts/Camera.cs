using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Camera : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Character _character;


    int clickCounter = 0;
    float lastClickTime = 0;

    public Outline SelectedItem { get; private set; } = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;
        OnMultiClick(clickCount);
    }

    void OnMultiClick(int i)
    {
        if(i <= 0)
        {
            return;
        }

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

        if (i >= 4)
        {
            _character.MovingSpeed = Character.FastRunning;
        }
        else if (i >= 2)
        {
            _character.MovingSpeed = Character.Running;
        }
        else
        {
            _character.MovingSpeed = Character.Walking;
        }
        _character.TargetPosition = hit.point;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
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
            OnMultiClick(clickCounter);
        }
    }
}

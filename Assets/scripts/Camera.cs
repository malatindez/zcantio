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
        

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.TryGetComponent<Interactible>(out Interactible item))
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
        if (hit.collider == null)
        {
            return;
        }
        if (i >= 2)
        {
            _character.MovingSpeed = _character.FastRunning;
            _character.AnimationFasterRun();
        }
        else
        {
            _character.MovingSpeed = _character.Running;
            _character.AnimationRun();
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

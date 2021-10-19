using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Selectable : MonoBehaviour
{
    private static Selectable _selectedObj;

    private static Selectable SelectedObject
    {
        get => _selectedObj;
        set
        {
            if (_selectedObj != value)
            {
                var buf = _selectedObj;
                _selectedObj = value;
                buf?.Deselect();
            }
        }
    }


    private const string OutlineColorBase = "_SolidOutline";
    [SerializeField] UnityEngine.Color SelectedColor = Color.yellow;
    [SerializeField] UnityEngine.Color HoveredColor = Color.white;

    [SerializeField] protected float OutlineWidth = 0.5f;


    private Material _material = null;
    private SpriteRenderer _renderer;
    private bool highlighted = false;

    protected void Start()
    {
        _material = Resources.Load("Materials/OutlineMaterial", typeof(Material)) as Material;
        _material = new Material(_material);
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _renderer.material = _material;
    }

    private void OnMouseEnter()
    {
        if(SelectedObject != this && !highlighted)
        {
            _material.SetColor(OutlineColorBase, HoveredColor);
            _material.SetFloat("_Thickness", OutlineWidth);
        }
    }

    private void OnMouseExit()
    {
        if (SelectedObject != this && !highlighted)
            _material.SetFloat("_Thickness", 0);
    }

    public void Select()
    {
        SelectedObject = this;
        _material.SetColor(OutlineColorBase, SelectedColor);
        _material.SetFloat("_Thickness", OutlineWidth);
    }

    public void Deselect()
    {
        if (SelectedObject == this)
        {
            SelectedObject = null;
        }
        _material.SetFloat("_Thickness", 0);
    }

    public void EnableHighlighting(UnityEngine.Color highlightColor)
    {
        highlighted = true;
        _material.SetColor(OutlineColorBase, highlightColor);
        _material.SetFloat("_Thickness", OutlineWidth);
    }
    public void DisableHighlighting(UnityEngine.Color highlightColor)
    {
        highlighted = false;
        _material.SetFloat("_Thickness", 0);
    }
}

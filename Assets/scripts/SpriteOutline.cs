using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpriteOutline : MonoBehaviour
{
    private const string OutlineColorBase = "_SolidOutline";
    [SerializeField] UnityEngine.Color SelectedColor = Color.yellow;
    [SerializeField] UnityEngine.Color HoveredColor = Color.white;
    [SerializeField] UnityEngine.Color HighlighColor = Color.red;

    [SerializeField] protected float OutlineWidth = 0.5f;
    public bool Selected { get; set; } = false;
    public bool Hovered { get; set; } = false;
    public bool Highlight { get; set; } = false;

    private Material _material = null;
    private SpriteRenderer _renderer;


    protected void Start()
    {
        _material = Resources.Load("Materials/OutlineMaterial", typeof(Material)) as Material;
        _material = new Material(_material);
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _renderer.material = _material;
    }

    public void Update()
    {
        if (Selected)
        {
            _material.SetColor(OutlineColorBase, SelectedColor);
            _material.SetFloat("_Thickness", OutlineWidth);
        }
        else if (Hovered)
        {
            _material.SetColor(OutlineColorBase, HoveredColor);
            _material.SetFloat("_Thickness", OutlineWidth);
        }
        else if (Highlight)
        {
            _material.SetColor(OutlineColorBase, HighlighColor);
            _material.SetFloat("_Thickness", 3.0f);
        }
        else
        {
            _material.SetFloat("_Thickness", 0);
        }
    }
}

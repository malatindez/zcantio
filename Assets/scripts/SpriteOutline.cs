using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutline : MonoBehaviour
{
    [SerializeField] UnityEngine.Color SelectedColor = Color.yellow;
    [SerializeField] UnityEngine.Color HoveredColor = Color.red;

    [SerializeField] protected float OutlineScalePercent = 1;
    public bool Hovered { get; set; } = false;
    public bool Selected { get; set; } = false;

    private GameObject hoveredSprite;
    private GameObject selectedSprite;


    protected void Start()
    {
        if (OutlineScalePercent <= 0)
        {
            throw new System.ArgumentException("OutlineScalePercent width cannot be less or equal to zero!");
        }
        byte clump(float i)
        {
            if(i < 0)
            {
                return 0;
            } else if (i > 255)
            {
                return 255;
            }
            return System.Convert.ToByte(i);
        }
        void paint(byte[] array, int i, UnityEngine.Color color)
        {
            array[i] = clump(SelectedColor.r * 255);
            array[i + 1] = clump(SelectedColor.g * 255);
            array[i + 2] = clump(SelectedColor.b * 255);
        }

        Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        byte[] selectedTextureRaw = sprite.texture.GetRawTextureData();
        byte[] hoveredTextureRaw = sprite.texture.GetRawTextureData();
        // paint all pixels to desired color;
        for (int i = 0; i < selectedTextureRaw.Length; i += 4)
        {
            if (selectedTextureRaw[i + 3] != 0)
            {
                paint(selectedTextureRaw, i, SelectedColor);
                paint(hoveredTextureRaw, i, HoveredColor);
            }
        }
        Texture2D selectedTexture = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, false);
        Texture2D hoveredTexture = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, false);
        
        selectedTexture.filterMode = hoveredTexture.filterMode = FilterMode.Point;

        selectedTexture.LoadRawTextureData(selectedTextureRaw);
        hoveredTexture.LoadRawTextureData(hoveredTextureRaw);
        selectedTexture.Apply();
        hoveredTexture.Apply();

        selectedSprite = new GameObject("selected_" + name);
        hoveredSprite = new GameObject("hovered_" + name);
        selectedSprite.transform.localPosition = hoveredSprite.transform.localPosition = transform.position;
        // set parents
        selectedSprite.transform.parent = transform;
        hoveredSprite.transform.parent = transform;
        // transform outline position to be behind the main sprite
        // and increase its scale
        print(selectedSprite.transform.position);

        var v = (1 + OutlineScalePercent / 100);
        selectedSprite.transform.localScale = new Vector3(transform.localScale.x * v, transform.localScale.y * v);
        hoveredSprite.transform.localScale = new Vector3(transform.localScale.x * v, transform.localScale.y * v);



        SpriteRenderer selectedRenderer = selectedSprite.AddComponent<SpriteRenderer>();
        SpriteRenderer hoveredRenderer = hoveredSprite.AddComponent<SpriteRenderer>();

        selectedRenderer.sprite = Sprite.Create(
            selectedTexture,
            sprite.rect,
            gameObject.GetComponent<SpriteRenderer>().bounds.center - transform.position,
            sprite.pixelsPerUnit);
        hoveredRenderer.sprite = Sprite.Create(
            hoveredTexture,
            sprite.rect,
            gameObject.GetComponent<SpriteRenderer>().bounds.center - transform.position,
            sprite.pixelsPerUnit);

        selectedRenderer.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        hoveredRenderer.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        selectedSprite.SetActive(false);
        hoveredSprite.SetActive(false);
    }

    void Update()
    {
        hoveredSprite.SetActive(Hovered);
        selectedSprite.SetActive(Selected);
    }
}

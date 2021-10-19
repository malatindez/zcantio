using UnityEngine;
using UnityEngine.UI;

public class TextGenerator : MonoBehaviour
{
    public float multiplier = 1f;
    public Texture2D font;
    public string text = "Example";
    public Color color;

    private Texture2D[] chars;
    private int[] widths;
    private string _bufferedText = "Example";
    private string _bufferedFont = "";
    private Color _bufferedColor = Color.white;
    // TODO
    // completely rewrite this thing to raw image data
    public Texture2D GetTexture(string str)
    {
        int rows = 0;

        int longest = 0, count = 0;
        for (int ind = 0; ind < str.Length; ind++)
        {
            int value = str[ind];
            if (value < 20 || value > 128)
                value = 128;

            value -= 32;
            count += widths[value];

            if (str[ind] == '\n' || ind == str.Length - 1)
            {
                if (count > longest)
                {
                    longest = count;
                }
                count = 0;
                rows++;
            }
        }

        Texture2D text = new Texture2D(longest, rows * 7);

        Color transparent = new Color(1, 1, 1, 0);
        for (int indy = 0; indy < text.height; indy++)
        {
            for (int indx = 0; indx < text.width; indx++)
            {
                text.SetPixel(indx, indy, transparent);
            }
        }

        int x = 0, y = text.height - 7;
        for (int ind = 0; ind < str.Length; ind++)
        {
            int value = str[ind];

            if (value == '\n')
            {
                x = 0;
                y -= 7;
                continue;
            }

            if (value < 20 || value > 128)
                value = 128;

            value -= 32;

            Texture2D tempChar = chars[value];

            for (int pixy = 0; pixy < tempChar.height; pixy++)
            {
                for (int pixx = 0; pixx < tempChar.width; pixx++)
                {
                    text.SetPixel(x + pixx, y + pixy, tempChar.GetPixel(pixx, pixy));
                }
            }

            x += tempChar.width;
        }
        text.filterMode = FilterMode.Point;
        if (text.height != 0 && text.width != 0)
            text.Apply();

        return text;
    }


    public Texture2D GetTexture2(string str)
    {
        int rows = 0;

        int counter = 0;
        int biggest = 0, biggestind = 0;
        for (int ind = 0; ind < str.Length; ind++)
        {
            counter++;

            if (str[ind] == '\n' || ind == str.Length - 1)
            {
                if (biggest < counter)
                {
                    biggest = counter;
                    biggestind = ind - biggest;
                }
                counter = 0;
                rows++;
            }
        }

        Texture2D text = new Texture2D(biggest * 9, rows * 7);

        int x = 0, y = text.height - 7;
        int biggestpixels = 0;
        for (int ind = 0; ind < str.Length; ind++)
        {
            int value = str[ind];

            if (value == '\n')
            {
                x = 0;
                y -= 7;
                continue;
            }

            if (value < 20 || value > 128)
                value = 128;

            value -= 32;

            Texture2D temp = chars[value];

            for (int pixy = 0; pixy < temp.height; pixy++)
            {
                for (int pixx = 0; pixx < temp.width; pixx++)
                {
                    text.SetPixel(x + pixx, y + pixy, temp.GetPixel(pixx, pixy));
                }
            }

            x += temp.width;
            if (ind > biggestind && ind <= biggestind + biggest)
                biggestpixels += temp.width;
        }

        Texture2D resizedText = new Texture2D(biggestpixels, rows * 7);

        for (int indy = 0; indy < resizedText.height; indy++)
        {
            for (int indx = 0; indx < resizedText.width; indx++)
            {
                
                resizedText.SetPixel(indx, indy, text.GetPixel(indx, indy));
                if (resizedText.GetPixel(indx, indy) != color && resizedText.GetPixel(indx, indy) != new Color(1,1,1,0))
                    resizedText.SetPixel(indx, indy, new Color(1, 1, 1, 0));
            }
        }
        if (resizedText.height != 0 && resizedText.width != 0)
            resizedText.Apply();
        resizedText.filterMode = FilterMode.Point;

        return resizedText;
    }

    public void UpdateText()
    {
        if (_bufferedText == text && color == _bufferedColor)
        {
            return;
        }
        _bufferedText = text;
        UpdateFont();
        RawImage rimage = this.GetComponent<RawImage>();
        rimage.texture = GetTexture(text);
        rimage.rectTransform.sizeDelta = new Vector2(rimage.texture.width * multiplier, rimage.texture.height * multiplier);
    }

    private void UpdateFont()
    {
        if(_bufferedFont == font.name && color == _bufferedColor)
        {
            return;
        }
        _bufferedFont = font.name;
        _bufferedColor = color;
        
        Sprite[] res = Resources.LoadAll<Sprite>(font.name);
        chars = new Texture2D[res.Length];
        widths = new int[res.Length];
        for (int ind = 0; ind < res.Length; ind++)
        {
            Rect rect = res[ind].rect;
            Texture2D temp = new Texture2D((int)rect.width, (int)rect.height);

            for (int y = 0; y < temp.height; y++)
            {
                for (int x = 0; x < temp.width; x++)
                {
                    Color c = res[ind].texture.GetPixel((int)rect.x + x, (int)rect.y + y);
                    if (c == Color.black)
                        c = color;

                    temp.SetPixel(x, y, c);
                }
            }

            chars[ind] = temp;
            widths[ind] = temp.width;
        }

    }
    [ContextMenu("Initialize")]
    private void Initialize()
    {
        UpdateFont();
        UpdateText();
    }
    void Start() 
    {
        Initialize();
    }
}

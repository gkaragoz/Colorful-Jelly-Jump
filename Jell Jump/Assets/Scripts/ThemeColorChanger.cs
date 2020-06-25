using System.Collections;
using System.Linq;
using UnityEngine;

public class ThemeColorChanger : MonoBehaviour
{

    [SerializeField]
    private Color[] _colors;

    [HideInInspector]
    public Renderer swatchingRenderer;

    static MaterialPropertyBlock mpb;
    static int colorShaderId;

    [SerializeField]
    private Color _currentColor;

    private void Awake()
    {
        if (mpb == null)
        {
            mpb = new MaterialPropertyBlock();
            colorShaderId = Shader.PropertyToID("_BaseColor");
        }
        if (swatchingRenderer == null)
        {
            swatchingRenderer = GetComponent<Renderer>();
        }

        mpb.SetColor(colorShaderId, _currentColor);
        swatchingRenderer.SetPropertyBlock(mpb);
    }

    private void Update()
    {
        Color c = Color.Lerp(mpb.GetColor(colorShaderId), _currentColor, Time.deltaTime);
        mpb.SetColor(colorShaderId, c);
        swatchingRenderer.SetPropertyBlock(mpb);
    }

    private Color GetRandomColor()
    {
        Color color;
        while (true)
        {
            color = _colors[Random.Range(0, _colors.Length)];

            if (color != _currentColor)
            {
                break;
            }
        }

        return color;
    }

    public void ChangeColor()
    {
        _currentColor = GetRandomColor();
    }
    public bool IsEqualTo(Color me, Color other)
    {
        return me.r == other.r && me.g == other.g && me.b == other.b && me.a == other.a;
    }

}

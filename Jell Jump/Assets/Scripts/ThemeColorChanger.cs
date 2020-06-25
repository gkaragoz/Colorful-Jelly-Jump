using System.Collections;
using System.Linq;
using UnityEngine;

public class ThemeColorChanger : MonoBehaviour
{

    [SerializeField]
    private Color[] _colors;
    [SerializeField]
    private Color[] _wallColors;

    [HideInInspector]
    public Renderer swatchingRenderer;
    [SerializeField]
    private Renderer[] _wallRenderers;

    static MaterialPropertyBlock mpbGround;
    static MaterialPropertyBlock mpbWall;
    static int colorShaderId;

    [SerializeField]
    private Color _currentColor;
    [SerializeField]
    private Color _currentColorWalls;

    private void Awake()
    {
        mpbGround = new MaterialPropertyBlock();
        mpbWall = new MaterialPropertyBlock();

        colorShaderId = Shader.PropertyToID("_BaseColor");

        swatchingRenderer = GetComponent<Renderer>();

        mpbGround.SetColor(colorShaderId, _currentColor);
        mpbWall.SetColor(colorShaderId, _currentColorWalls);

        swatchingRenderer.SetPropertyBlock(mpbGround);

        foreach (var item in _wallRenderers)
        {
            item.SetPropertyBlock(mpbWall);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeColor();
        }

        Color c = Color.Lerp(mpbGround.GetColor(colorShaderId), _currentColor, Time.deltaTime);
        mpbGround.SetColor(colorShaderId, c);
        swatchingRenderer.SetPropertyBlock(mpbGround);


        Color cWall = Color.Lerp(mpbWall.GetColor(colorShaderId), _currentColorWalls, Time.deltaTime);
        mpbWall.SetColor(colorShaderId, cWall);

        foreach (var item in _wallRenderers)
        {
            item.SetPropertyBlock(mpbWall);
        }
    }

    private Color[] GetRandomColor()
    {
        Color[] color = new Color[2];
        while (true)
        {
            int index = Random.Range(0, _colors.Length);
            color[0] = _colors[index];

            if (color[0] != _currentColor)
            {
                color[1] = _wallColors[index];
                break;
            }
        }

        return color;
    }

    public void ChangeColor()
    {
        Color[] c = GetRandomColor();

        _currentColor = c[0];
        _currentColorWalls = c[1];
    }
    public bool IsEqualTo(Color me, Color other)
    {
        return me.r == other.r && me.g == other.g && me.b == other.b && me.a == other.a;
    }

}

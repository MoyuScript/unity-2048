using UnityEngine;
using System.Collections.Generic;

class Tile
{
    private enum FgColors
    {
        White = 0xf9f6f2,
        Black = 0x776e65,
    }
    private readonly struct TileColor
    {
        public readonly int bg;
        public readonly FgColors fg;
        public TileColor(int bg, FgColors fg)
        {
            this.bg = bg;
            this.fg = fg;
        }
    }
    private static readonly Dictionary<int, TileColor> ColorMap = new()
    {
        {-1, new (0x3c3a32, FgColors.White)},
        {2, new (0xeee4da, FgColors.Black)},
        {4, new (0xede0c8, FgColors.Black)},
        {8, new (0xf2b179, FgColors.White)},
        {16, new (0xf59563, FgColors.White)},
        {32, new (0xf67c5f, FgColors.White)},
        {64, new (0xf65e3b, FgColors.White)},
        {128, new (0xedcf72, FgColors.White)},
        {256, new (0xedcc61, FgColors.White)},
        {512, new (0xedc850, FgColors.White)},
        {1024, new (0xedc53f, FgColors.White)},
        {2048, new (0xedc22e, FgColors.White)},
    };

    private static Vector3 ToUnityPosition(Vector2Int position, int z)
    {
        return new Vector3(
            position.x * 1.1f - 0.55f - 1.1f,
            -position.y * 1.1f + 0.55f + 1.1f,
            z
        );
    }

    public GameObject bgGameObject;
    public GameObject textGameObject;
    public GameObject boxGameObject;
    public int z = -3;

    private Vector2Int _position;
    public Vector2Int position
    {
        get { return _position; }
        set {
            _position = value;
            boxGameObject.name = $"Tile {value.x} {value.y} {z}";
            Vector3 newPosition = ToUnityPosition(value, z);
            iTween.MoveTo(boxGameObject, newPosition, 0.3f);
        }
    }

    private int _value = 2;
    public int value {
        get { return _value; }
        set {
            _value = value;
            TileColor tileColor = ColorMap.GetValueOrDefault(value, ColorMap[-1]);
            bgGameObject.GetComponent<SpriteRenderer>().color = new Color32(
                (byte)((tileColor.bg & 0xFF0000) >> 16),
                (byte)((tileColor.bg & 0x00FF00) >> 8),
                (byte)((tileColor.bg & 0x0000FF) >> 0),
                0xFF
            );
            var textMeshPro = textGameObject.GetComponent<TMPro.TextMeshPro>();
            textMeshPro.text = value.ToString();
            textMeshPro.color = new Color32(
                (byte)((int)tileColor.fg & 0xFF),
                (byte)(((int)tileColor.fg & 0xFF00) >> 8),
                (byte)(((int)tileColor.fg & 0xFF0000) >> 16),
                0xFF
            );
        }
    }
    public Tile(Vector2Int position, int value, GameObject tilePrefab)
    {
        boxGameObject = GameObject.Instantiate(tilePrefab);
        bgGameObject = boxGameObject.transform.Find("TileBg").gameObject;
        textGameObject = boxGameObject.transform.Find("TileNumber").gameObject;
        _position = position;
        this.value = value;
        boxGameObject.transform.position = ToUnityPosition(position, z);
        boxGameObject.transform.localScale = Vector3.zero;
        boxGameObject.name = $"Tile {position.x} {position.y}";
        iTween.ScaleTo(boxGameObject, Vector3.one, 0.3f);
    }

    public void Destroy(float delay = 0)
    {
        iTween.ScaleTo(boxGameObject, Vector3.zero, 0.3f);
        GameObject.Destroy(boxGameObject, delay);
    }

    public override string ToString()
    {
        return $"[Tile {position.x} {position.y} {value}]";
    }
}
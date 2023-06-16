using UnityEngine;
using System.Collections.Generic;

class TileGameObject
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

    private static Vector3 TransformToPosition((int x, int y) position)
    {
        var (x, y) = position;
        return new Vector3(
            x * 1.1f - 0.55f - 1.1f,
            -y * 1.1f + 0.55f + 1.1f,
            -1
        );
    }

    public GameObject bgGameObject;
    public GameObject textGameObject;
    public GameObject boxGameObject;

    private (int x, int y) _position;
    public (int x, int y) position
    {
        get { return _position; }
        set {
            _position = value;
            var (x, y) = value;
            boxGameObject.name = $"Tile {x} {y}";
            Vector3 newPosition = TransformToPosition(value);
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
    public TileGameObject((int x, int y) position, int value, GameObject tilePrefab)
    {
        boxGameObject = GameObject.Instantiate(tilePrefab);
        bgGameObject = boxGameObject.transform.Find("TileBg").gameObject;
        textGameObject = boxGameObject.transform.Find("TileNumber").gameObject;
        _position = position;
        this.value = value;
        boxGameObject.transform.position = TransformToPosition(position);
        boxGameObject.transform.localScale = Vector3.zero;
        boxGameObject.name = $"Tile {position.x} {position.y}";
        iTween.ScaleTo(boxGameObject, Vector3.one, 0.3f);
    }

    public void Destroy()
    {
        iTween.ScaleTo(boxGameObject, Vector3.zero, 0.3f);
        GameObject.Destroy(boxGameObject, 0.5f);
    }
}
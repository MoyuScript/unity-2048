#nullable enable
using System.Collections.Generic;
using UnityEngine;
class TileMap
{
    private TileMapItem?[,] _tileMap = new TileMapItem[Constants.BoardSize, Constants.BoardSize];

    public TileMap()
    { }
    public TileMap(TileMap tileMap)
    {
        for (int y = 0; y < Constants.BoardSize; y++)
        {
            for (int x = 0; x < Constants.BoardSize; x++)
            {
                if (tileMap[x, y] is null)
                {
                    continue;
                }
                this[x, y] = new(tileMap[x, y]);
            }
        }
    }

    public TileMapItem? this[int x, int y]
    {
        get { return _tileMap[y, x]; }
        set
        {
            _tileMap[y, x] = value;
        }
    }

    public override string ToString()
    {
        string result = "";
        for (int y = 0; y < Constants.BoardSize; y++)
        {
            result += "\n";
            for (int x = 0; x < Constants.BoardSize; x++)
            {
                if (this[x, y] == null)
                {
                    result += $"{"-",-5}";
                    continue;
                }
                result += $"{this[x, y]!.value,-5}";
            }
        }
        result += "\n";
        return result;
    }

    public void Rotate(int times = 1)
    {
        for (; times > 0; times--)
        {
            TileMapItem?[,] newTileMap = new TileMapItem?[Constants.BoardSize, Constants.BoardSize];
            for (int y = 0; y < Constants.BoardSize; y++)
            {
                for (int x = 0; x < Constants.BoardSize; x++)
                {
                    var tileMapItem = _tileMap[Constants.BoardSize - x - 1, y];
                    newTileMap[y, x] = tileMapItem;

                    if (tileMapItem is not null)
                    {
                        tileMapItem.position = new Vector2Int(x, y);
                    }
                }
            }

            _tileMap = newTileMap;
        }
    }

    public bool IsSameTo(TileMap tileMap)
    {
        for (int y = 0; y < Constants.BoardSize; y++)
        {
            for (int x = 0; x < Constants.BoardSize; x++)
            {
                var tile1 = this[x, y];
                var tile2 = tileMap[x, y];
                if (tile1 is null && tile2 is null)
                {
                    continue;
                }
                if (tile1 is null || tile2 is null)
                {
                    return false;
                }
                if (tile1.position != tile2.position || tile1.value != tile2.value)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public IEnumerable<TileMapItem> GetTiles()
    {
        for (int y = 0; y < Constants.BoardSize; y++)
        {
            for (int x = 0; x < Constants.BoardSize; x++)
            {
                if (this[x, y] is null)
                {
                    continue;
                }
                yield return this[x, y]!;
            }
        }
    }

    public void Commit()
    {
        foreach (var tile in GetTiles())
        {
            tile.Commit();
        }
    }

    public void Destroy()
    {
        foreach (var tile in GetTiles())
        {
            tile.isDeleted = true;
            tile.Commit();
        }
    }
}
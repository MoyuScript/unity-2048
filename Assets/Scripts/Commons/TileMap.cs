#nullable enable
using System.Collections.Generic;
class TileMap
{
    
    private Tile?[,] _tileMap = new Tile[Constants.BoardSize, Constants.BoardSize];

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

    public Tile? this[int x, int y]
    {
        get { return _tileMap[y, x]; }
        set
        {
            _tileMap[y, x] = value;
        }
    }

    public void SyncAllTile()
    {

        for (int y = 0; y < Constants.BoardSize; y++)
        {
            for (int x = 0; x < Constants.BoardSize; x++)
            {
                if (this[x, y] is null)
                {
                    continue;
                }
                this[x, y]!.Sync();
            }
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
            Tile?[,] newTileMap = new Tile?[Constants.BoardSize, Constants.BoardSize];
            for (int y = 0; y < Constants.BoardSize; y++)
            {
                for (int x = 0; x < Constants.BoardSize; x++)
                {
                    var tile = _tileMap[Constants.BoardSize - x - 1, y];
                    newTileMap[y, x] = tile;

                    if (tile is not null)
                    {
                        tile.position = (x, y);
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

    public void Destroy()
    {
        foreach (var tile in _tileMap)
        {
            tile?.tileGameObject.Destroy();
        }
    }

    public IEnumerable<Tile> GetTiles()
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
}
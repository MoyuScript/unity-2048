using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
class TileMapController
{
    public TileMap tileMap = new();
    public GameObject tilePrefab;

    public TileMapController(GameObject tilePrefab)
    {
        this.tilePrefab = tilePrefab;
    }

    public void GenerateNewTile()
    {
        List<Vector2Int> availablePosition = new();

        for (int y = 0; y < Constants.BoardSize; y++)
        {
            for (int x = 0; x < Constants.BoardSize; x++)
            {
                if (tileMap[x, y] == null)
                {
                    availablePosition.Add(new Vector2Int(x, y));
                }
            }
        }

        if (availablePosition.Count == 0)
        {
            return;
        }
        int index = UnityEngine.Random.Range(0, availablePosition.Count);
        var pos = availablePosition[index];
        Tile tile = new (pos, 2, tilePrefab);
        TileMapItem tileMapItem = new(tile);
        tileMap[pos.x, pos.y] = tileMapItem;
    }

    private (TileMap nextTileMap, List<TileMapItem> deletedTiles, List<TileMapItem> mergedTiles) GetNextTileMap(Constants.Direction moveDirection)
    {
        TileMap nextTileMap = new(tileMap);
        int rotateTimes = moveDirection switch
        {
            Constants.Direction.Left => 0,
            Constants.Direction.Right => 2,
            Constants.Direction.Up => 3,
            Constants.Direction.Down => 1,
            _ => throw new NotImplementedException(),
        };
        nextTileMap.Rotate(rotateTimes);

        List<TileMapItem> mergedTiles = new();
        List<TileMapItem> deletedTiles = new();

        // From => To
        Dictionary<TileMapItem, TileMapItem> mergedTileMap = new();

        for (int y = 0; y < Constants.BoardSize; y++)
        {
            for (int x = 0; x < Constants.BoardSize; x++)
            {
                var tile = nextTileMap[x, y];
                if (tile is null)
                {
                    continue;
                }

                int nx = x - 1;
                while (nx >= 0)
                {
                    var leftTile = nextTileMap[nx, y];

                    if (leftTile is null)
                    {
                        // Move Left
                        nextTileMap[nx, y] = tile;
                        tile.position = new Vector2Int(nx, y);
                        nextTileMap[nx + 1, y] = null;
                    }
                    else if (leftTile.value == tile.value && !mergedTiles.Contains(tile) && !mergedTiles.Contains(leftTile))
                    {
                        // Merge
                        nextTileMap[nx, y] = tile;
                        tile.position = new Vector2Int(nx, y);

                        leftTile.isDeleted = true;
                        deletedTiles.Add(leftTile);
                        nextTileMap[nx + 1, y] = null;
                        tile.value *= 2;
                        mergedTiles.Add(tile);

                        mergedTileMap.Add(tile, leftTile);
                    }
                    else
                    {
                        break;
                    }
                    nx--;
                }
            }
        }


        nextTileMap.Rotate((4 - rotateTimes) % 4);
        // Set merged tile position
        foreach (var (fromTile, toTile) in mergedTileMap)
        {
            toTile.position = fromTile.position;
        }
        return (nextTileMap, deletedTiles, mergedTiles);
    }

    public bool Move(Constants.Direction dir)
    {
        var (nextTileMap, deletedTiles, mergedTiles) = GetNextTileMap(dir);
        
        if (tileMap.IsSameTo(nextTileMap))
        {
            return false;
        }
        
        foreach (var tile in deletedTiles)
        {
            tile.Commit();
        }
        tileMap = nextTileMap;
        tileMap.Commit();

        foreach (var tile in mergedTiles)
        {
            GlobalState.instance.score += tile.value;
        }
        return true;
    }

    public bool IsGameOver()
    {
        // 检查是否可移动
        for (Constants.Direction direction = Constants.Direction.Left; direction <= Constants.Direction.Down; direction++)
        {
            var (nextTileMap, _, _) = GetNextTileMap(direction);
            if (!nextTileMap.IsSameTo(tileMap))
            {
                return false;
            }
        }
        return true;
    }

    public void Destroy()
    {
        tileMap.Destroy();
    }

    public void Print()
    {
        Debug.Log(tileMap.ToString());
    }
}
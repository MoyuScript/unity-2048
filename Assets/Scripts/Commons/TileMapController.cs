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
        List<(int x, int y)> availablePosition = new();

        for (int y = 0; y < Constants.BoardSize; y++)
        {
            for (int x = 0; x < Constants.BoardSize; x++)
            {
                if (tileMap[x, y] == null)
                {
                    availablePosition.Add((x, y));
                }
            }
        }

        if (availablePosition.Count == 0)
        {
            return;
        }
        int index = UnityEngine.Random.Range(0, availablePosition.Count);
        var pos = availablePosition[index];
        TileGameObject tileGameObject = new (pos, 2, tilePrefab);
        tileMap[pos.x, pos.y] = new Tile(tileGameObject) {
            position = pos,
            value = 2,
            isDeleted = false,
        };
    }

    private (TileMap nextTileMap, List<Tile> deletedTiles, List<Tile> mergedTiles) GetNextTileMap(Constants.Direction moveDirection)
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

        List<Tile> mergedTiles = new();
        List<Tile> deletedTiles = new();

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
                        tile.position = (nx, y);
                        nextTileMap[nx + 1, y] = null;
                    }
                    else if (leftTile.value == tile.value && !mergedTiles.Contains(tile) && !mergedTiles.Contains(leftTile))
                    {
                        // Merge
                        nextTileMap[nx, y] = tile;
                        tile.position = (nx, y);

                        leftTile.isDeleted = true;
                        deletedTiles.Add(leftTile);
                        nextTileMap[nx + 1, y] = null;
                        tile.value *= 2;
                        mergedTiles.Add(tile);
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
            tile.Sync();
        }
        tileMap = nextTileMap;
        tileMap.SyncAllTile();

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
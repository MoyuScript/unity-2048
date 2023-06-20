using UnityEngine;
class TileMapItem
{
    public Tile tile;
    public Vector2Int position;
    public int value;
    public bool isDeleted = false;
    public TileMapItem(Tile tile)
    {
        this.tile = tile;
        position = tile.position;
        value = tile.value;
    }
    public TileMapItem(TileMapItem item)
    {
        tile = item.tile;
        position = item.position;
        value = item.value;
    }
    public void Commit()
    {
        if (value != tile.value)
        {
            tile.value = value;
        }
        if (isDeleted)
        {
            tile.z = -2;
            // Move then delete
            tile.position = position;
            tile.Destroy(0.3f);
        }
        else if (tile.position != position)
        {
            // Move
            tile.position = position;
        }

    }
}
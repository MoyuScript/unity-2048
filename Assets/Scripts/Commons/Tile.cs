using UnityEngine;
class Tile
{
    public int value = 2;
    public (int x, int y) position;
    public bool isDeleted = false;
    public TileGameObject tileGameObject;

    public Tile(TileGameObject tileGameObject)
    {
        this.tileGameObject = tileGameObject;
    }

    public Tile(Tile tile)
    {
        value = tile.value;
        tileGameObject = tile.tileGameObject;
        position = tile.position;
        isDeleted = tile.isDeleted;
    }

    public override string ToString()
    {
        return $"Tile x:{position.x} y:{position.y} value:{value}";
    }

    public void Sync()
    {
        if (isDeleted)
        {
            tileGameObject.Destroy();
            return;
        }
        tileGameObject.value = value;
        tileGameObject.position = position;
    }
}

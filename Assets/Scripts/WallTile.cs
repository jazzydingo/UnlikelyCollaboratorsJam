using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Wall Tile", menuName = "Tiles/Wall Tile")]
public class WallTile : Tile
{
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            Collider2D collider = go.GetComponent<Collider2D>();
            if (collider == null)
            {
                collider = go.AddComponent<BoxCollider2D>(); // Add collider if missing
            }
        }
        return base.StartUp(position, tilemap, go);
    }
}
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace game {
    public class TilemapAddon : MonoBehaviour {
        private static readonly string RevealTextureId = "_RevealTex";
        [SerializeField] private GameObject _spritePrefab;
        [SerializeField] private Tilemap _baseTilemap;
        [SerializeField] private Tilemap _revealTilemap;
        [SerializeField] private int _pixelsPerUnit = 16;

        private BoundsInt maxBounds;

        private void Start() {
            BoundsInt baseBounds = _baseTilemap.cellBounds;
            BoundsInt revealBounds = _revealTilemap.cellBounds;

            int xMin = Math.Min(baseBounds.xMin, revealBounds.xMin);
            int yMin = Math.Min(baseBounds.yMin, revealBounds.yMin);
            int xMax = Math.Max(baseBounds.xMax, revealBounds.xMax);
            int yMax = Math.Max(baseBounds.yMax, revealBounds.yMax);

            maxBounds = new BoundsInt(xMin, yMin, 0, xMax - xMin, yMax - yMin, 0);

            Texture2D baseTexture = GenerateTilemapTexture(_baseTilemap);
            Texture2D revealTexture = GenerateTilemapTexture(_revealTilemap);

            GameObject sprite = Instantiate(_spritePrefab);
            SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = Sprite.Create(
                baseTexture, 
                new Rect(0, 0, baseTexture.width, baseTexture.height), 
                new Vector2 (0.5f, 0.5f), 
                _pixelsPerUnit
            );

            Material tempMaterial = new(spriteRenderer.sharedMaterial);

            tempMaterial.SetTexture(RevealTextureId, revealTexture);

            spriteRenderer.sharedMaterial = tempMaterial;

            gameObject.SetActive(false);
        }

        
        private Texture2D GenerateTilemapTexture(Tilemap tilemap) {
            int cellSize = _pixelsPerUnit;

            int width = cellSize * maxBounds.size.x;
            int height = cellSize * maxBounds.size.y;

            Texture2D tex = new(width, height);
            
            tex.filterMode = FilterMode.Point;

            for (int x = maxBounds.xMin; x < maxBounds.xMax; x++) {
                for (int y = maxBounds.yMin; y < maxBounds.yMax; y++) {
                    TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));


                    if (tile is Tile) {
                        Tile tileData = tile as Tile;
                        Sprite sprite = tileData.sprite;
                        
                        if (sprite) {
                            Rect spriteRect = sprite.rect;
                            Texture2D spriteTexture = sprite.texture;

                            Color[] pixels = spriteTexture.GetPixels(
                                (int)spriteRect.x, (int)spriteRect.y,
                                (int)spriteRect.width, (int)spriteRect.height
                            );

                            tex.SetPixels(
                                (x - maxBounds.xMin) * cellSize, 
                                (y - maxBounds.yMin) * cellSize, 
                                cellSize, 
                                cellSize, 
                                pixels
                            );

                        }
                    } else {
                        Color[] pixels = Enumerable.Repeat(Color.clear, cellSize * cellSize).ToArray();

                        tex.SetPixels(
                            (x - maxBounds.xMin) * cellSize, 
                            (y - maxBounds.yMin) * cellSize, 
                            cellSize, 
                            cellSize, 
                            pixels
                        );
                    }
                }
            }

            
            tex.Apply();
            return tex;
        }

    }
}

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace game {
    public class MapGenerator : MonoBehaviour {
        private static readonly string RevealTexId = "_RevealTex";
        [SerializeField] private GameObject _mapPrefab;
        [SerializeField] private Tilemap _baseTilemap;
        [SerializeField] private Tilemap _revealTilemap;
        [SerializeField] private GameObject _colliderObject;
        [SerializeField] private int _pixelsPerUnit = 16;

        private void Start() {
            BoundsInt maxBounds = GetMaxBounds();
            GameObject mapObject = Instantiate(_mapPrefab);

            SetUpTilemapGraphic(mapObject, maxBounds);
            SetUpCollider(mapObject);

            gameObject.SetActive(false);
        }

        private BoundsInt GetMaxBounds() {
            BoundsInt baseBounds = _baseTilemap.cellBounds;
            BoundsInt revealBounds = _revealTilemap.cellBounds;

            int xMin = Math.Min(baseBounds.xMin, revealBounds.xMin);
            int yMin = Math.Min(baseBounds.yMin, revealBounds.yMin);
            int xMax = Math.Max(baseBounds.xMax, revealBounds.xMax);
            int yMax = Math.Max(baseBounds.yMax, revealBounds.yMax);

            return new BoundsInt(xMin, yMin, 0, xMax - xMin, yMax - yMin, 0);
        }

        private void SetUpTilemapGraphic(GameObject mapObject, BoundsInt maxBounds) {
            Transform mapGraphic = mapObject.transform.GetChild(0);

            Texture2D mainTex = GenerateTilemapTexture(_baseTilemap, maxBounds);
            Texture2D revealTex = GenerateTilemapTexture(_revealTilemap, maxBounds);

            MeshRenderer meshRenderer = mapGraphic.GetComponent<MeshRenderer>();
            Material tempMaterial = new(meshRenderer.sharedMaterial);

            tempMaterial.mainTexture = mainTex;
            tempMaterial.SetTexture(RevealTexId, revealTex);
            meshRenderer.sharedMaterial = tempMaterial;

            mapGraphic.localPosition = maxBounds.center;
            mapGraphic.localScale = new Vector3(maxBounds.size.x, maxBounds.size.y, 0);
        }

        // TODO: Clean code smells and Inefficiencies here
        private Texture2D GenerateTilemapTexture(Tilemap tilemap, BoundsInt maxBounds) {
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

        private void SetUpCollider(GameObject mapObject) {
            Transform tilemapGrid = mapObject.transform.GetChild(1);
            _colliderObject.transform.SetParent(tilemapGrid);
            _colliderObject.GetComponent<TilemapRenderer>().enabled = false;
        }

    }
}

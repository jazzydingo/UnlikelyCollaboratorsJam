using UnityEngine;

namespace game.lights {
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererAddon : MonoBehaviour {
        private static readonly string RevealTexId = "_RevealTex";
        [SerializeField] private Sprite _revealSprite;

        // TODO: Add scaling for when main tex isn't proportional to reveal tex

        private void OnValidate() {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Material tempMaterial = new(spriteRenderer.sharedMaterial);
            Texture2D spriteTexture = GetSpriteTexture();
            
            tempMaterial.SetTexture(RevealTexId, spriteTexture);
            spriteRenderer.sharedMaterial = tempMaterial;
        }

        private Texture2D GetSpriteTexture() {
            if (!IsSpriteFromSpritesheet()) return _revealSprite.texture;

            Texture2D sourceTex = _revealSprite.texture;
            Rect spriteRect = _revealSprite.rect;

            Texture2D spriteTex = new((int)spriteRect.width, (int)spriteRect.height);

            Color[] pixels = sourceTex.GetPixels(
                (int)spriteRect.x, 
                (int)spriteRect.y, 
                (int)spriteRect.width, 
                (int)spriteRect.height
            );

            spriteTex.SetPixels(pixels);
            spriteTex.filterMode = FilterMode.Point;
            spriteTex.Apply();

            return spriteTex;
        }

        private bool IsSpriteFromSpritesheet() {
            bool hasDifferentWidths = _revealSprite.rect.width != _revealSprite.texture.width;
            bool hasDifferentHeights = _revealSprite.rect.height != _revealSprite.texture.height;
            return hasDifferentWidths || hasDifferentHeights;
        }

    }
}

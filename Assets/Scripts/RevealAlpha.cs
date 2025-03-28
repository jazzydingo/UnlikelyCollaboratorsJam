using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    

    public class RevealAlpha : MonoBehaviour
    {
        public float fadeDuration = 2f; // Time in seconds for fade-out
        private SpriteRenderer spriteRenderer;
        private CanvasGroup canvasGroup;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            canvasGroup = GetComponent<CanvasGroup>();
            StartCoroutine(FadeToZeroAlpha());
        }

        IEnumerator FadeToZeroAlpha()
        {
            float elapsedTime = 0f;

            if (spriteRenderer != null)
            {
                Color startColor = spriteRenderer.color;
                while (elapsedTime < fadeDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);
                    spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                    yield return null;
                }
            }
            else if (canvasGroup != null)
            {
                float startAlpha = canvasGroup.alpha;
                while (elapsedTime < fadeDuration)
                {
                    elapsedTime += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
                    yield return null;
                }
            }
        }
    }

}

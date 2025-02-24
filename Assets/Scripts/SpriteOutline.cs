using UnityEngine;

namespace Game 
{
public class SpriteOutline : MonoBehaviour
{
    public Color outlineColor = Color.black;
    public int outlineSize = 4;  // Thickness of the outline

    private SpriteRenderer spriteRenderer;
    private GameObject[] outlines;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Create 4 extra copies of the sprite slightly offset
        outlines = new GameObject[4];
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

        for (int i = 0; i < 4; i++)
        {
            GameObject outline = new GameObject("Outline");
            outline.transform.parent = transform;
            outline.transform.localPosition = directions[i] * 0.05f; // Adjust thickness
            outline.transform.localScale = Vector3.one;

            SpriteRenderer outlineRenderer = outline.AddComponent<SpriteRenderer>();
            outlineRenderer.sprite = spriteRenderer.sprite;
            outlineRenderer.color = outlineColor;
            outlineRenderer.sortingOrder = spriteRenderer.sortingOrder - 1; // Render behind main sprite
            outlines[i] = outline;
        }
    }
}
}

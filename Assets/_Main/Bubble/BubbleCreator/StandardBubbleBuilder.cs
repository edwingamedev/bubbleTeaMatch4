using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBubbleBuilder : IBubbleBuilder
{
    private Settings settings;
    private int bubbleAmount;

    public StandardBubbleBuilder(Settings settings)
    {
        this.settings = settings;
        bubbleAmount = settings.BubbleSettings.BubblePresets.Count;
    }



    public void Generate(int x, int y)
    {
        int bubbleIndex = Random.Range(0, bubbleAmount);

        var go = Object.Instantiate(settings.BubbleSettings.Prefab, new Vector2(x, y), Quaternion.identity);
        var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.material = new Material(settings.BubbleSettings.Shader);
        spriteRenderer.material.SetColor("_MainColor", settings.BubbleSettings.BubblePresets[bubbleIndex].mainColor);
        spriteRenderer.material.SetColor("_EyeColor", settings.BubbleSettings.BubblePresets[bubbleIndex].eyeColor);

    }
}

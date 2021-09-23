using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCreator : MonoBehaviour
{
    public Shader shader;
    public GameObject prefab;
    public List<BubbleProperties> bubbleProperties;

    private void Start()
    {
        var amount = bubbleProperties.Count;

        for (int i = 0; i < amount; i++)
        {
            var go = Instantiate(prefab, new Vector3(i, 0, 0), Quaternion.identity);
            var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.material = new Material(shader);
            spriteRenderer.material.SetColor("_MainColor", bubbleProperties[i].mainColor);
            spriteRenderer.material.SetColor("_EyeColor", bubbleProperties[i].eyeColor);
        }
    }
}

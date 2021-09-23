using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Settings settings;
    private IGridBuilder gridBuilder;
    private IBubbleBuilder bubbleBuilder;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bubbleBuilder = new StandardBubbleBuilder(settings);
        gridBuilder = new StandardGridBuilder(settings);

        gridBuilder.Build();

        bubbleBuilder.Generate(settings.SpawnPosition.x, settings.SpawnPosition.y);
        bubbleBuilder.Generate(settings.SpawnPosition.x, settings.SpawnPosition.y + 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

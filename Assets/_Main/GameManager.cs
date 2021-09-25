using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    private IGridBuilder gridBuilder;
    private IBubbleBuilder bubbleBuilder;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bubbleBuilder = new StandardBubbleBuilder(gameSettings);
        gridBuilder = new StandardGridBuilder(gameSettings);

        gridBuilder.Build();

        bubbleBuilder.Generate(gameSettings.MainBubbleSpawnPosition);
        bubbleBuilder.Generate(gameSettings.SecondaryBubbleSpawnPosition);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

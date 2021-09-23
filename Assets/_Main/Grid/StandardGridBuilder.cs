using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGridBuilder : IGridBuilder
{
    private Settings settings;
    private Grid grid;
    public Grid Grid  => grid;


    public StandardGridBuilder(Settings settings)
    {
        this.settings = settings;
    }

    public void Build()
    {
        grid = new Grid(settings.GridSize);
    }
}

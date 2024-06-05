using System.Collections;
using System.Collections.Generic;

public class MainType
{
    /// <summary>
    /// countable object type
    /// </summary>
    public enum CountObjectType
    {
        Ingradient,
        AboveObject
    }

    /// <summary>
    /// above object type
    /// </summary>
    public enum AboveObjectType
    {
        Turret, //turret can destroy obstacle or enemy
        Generator, //ingredient generator
        Collector //ingredient collector
    }
}
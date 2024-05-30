using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "New Ingredient Data", order = 1)]
public class IngredientData : ScriptableObject
{
    [Header("Information")]
    /// <summary>
    /// ingredient code
    /// 50001 = branch
    /// 50002 = plastic
    /// 50003 = iron
    /// 50004 = gold
    /// </summary>
    public int m_code = 0;

    /// <summary>
    /// object name
    /// </summary>
    public string m_name = string.Empty;

    /// <summary>
    /// object sprite
    /// </summary>
    public Sprite m_sprite = null;

    /// <summary>
    /// object explain
    /// </summary>
    [TextArea]
    public string m_explain = string.Empty;

    [Header("Setting")]
    /// <summary>
    /// generate weight
    /// it can generate object this percent of
    /// </summary>
    public int m_generateWeight = 0;

    /// <summary>
    /// generate weight change
    /// per generate speed
    /// </summary>
    public int m_weightChange = 0;

    /// <summary>
    /// if game start
    /// the amount that time
    /// </summary>
    public int m_startAmount = 0;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AboveObjectData", menuName = "New Above Object Data", order = 1)]
public class AboveObjectData : ScriptableObject
{
    [Header("Information")]
    /// <summary>
    /// obj code
    /// 30001 = motor (cant use with raft)
    /// 30002 = woodLuncher
    /// 30003 = plasticLuncher
    /// 30004 = ironLuncher
    /// 30005 = goldLuncher
    /// </summary>
    public int m_code = 0;

    /// <summary>
    /// obstacle sprite
    /// </summary>
    public Sprite m_sprite = null;

    /// <summary>
    /// object explain
    /// </summary>
    [TextArea]
    public string m_explain = string.Empty;

    /// <summary>
    /// the ingredient code
    /// it needed to build
    /// </summary>
    public List<int> m_needIngredientCode = new List<int>();

    /// <summary>
    /// the ingredient amount
    /// it needed to build
    /// </summary>
    public List<int> m_needIngredientAmount = new List<int>();
}

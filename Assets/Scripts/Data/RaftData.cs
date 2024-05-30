using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RaftData", menuName = "New Raft Data", order = 1)]
public class RaftData : ScriptableObject
{
    [Header("Information")]
    /// <summary>
    /// raft code
    /// 10001 = woodRaft
    /// 10002 = plasticRaft
    /// 10003 = ironRaft
    /// 10004 = goldRaft
    /// </summary>
    public int m_code = 0;

    /// <summary>
    /// next raft ugrade code
    /// </summary>
    public int m_upgradeCode = 0;

    /// <summary>
    /// raft name
    /// </summary>
    public string m_name = string.Empty;

    /// <summary>
    /// raft sprite
    /// </summary>
    public Sprite m_sprite = null;

    /// <summary>
    /// object explain
    /// </summary>
    [TextArea]
    public string m_explain = string.Empty;

    [Header("Setting")]
    /// <summary>
    /// raft hp9
    /// 10001 = 1
    /// 10002 = 2
    /// 10003 = 3
    /// 10004 = 4
    /// </summary>
    public int m_hp = 0;

    /// <summary>
    /// the ingredient code
    /// it needed to build or upgrade
    /// </summary>
    public List<int> m_needIngredientCode = new List<int>();

    /// <summary>
    /// the ingredient amount
    /// it needed to build or upgrade
    /// </summary>
    public List<int> m_needIngredientAmount = new List<int>();
}

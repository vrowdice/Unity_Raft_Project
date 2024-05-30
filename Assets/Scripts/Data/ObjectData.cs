using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "New Object Data", order = 1)]
public class ObjectData : ScriptableObject
{
    [Header("Information")]
    /// <summary>
    /// raft code
    /// 20001 = woodLuncher
    /// 20002 = plasticLuncher
    /// 20003 = ironLuncher
    /// 20004 = goldLuncher
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
}

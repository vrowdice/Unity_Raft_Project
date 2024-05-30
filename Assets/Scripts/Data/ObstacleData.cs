using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "New Obstacle Data", order = 1)]
public class ObstacleData : ScriptableObject
{
    [Header("Information")]
    /// <summary>
    /// obstacle code
    /// 70001 = rock
    /// 70002 = rog
    /// 70003 = boom
    /// </summary>
    public int m_code = 0;

    /// <summary>
    /// object name
    /// </summary>
    public string m_name = string.Empty;

    /// <summary>
    /// obstacle sprite
    /// </summary>
    public Sprite m_sprite = null;

    /// <summary>
    /// object explain
    /// </summary>
    [TextArea]
    public string m_explain = string.Empty;

    [Header("Setting")]
    /// <summary>
    /// destroy raft position
    /// the standard is the raft that crashed into it.
    /// </summary>
    public List<Vector2> m_destroyRaftPos = new List<Vector2>();

    /// <summary>
    /// the damege give to raft
    /// </summary>
    public int m_damage = 0;

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
}

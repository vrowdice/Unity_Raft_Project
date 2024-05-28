using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveObject : MonoBehaviour
{
    /// <summary>
    /// above raft object data
    /// </summary>
    int m_code = 0;

    /// <summary>
    /// above object sprite renderer
    /// </summary>
    SpriteRenderer m_spriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public SpriteRenderer SpriteRenderer
    {
        get { return m_spriteRenderer; }
    }
    public int Code
    {
        get { return m_code; }
        set { this.m_code = value; }
    }

}

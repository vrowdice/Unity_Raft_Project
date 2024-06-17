using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveObjBtn : MonoBehaviour
{
    /// <summary>
    /// above obj code
    /// </summary>
    int m_code = 0;

    public void Click()
    {
        if(m_code <= 30000)
        {
            return;
        }

        MainGameManager.Instance.BuildAboveObjBtn(m_code);
    }

    public int Code
    {
        get { return m_code; }
        set { m_code = value; }
    }
}

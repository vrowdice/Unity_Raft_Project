using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raft : MonoBehaviour
{
    /// <summary>
    /// above object of this raft
    /// </summary>
    [SerializeField]
    AboveObject m_aboveObject = null;

    /// <summary>
    /// sprite renderer of view object
    /// </summary>
    [SerializeField]
    SpriteRenderer m_viewSprite = null;

    /// <summary>
    /// raft slider
    /// </summary>
    Slider m_slider = null;

    /// <summary>
    /// raft x axis index data
    /// </summary>
    int m_raftXIndex = -1;

    /// <summary>
    /// raft y axis index data
    /// </summary>
    int m_raftYIndex = -1;

    /// <summary>
    /// raft data info
    /// </summary>
    int m_code = 0;

    /// <summary>
    /// raft hp
    /// </summary>
    int m_maxHp = 0;

    /// <summary>
    /// now hp
    /// </summary>
    int m_nowHp = 0;

    /// <summary>
    /// reset slider
    /// </summary>
    public void ResetSlider()
    {
        if (m_slider != null)
        {
            Destroy(m_slider.gameObject);
            m_slider = null;
        }
    }

    /// <summary>
    /// set slider values
    /// </summary>
    public void SetSlider()
    {
        if (m_nowHp < m_maxHp)
        {
            if (m_slider == null)
            {
                m_slider = Instantiate(MainGameManager.Instance.RaftSlider,
                    MainGameManager.Instance.Canvas.transform).GetComponent<Slider>();

                m_slider.gameObject.transform.position = new Vector2(
                        transform.position.x, transform.position.y - 0.5f);

                m_slider.maxValue = m_maxHp;
                m_slider.minValue = 0;
                m_slider.value = m_nowHp;

                m_slider.gameObject.GetComponentInChildren<Text>().text =
                    m_maxHp + " / " + m_nowHp;
            }
            else
            {
                m_slider.maxValue = m_maxHp;
                m_slider.minValue = 0;

                m_slider.value = m_nowHp;
                m_slider.gameObject.GetComponentInChildren<Text>().text =
                    m_maxHp + " / " + m_nowHp;
            }
        }
        else
        {
            ResetSlider();
        }
    }

    /// <summary>
    /// reset raft state
    /// </summary>
    public void ResetRaftState()
    {
        m_code = 0;
        m_maxHp = 0;
        m_nowHp = 0;

        ResetSlider();
        m_aboveObject.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public AboveObject AboveObject
    {
        get { return m_aboveObject; }
    }
    public SpriteRenderer ViewSprite
    {
        get { return m_viewSprite; }
    }
    public int RaftXIndexData
    {
        get { return m_raftXIndex; }
        set { this.m_raftXIndex = value; }
    }
    public int RaftYIndexData
    {
        get { return m_raftYIndex; }
        set { this.m_raftYIndex = value; }
    }
    public int Code
    {
        get { return m_code; }
        set { this.m_code = value; }
    }
    public int MaxRaftHp
    {
        get { return m_maxHp; }
        set { this.m_maxHp = value; }
    }

    public int RaftHp
    {
        get { return m_nowHp; }
        set
        {
            if (value >= m_maxHp)
            {
                m_nowHp = m_maxHp;
                ResetSlider();
                return;
            }
            else if (value <= 0)
            {
                m_nowHp = 0;
                MainGameManager.Instance.DestroyRaft(m_raftXIndex, m_raftYIndex);
                return;
            }

            m_nowHp = value;
            SetSlider();
        }
    }
}

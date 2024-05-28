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
    /// main game manager
    /// </summary>
    MainGameManager m_mainGameManager = null;

    /// <summary>
    /// raft slider
    /// </summary>
    Slider m_slider = null;

    /// <summary>
    /// raft x axis index data
    /// </summary>
    int m_raftXIndexData = -1;

    /// <summary>
    /// raft y axis index data
    /// </summary>
    int m_raftYIndexData = -1;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// reset slider
    /// </summary>
    void ResetSlider()
    {
        if (m_slider != null)
        {
            Destroy(m_slider.gameObject);
            m_slider = null;
        }
    }

    /// <summary>
    /// setting raft state
    /// </summary>
    /// /// <param name="argXPos">this raft x index position</param>
    /// <param name="argYPos">this raft y index position</param>
    /// <param name="argRaftDataCode">raft data code if value <= 0 raft is not here</param>
    /// <param name="argAboveObjCode">raft above object data code if value <= 0 object is not here</param>
    public void SetRaftState(int argRaftDataCode, int argAboveObjCode,
        int argXPos, int argYPos, MainGameManager argMainGameManager)
    {
        if(argXPos <= -1 || argYPos <= -1)
        {
            return;
        }

        GameManager _gManager = GameManager.Instance;
        SpriteRenderer _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        m_mainGameManager = argMainGameManager;
        m_raftXIndexData = argXPos;
        m_raftYIndexData = argYPos;

        //raft setting
        if (argRaftDataCode <= 0)
        {
            NoRaftState();
        }
        else
        {
            gameObject.SetActive(true);
            m_code = argRaftDataCode;
            m_maxHp = _gManager.GetRaftData(argRaftDataCode).m_hp;
            m_nowHp = m_maxHp;
            _spriteRenderer.sprite = _gManager.GetRaftData(argRaftDataCode).m_sprite;
        }

        //above object setting
        if (argAboveObjCode <= 0)
        {
            m_aboveObject.gameObject.SetActive(false);
        }
        else
        {
            m_aboveObject.gameObject.SetActive(true);
            m_aboveObject.Code = argAboveObjCode;
            m_aboveObject.SpriteRenderer.sprite = _gManager.GetObjData(argAboveObjCode).m_sprite;
        }
    }

    /// <summary>
    /// setting raft state
    /// </summary>
    /// <param name="argRaftDataCode">raft data code if value <= 0 raft is not here</param>
    /// <param name="argAboveObjCode">raft above object data code if value <= 0 object is not here</param>
    public void SetRaftState(int argRaftDataCode, int argAboveObjCode)
    {
        GameManager _gManager = GameManager.Instance;
        SpriteRenderer _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        //raft setting
        if (argRaftDataCode <= 0)
        {
            NoRaftState();
        }
        else
        {
            gameObject.SetActive(true);

            m_code = argRaftDataCode;
            m_maxHp = _gManager.GetRaftData(argRaftDataCode).m_hp;
            m_nowHp = m_maxHp;
            _spriteRenderer.sprite = _gManager.GetRaftData(argRaftDataCode).m_sprite;

            ResetSlider();
        }

        //above object setting
        if (argAboveObjCode <= 0)
        {
            m_aboveObject.gameObject.SetActive(false);
        }
        else
        {
            m_aboveObject.gameObject.SetActive(true);
            m_aboveObject.Code = argAboveObjCode;
            m_aboveObject.SpriteRenderer.sprite = _gManager.GetObjData(argAboveObjCode).m_sprite;
        }
    }

    /// <summary>
    /// reset raft state
    /// </summary>
    public void NoRaftState()
    {
        m_code = 0;
        m_maxHp = 0;
        m_nowHp = 0;

        ResetSlider();
        m_aboveObject.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// get damage this raft
    /// </summary>
    /// <param name="argDamage">damage</param>
    public void DamageRaft(int argDamage)
    {
        if(m_nowHp <= 0 || m_maxHp <= 0 || m_nowHp > m_maxHp)
        {
            DestroyRaft();
            return;
        }

        m_nowHp -= argDamage;

        if(m_nowHp < m_maxHp)
        {
            if(m_slider == null)
            {
                m_slider = Instantiate(m_mainGameManager.RaftSlider,
                    m_mainGameManager.Canvas.transform).GetComponent<Slider>();

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
                m_slider.value = m_nowHp;
                m_slider.gameObject.GetComponentInChildren<Text>().text =
                    m_maxHp + " / " + m_nowHp;
            }
        }

        if(m_nowHp <= 0)
        {
            DestroyRaft();
            return;
        }
    }

    /// <summary>
    /// destroy raft
    /// </summary>
    public void DestroyRaft()
    {
        SetRaftState(0, 0);
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
        get { return m_raftXIndexData; }
        set { this.m_raftXIndexData = value; }
    }
    public int RaftYIndexData
    {
        get { return m_raftYIndexData; }
        set { this.m_raftYIndexData = value; }
    }
    public int Code
    {
        get { return m_code; }
        set { this.m_code = value; }
    }
}

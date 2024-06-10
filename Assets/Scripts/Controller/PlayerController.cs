using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// player view sprite
    /// </summary>
    GameObject m_playerViewObject = null;

    /// <summary>
    /// selected raft gameobject
    /// </summary>
    GameObject m_selectRaft = null;

    /// <summary>
    /// buildFlag
    /// </summary>
    bool m_buildFlag = false;

    /// <summary>
    /// player x position
    /// </summary>
    int m_playerXPos = 0;

    /// <summary>
    /// player y position
    /// </summary>
    int m_playerYPos = 0;

    /// <summary>
    /// the raft selected x index
    /// </summary>
    int m_selectRaftX = 0;

    /// <summary>
    /// the raft selected y index
    /// </summary>
    int m_selectRaftY = 0;

    /// <summary>
    /// max player hp
    /// </summary>
    int m_maxPlayerHp = 0;

    /// <summary>
    /// max player mp
    /// </summary>
    int m_maxPlayerMp = 0;

    /// <summary>
    /// player hp
    /// </summary>
    int m_playerHp = 0;

    /// <summary>
    /// player mp
    /// </summary>
    int m_playerMp = 0;

    private void Awake()
    {
        m_playerViewObject = transform.Find("PlayerViewObject").gameObject;
        m_selectRaft = transform.Find("SelectRaft").gameObject;
    }

    private void Start()
    {
        MainGameManager.Instance.SetPlayerHpSlider();
        MainGameManager.Instance.SetPlayerMpSlider();
    }

    // Update is called once per frame
    void Update()
    {
        MoveController();
        UpgradeRaft();
    }

    /// <summary>
    /// if get ingredient
    /// </summary>
    /// <param name="argCode">ingredient code</param>
    /// <param name="argAmount">amount</param>
    public void GetIngredient(int argCode, int argAmount)
    {
        MainGameManager.Instance.GetIngredient(argCode, argAmount);
    }

    /// <summary>
    /// buildRaft
    /// </summary>
    public void UpgradeRaft()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MainGameManager.Instance.BuildRaft(m_selectRaftX, m_selectRaftY);
        }
    }
   
    
    /// <summary>
    /// set player position
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public void SetPlayerPosition(int argRaftXIndex, int argRaftYIndex)
    {
        if(argRaftXIndex > MainGameManager.Instance.MaxRaftXSize - 1 ||
           argRaftYIndex > MainGameManager.Instance.MaxRaftYSize - 1 ||
           argRaftXIndex < 0 ||
           argRaftYIndex < 0)
        {
            return;
        }

        Raft _raft = MainGameManager.Instance.GetRaft(argRaftXIndex, argRaftYIndex);
        if(_raft.Code > 10000)
        {
            m_playerXPos = argRaftXIndex;
            m_playerYPos = argRaftYIndex;
            transform.position = _raft.gameObject.transform.position;

            m_selectRaftX = m_playerXPos;
            m_selectRaftY = m_playerYPos;
            m_selectRaft.transform.position = _raft.gameObject.transform.position;
        }
        else
        {
            if(argRaftXIndex == m_selectRaftX && argRaftYIndex == m_selectRaftY)
            {
                MainGameManager.Instance.BuildRaft(m_selectRaftX, m_selectRaftY);

                m_selectRaftX = m_playerXPos;
                m_selectRaftY = m_playerYPos;

                m_selectRaft.transform.position = MainGameManager.Instance.GetRaft(m_playerXPos, m_playerYPos).
                    gameObject.transform.position;
                return;
            }
            m_selectRaftX = argRaftXIndex;
            m_selectRaftY = argRaftYIndex;

            m_selectRaft.transform.position = _raft.gameObject.transform.position;
        }
    }

    /// <summary>
    /// go remain raft if no any under raft
    /// will spwon same and close to back horizontal line
    /// if it cant will spown another last horizontal line
    /// will spwon below line 
    /// if no any raft, game is over
    /// </summary>
    public void GoRemainRaft()
    {
        for(int i = 0; i < MainGameManager.Instance.MaxRaftXSize; i++)
        {
            for(int o = 0; o < MainGameManager.Instance.MaxRaftYSize; o++)
            {
                Raft _raft = MainGameManager.Instance.GetRaft(i, m_playerYPos + o);
                if (_raft != null && _raft.Code > 10000)
                {
                    SetPlayerPosition(_raft.RaftXIndexData, _raft.RaftYIndexData);
                    return;
                }

                _raft = MainGameManager.Instance.GetRaft(i, m_playerYPos - o);
                if (_raft != null && _raft.Code > 10000)
                {
                    SetPlayerPosition(_raft.RaftXIndexData, _raft.RaftYIndexData);
                    return;
                }
            }
        }

        MainGameManager.Instance.GameOver();
    }

    /// <summary>
    /// if player got damaged
    /// </summary>
    /// <param name="argDamage">damage</param>
    public void GetDamage(int argDamage)
    {
        PlayerHp += argDamage;

        MainGameManager.Instance.SetPlayerHpSlider();
    }

    /// <summary>
    /// player move controll
    /// </summary>
    void MoveController()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetPlayerPosition(m_playerXPos, m_playerYPos - 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetPlayerPosition(m_playerXPos, m_playerYPos + 1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetPlayerPosition(m_playerXPos + 1, m_playerYPos);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetPlayerPosition(m_playerXPos - 1, m_playerYPos);
        }
    }

    public int PlayerXPos
    {
        get { return m_playerXPos; }
    }

    public int PlayerYPos
    {
        get { return m_playerYPos; }
    }

    public int MaxPlayerHp
    {
        get { return m_maxPlayerHp; }
        set { m_maxPlayerHp = value; }
    }

    public int MaxPlayerMp
    {
        get { return m_maxPlayerMp; }
        set { m_maxPlayerMp = value; }
    }

    public int PlayerHp
    {
        get { return m_playerHp; }
        set
        {
            if(value >= m_maxPlayerHp)
            {
                m_playerHp = m_maxPlayerHp;
                return;
            }
            else if(value <= 0)
            {
                m_playerHp = 0;
                return;
            }

            m_playerHp = value;
        }
    }

    public int PlayerMp
    {
        get { return m_playerMp; }
        set
        {
            if (value >= m_maxPlayerMp)
            {
                m_playerMp = m_maxPlayerMp;
                return;
            }
            else if (value <= 0)
            {
                m_playerMp = 0;
                return;
            }

            m_playerMp = value;
        }
    }
}

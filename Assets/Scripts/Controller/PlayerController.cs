using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// main game manager
    /// </summary>
    MainGameManager m_mainGameManager = null;

    /// <summary>
    /// player view sprite
    /// </summary>
    GameObject m_playerViewObject = null;

    /// <summary>
    /// selected raft gameobject
    /// </summary>
    GameObject m_selectRaft = null;

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

    private void Awake()
    {
        m_mainGameManager = GameObject.Find("MainGameManager").GetComponent<MainGameManager>();

        m_playerViewObject = transform.Find("PlayerViewObject").gameObject;
        m_selectRaft = transform.Find("SelectRaft").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveController();
        UpgradeRaft();
    }

    /// <summary>
    /// buildRaft
    /// </summary>
    public void UpgradeRaft()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_mainGameManager.UpgradeRaft(m_selectRaftX, m_selectRaftY);
        }
    }
    
    /// <summary>
    /// player move controll
    /// </summary>
    void MoveController()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
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
    
    /// <summary>
    /// set player position
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public void SetPlayerPosition(int argRaftXIndex, int argRaftYIndex)
    {
        if(argRaftXIndex > m_mainGameManager.MaxRaftXSize - 1 ||
           argRaftYIndex > m_mainGameManager.MaxRaftYSize - 1 ||
           argRaftXIndex < 0 ||
           argRaftYIndex < 0)
        {
            return;
        }

        Raft _raft = m_mainGameManager.GetRaft(argRaftXIndex, argRaftYIndex);
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
                m_mainGameManager.GetRaft(m_selectRaftX, m_selectRaftY).SetRaftState(10001, 0);

                m_selectRaftX = m_playerXPos;
                m_selectRaftY = m_playerYPos;

                m_selectRaft.transform.position = m_mainGameManager.GetRaft(m_playerXPos, m_playerYPos).
                    gameObject.transform.position;
                return;
            }
            m_selectRaftX = argRaftXIndex;
            m_selectRaftY = argRaftYIndex;

            m_selectRaft.transform.position = _raft.gameObject.transform.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    /// <summary>
    /// in game = true, else = false
    /// </summary>
    bool m_isGame = false;
    
    /// <summary>
    /// scene canvas
    /// </summary>
    GameObject m_canvas = null;

    [Header("Raft")]
    /// <summary>
    /// raft gameobject prefeb
    /// </summary>
    [SerializeField]
    GameObject m_raftObject = null;

    /// <summary>
    /// raft slider object
    /// </summary>
    [SerializeField]
    GameObject m_raftSliderObject = null;

    /// <summary>
    /// raft pivot vector3
    /// </summary>
    [SerializeField]
    Vector2 m_raftPivot = new Vector2();

    /// <summary>
    /// max raft x size
    /// </summary>
    [SerializeField]
    int m_maxRaftXSize = 0;

    /// <summary>
    /// max raft y size
    /// </summary>
    [SerializeField]
    int m_maxRaftYSize = 0;

    /// <summary>
    /// start raft x size
    /// </summary>
    [SerializeField]
    int m_startRaftXSize = 0;

    /// <summary>
    /// start raft y size
    /// </summary>
    [SerializeField]
    int m_startRaftYSize = 0;

    /// <summary>
    /// raft root object
    /// </summary>
    GameObject m_raftRoot = null;

    /// <summary>
    /// raft block data array
    /// </summary>
    Raft[,] m_raftBlockData = null;

    [Header("Player")]
    /// <summary>
    /// player first position index
    /// </summary>
    [SerializeField]
    int m_playerfirstXIndex = 0;

    /// <summary>
    /// player first position index
    /// </summary>
    [SerializeField]
    int m_playerfirstYIndex = 0;

    /// <summary>
    /// player controller
    /// </summary>
    PlayerController m_playerController = null;

    private void Awake()
    {
        m_raftRoot = GameObject.Find("RaftRoot");
        m_canvas = GameObject.Find("Canvas");
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        m_isGame = true;
    }

    private void Start()
    {
        ResetRaft();

        m_playerController.SetPlayerPosition(m_playerfirstXIndex, m_playerfirstYIndex);
    }

    /// <summary>
    /// ugrade raft
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public void UpgradeRaft(int argRaftXIndex, int argRaftYIndex)
    {
        Raft _raft = GetRaft(argRaftXIndex, argRaftYIndex);
        if (_raft.Code < 10000 || _raft == null)
        {
            return;
        }

        int _nextRaftCode = _raft.Code + 1;

        //this part is only can use earyler test version
        if (_nextRaftCode == 10002)
        {
            _raft.ViewSprite.color = Color.gray;
        }
        else if (_nextRaftCode == 10003)
        {
            _raft.ViewSprite.color = Color.yellow;
        }
        else
        {
            return;
        }

        _raft.SetRaftState(_nextRaftCode, _raft.AboveObject.Code);
    }

    /// <summary>
    /// get raft position
    /// </summary>
    /// <param name="argXIndex">raft x index</param>
    /// <param name="argYIndex">raft y index</param>
    /// <returns>raft position</returns>
    public Raft GetRaft(int argXIndex, int argYIndex)
    {
        try
        {
            return m_raftBlockData[argXIndex, argYIndex];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// generate rafts
    /// </summary>
    void ResetRaft()
    {
        m_raftBlockData = new Raft[m_maxRaftXSize, m_maxRaftYSize];

        bool[,] _firstRaft = new bool[m_maxRaftXSize, m_maxRaftYSize];
        for (int i = m_startRaftXSize / 2; i < m_startRaftXSize + m_startRaftXSize / 2; i++)
        {
            for (int o = m_startRaftYSize / 2; o < m_startRaftYSize + m_startRaftYSize / 2; o++)
            {
                _firstRaft[i, o] = true;
            }
        }

        for (int i = 0; i < m_maxRaftYSize; i++)
        {
            for (int o = 0; o < m_maxRaftXSize; o++)
            {
                GameObject _raftObj = Instantiate(m_raftObject, m_raftRoot.transform);
                _raftObj.transform.localPosition = new Vector3(o * 1.5f + m_raftPivot.x, i * -1.5f + m_raftPivot.y);

                Raft _raft = _raftObj.GetComponent<Raft>();
                m_raftBlockData[o, i] = _raft;
                if (_firstRaft[o, i])
                {
                    _raft.SetRaftState(10001, 0, o, i, this);
                }
                else
                {
                    _raft.SetRaftState(0, 0, o, i, this);
                }
            }
        }
    }

    public GameObject Canvas
    {
        get { return m_canvas; }
    }

    public GameObject RaftSlider
    {
        get { return m_raftSliderObject; }
    }

    public bool IsGame
    {
        get { return m_isGame; }
    }
    public int MaxRaftXSize
    {
        get { return m_maxRaftXSize; }
    }
    public int MaxRaftYSize
    {
        get { return m_maxRaftYSize;  }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// the ingredient count data
/// </summary>
public class IngredientCountData
{
    /// <summary>
    /// code
    /// </summary>
    public int m_code = 0;

    /// <summary>
    /// amount
    /// </summary>
    public int m_amount = 0;

    /// <summary>
    /// count text
    /// </summary>
    public Text m_text = null;

    /// <summary>
    /// set this ingredient amount
    /// </summary>
    /// <param name="argAmount">amount</param>
    /// <returns>amount to add is not could be = false, else true</returns>
    public bool SetAmount(int argAmount)
    {
        if(m_amount + argAmount < 0)
        {
            GameManager.Instance.Alert("재료가 부족합니다");
            return false;
        }

        m_amount += argAmount;
        SetTextToAmount();
        return true;
    }

    /// <summary>
    /// set ingredient image text to this amount
    /// </summary>
    public void SetTextToAmount()
    {
        m_text.text = m_amount.ToString();
    }
}

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

    [Header("Ingredient")]
    /// <summary>
    /// ingredient image
    /// </summary>
    [SerializeField]
    GameObject m_ingredientImage = null;

    /// <summary>
    /// ingredient scrollview content
    /// </summary>
    [SerializeField]
    GameObject m_ingredientScrollViewContent = null;

    /// <summary>
    /// ingredient count dictionaty
    /// </summary>
    Dictionary<int, IngredientCountData> m_ingredientCountDic = new Dictionary<int, IngredientCountData>();

    [Header("Above Object")]
    /// <summary>
    /// above object scrollview content
    /// </summary>
    [SerializeField]
    GameObject m_aboveObjectScrollViewContent = null;

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

        SetIngredientScrollView();
    }

    /// <summary>
    /// ugrade raft
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public void BuildRaft(int argRaftXIndex, int argRaftYIndex)
    {
        Raft _raft = GetRaft(argRaftXIndex, argRaftYIndex);

        if (_raft == null)
        {
            return;
        }

        int _codeToChange = 10001;
        if (_raft.Code >= 10001)
        {
            _codeToChange = _raft.Code + 1;
        }
        RaftData _data = GameManager.Instance.GetRaftData(_codeToChange);
        if(_data == null ||
            _data.m_needIngredientCode.Count != _data.m_needIngredientAmount.Count)
        {
            return;
        }

        for(int i = 0; i < _data.m_needIngredientCode.Count; i++)
        {
            if (!m_ingredientCountDic[_data.m_needIngredientCode[i]].SetAmount(_data.m_needIngredientAmount[i]))
            {
                return;
            }
        }

        if (_codeToChange == 10002)
        {
            _raft.ViewSprite.color = Color.gray;
        }
        else if (_codeToChange == 10003)
        {
            _raft.ViewSprite.color = Color.yellow;
        }
        
        _raft.SetRaftState(_codeToChange, _raft.AboveObject.Code);
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

        //first raft setting
        bool[,] _firstRaft = new bool[m_maxRaftXSize, m_maxRaftYSize];
        for (int i = m_startRaftXSize / 2; i < m_startRaftXSize + m_startRaftXSize / 2; i++)
        {
            for (int o = m_startRaftYSize / 2; o < m_startRaftYSize + m_startRaftYSize / 2; o++)
            {
                _firstRaft[i, o] = true;
            }
        }

        //raft generate
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

    /// <summary>
    /// if get ingredient
    /// </summary>
    /// <param name="argCode">ingredient code</param>
    /// <param name="argAmount">amount</param>
    public void GetIngredient(int argCode, int argAmount)
    {
        m_ingredientCountDic[argCode].m_amount += argAmount;
        m_ingredientCountDic[argCode].SetTextToAmount();
    }

    /// <summary>
    /// ingredient scroll view setting
    /// </summary>
    void SetIngredientScrollView()
    {
        foreach (KeyValuePair<int, IngredientData> val in GameManager.Instance.IngredientDic)
        {
            IngredientCountData _data = new IngredientCountData();
            _data.m_code = val.Key;
            _data.m_amount = val.Value.m_startAmount;

            GameObject _object = Instantiate(m_ingredientImage, m_ingredientScrollViewContent.transform);
            _object.GetComponent<Image>().sprite = val.Value.m_sprite;

            _data.m_text = _object.GetComponentInChildren<Text>();
            _data.SetTextToAmount();

            m_ingredientCountDic.Add(val.Key, _data);
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

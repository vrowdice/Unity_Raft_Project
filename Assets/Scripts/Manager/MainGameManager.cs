using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// the ingredient count data
/// </summary>
public class CountData
{
    /// <summary>
    /// count object type
    /// </summary>
    public MainType.CountObjectType m_objectType;

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
    /// game manager
    /// </summary>
    static MainGameManager g_mainGameManager;

    /// <summary>
    /// in game = true, else = false
    /// </summary>
    bool m_isGame = false;

    /// <summary>
    /// in game score
    /// </summary>
    long m_score = 0;
    
    /// <summary>
    /// scene canvas
    /// </summary>
    GameObject m_canvas = null;

    [Header("Game Play")]
    /// <summary>
    /// this score add per second
    /// </summary>
    [SerializeField]
    int m_secScore = 0;

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
    /// player max hp
    /// </summary>
    [SerializeField]
    int m_playerMaxHp = 0;

    /// <summary>
    /// player max mp
    /// </summary>
    [SerializeField]
    int m_playerMaxMp = 0;

    /// <summary>
    /// time scale down flag
    /// (skill flag)
    /// </summary>
    bool m_skillFlag = false;

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
    Dictionary<int, CountData> m_ingredientCountDic = new Dictionary<int, CountData>();

    [Header("Above Object")]
    /// <summary>
    /// above object image UI
    /// </summary>
    [SerializeField]
    GameObject m_aboveObjectImage = null;

    /// <summary>
    /// above object scrollview content
    /// </summary>
    [SerializeField]
    GameObject m_aboveObjectScrollViewContent = null;

    /// <summary>
    /// ingredient count dictionaty
    /// </summary>
    Dictionary<int, CountData> m_aboveObjectCountDic = new Dictionary<int, CountData>();

    [Header("Player UI")]
    /// <summary>
    /// player hp slider
    /// </summary>
    [SerializeField]
    Slider m_playerHpSlider = null;

    /// <summary>
    /// player mp slider
    /// </summary>
    [SerializeField]
    Slider m_playerMpSlider = null;

    /// <summary>
    /// player hp slider text
    /// </summary>
    [SerializeField]
    Text m_playerHpText = null;

    /// <summary>
    /// player mp slider text
    /// </summary>
    [SerializeField]
    Text m_playerMpText = null;

    /// <summary>
    /// player money text
    /// </summary>
    [SerializeField]
    Text m_moneyText = null;

    /// <summary>
    /// score text
    /// </summary>
    [SerializeField]
    Text m_scoreText = null;

    private void Awake()
    {
        g_mainGameManager = this;

        m_raftRoot = GameObject.Find("RaftRoot");
        m_canvas = GameObject.Find("Canvas");
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        m_playerController.MaxPlayerHp = m_playerMaxHp;
        m_playerController.PlayerHp = m_playerMaxHp;

        m_playerController.MaxPlayerMp = m_playerMaxMp;
        m_playerController.PlayerMp = m_playerMaxMp;
        m_isGame = true;
    }

    private void Start()
    {
        ResetRaft();

        m_playerController.SetPlayerPosition(m_playerfirstXIndex, m_playerfirstYIndex);

        SetIngredientScrollView();

        SetPlayerMoneyText(GameManager.Instance.Money);

        InvokeRepeating("SecScore", 0.0f, 0.1f);
    }

    /// <summary>
    /// setting raft state
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    /// <param name="argRaftCode">raft data code if value <= 0 raft is not here</param>
    /// <param name="argAboveObjCode">raft above object data code if value <= 0 object is not here</param>
    public void SetRaftState(int argRaftCode, int argAboveObjCode,
        int argRaftXIndex, int argRaftYIndex)
    {
        if (argRaftXIndex <= -1 || argRaftYIndex <= -1)
        {
            return;
        }

        GameManager _gManager = GameManager.Instance;

        Raft _raft = GetRaft(argRaftXIndex, argRaftYIndex);
        SpriteRenderer _spriteRenderer = _raft.ViewSprite;

        _raft.RaftXIndexData = argRaftXIndex;
        _raft.RaftYIndexData = argRaftYIndex;

        //raft setting
        if (argRaftCode <= 0)
        {
            _raft.ResetRaftState();
        }
        else
        {
            _raft.gameObject.SetActive(true);
            _raft.Code = argRaftCode;
            _raft.MaxRaftHp = _gManager.GetRaftData(argRaftCode).m_hp;
            _raft.RaftHp = _raft.MaxRaftHp;

            _raft.SetSlider();
            _spriteRenderer.sprite = _gManager.GetRaftData(argRaftCode).m_sprite;
        }

        //above object setting
        if (argAboveObjCode <= 0)
        {
            _raft.AboveObject.gameObject.SetActive(false);
        }
        else
        {
            _raft.AboveObject.gameObject.SetActive(true);
            _raft.AboveObject.Code = argAboveObjCode;
            _raft.AboveObject.SpriteRenderer.sprite = _gManager.GetObjData(argAboveObjCode).m_sprite;
        }

        //not required once the raft image is defined
        _raft.ViewSprite.color = GetColor(argRaftCode);
    }

    /// <summary>
    /// generate rafts at first
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
                    SetRaftState(10001, 0, o, i);
                }
                else
                {
                    SetRaftState(0, 0, o, i);
                }
            }
        }
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
        
        SetRaftState(_codeToChange, _raft.AboveObject.Code, argRaftXIndex, argRaftYIndex);
    }

    /// <summary>
    /// get damage this raft
    /// </summary>
    /// <param name="argDamage">damage</param>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public void DamageRaft(int argDamage, int argRaftXIndex, int argRaftYIndex)
    {
        Raft _raft = GetRaft(argRaftXIndex, argRaftYIndex);

        if(_raft == null)
        {
            return;
        }

        if (_raft.RaftHp <= 0 || _raft.MaxRaftHp <= 0 || _raft.RaftHp > _raft.MaxRaftHp)
        {
            DestroyRaft(argRaftXIndex, argRaftYIndex);
            return;
        }

        _raft.RaftHp -= argDamage;

        _raft.SetSlider();

        if (_raft.RaftHp <= 0)
        {
            DestroyRaft(argRaftXIndex, argRaftYIndex);
            return;
        }
    }

    /// <summary>
    /// destroy raft
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public void DestroyRaft(int argRaftXIndex, int argRaftYIndex)
    {
        SetRaftState(0, 0, argRaftXIndex, argRaftYIndex);

        if(m_playerController.PlayerXPos == argRaftXIndex &&
            m_playerController.PlayerYPos == argRaftYIndex)
        {
            m_playerController.GetDamage(-20);
            m_playerController.GoRemainRaft();
        }
    }

    /// <summary>
    /// get score
    /// </summary>
    public void GetScore(long argScore)
    {
        if(m_score + argScore <= 0)
        {
            m_score = 0;
            m_scoreText.text = m_score.ToString();
            return;
        }
        m_score += argScore;

        m_scoreText.text = m_score.ToString();
    }

    /// <summary>
    /// game over
    /// </summary>
    public void GameOver()
    {
        GameManager.Instance.ChangeScene("Title");
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
    /// player hp setting
    /// </summary>
    /// <param name="argToSetValue">value to setting</param>
    public void SetPlayerHpSlider()
    {
        m_playerController.MaxPlayerHp = m_playerMaxHp;

        m_playerHpSlider.maxValue = m_playerMaxHp;
        m_playerHpSlider.minValue = 0;

        m_playerHpSlider.value = m_playerController.PlayerHp;

        m_playerHpText.text = "HP " +  m_playerController.PlayerHp + " / " +
            m_playerController.MaxPlayerHp;
    }


    /// <summary>
    /// player mp setting
    /// </summary>
    /// <param name="argToSetValue">value to setting</param>
    public void SetPlayerMpSlider()
    {
        m_playerController.MaxPlayerMp = m_playerMaxMp;

        m_playerMpSlider.maxValue = m_playerMaxMp;
        m_playerMpSlider.minValue = 0;

        m_playerMpSlider.value = m_playerController.PlayerMp;

        m_playerMpText.text = "MP " + m_playerController.PlayerMp + " / " +
            m_playerController.MaxPlayerMp;
    }

    /// <summary>
    /// player money setting
    /// and return data to game manager
    /// </summary>
    /// <param name="argValue"></param>
    public void SetPlayerMoneyText(long argValue)
    {
        GameManager.Instance.Money += argValue;

        m_moneyText.text = GameManager.Instance.Money.ToString();
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
    /// set color rogic
    /// this part will dont need after make raft and ingredient sprite
    /// </summary>
    /// <param name="argCode">raft, ingredient code</param>
    /// <returns>color</returns>
    public Color GetColor(int argCode)
    {
        int _colorCode = argCode % 10000;

        switch (_colorCode)
        {
            case 1:
                return new Color(196f / 255f, 140f / 255f, 80f / 255f);
            case 2:
                return new Color(112f / 255f, 221f / 255f, 255f / 255f);
            case 3:
                return Color.gray;
            case 4:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    /// <summary>
    /// ingredient scroll view setting
    /// </summary>
    void SetIngredientScrollView()
    {
        foreach (KeyValuePair<int, IngredientData> val in GameManager.Instance.IngredientDic)
        {
            CountData _data = new CountData();
            _data.m_objectType = MainType.CountObjectType.Ingradient;
            _data.m_code = val.Key;
            _data.m_amount = val.Value.m_startAmount;

            GameObject _object = Instantiate(m_ingredientImage, m_ingredientScrollViewContent.transform);
            _object.GetComponent<Image>().sprite = val.Value.m_sprite;
            _object.GetComponent<Image>().color = GetColor(val.Key);

            _data.m_text = _object.GetComponentInChildren<Text>();
            _data.SetTextToAmount();

            m_ingredientCountDic.Add(val.Key, _data);
        }
    }

    /// <summary>
    /// add score per second
    /// </summary>
    void SecScore()
    {
        GetScore(m_secScore);
    }

    /// <summary>
    /// instance
    /// </summary>
    public static MainGameManager Instance
    {
        get
        {
            return g_mainGameManager;
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
    public bool SkillFlag
    {
        get { return m_skillFlag; }
        set { m_skillFlag = value; }
    }
}

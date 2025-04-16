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
        if (m_amount + argAmount < 0)
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
    /// player controller
    /// </summary>
    PlayerController m_playerController = null;

    /// <summary>
    /// float generator
    /// </summary>
    FloatGenerator m_floatGenerator = null;

    /// <summary>
    /// time scale down flag
    /// (skill flag)
    /// </summary>
    bool m_skillFlag = false;

    /// <summary>
    /// scene canvas
    /// </summary>
    GameObject m_canvas = null;

    [Header("Game Play")]

    /// <summary>
    /// game over gage
    /// </summary>
    [SerializeField]
    Slider m_gameOverGage = null;

    /// <summary>
    /// clear gage
    /// </summary>
    [SerializeField]
    Slider m_clearGage = null;

    /// <summary>
    /// clear gage text
    /// </summary>
    [SerializeField]
    Text m_clearGageText = null;

    /// <summary>
    /// max clear distance
    /// </summary>
    [SerializeField]
    float m_maxClearDistance = 0;

    /// <summary>
    /// this score add per second
    /// </summary>
    [SerializeField]
    int m_secScore = 0;

    /// <summary>
    /// clear distance
    /// </summary>
    float m_clearDistance = 0.0f;

    /// <summary>
    /// clear distance change
    /// </summary>
    float m_clearDistanceChange = 0.1f;

    /// <summary>
    /// motor count
    /// </summary>
    int m_motorCount = 0;

    /// <summary>
    /// in game score
    /// </summary>
    long m_score = 0;

    /// <summary>
    /// in game = true, else = false
    /// </summary>
    bool m_isGame = false;

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
    GameObject m_aboveObjectBtn = null;

    /// <summary>
    /// above object scrollview content
    /// </summary>
    [SerializeField]
    GameObject m_aboveObjectScrollViewContent = null;

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
    /// build delay
    /// </summary>
    [SerializeField]
    Slider m_buildSlider = null;

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

    [Header("Filter UI")]
    /// <summary>
    /// time scale down filter animatior
    /// </summary>
    [SerializeField]
    Animator m_timeScaleDownFilterAni = null;

    private void Awake()
    {
        g_mainGameManager = this;

        m_raftRoot = GameObject.Find("RaftRoot");
        m_canvas = GameObject.Find("Canvas");
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        m_floatGenerator = GameObject.Find("FloatGenerator").GetComponent<FloatGenerator>();

        m_isGame = true;
    }

    private void Start()
    {
        ResetRaft();
        ResetAboveObj();

        ResetIngredientScrollView();
        ResetAboveObjScrollView();

        SetPlayerMoneyText(GameManager.Instance.Money);
        SetClearGamveOverGage();

        InvokeRepeating("SecScore", 1.0f, 1.0f);
    }

    /// <summary>
    /// setting raft state
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    /// <param name="argRaftCode">raft data code if value <= 0 raft is not here</param>
    public void SetRaftState(int argRaftCode, int argRaftXIndex, int argRaftYIndex)
    {
        if (argRaftXIndex <= -1 || argRaftYIndex <= -1)
        {
            return;
        }

        //get data
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

        _raft.ViewSprite.color = GetColor(argRaftCode);
    }

    /// <summary>
    /// ugrade raft
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public bool BuildRaft(int argRaftXIndex, int argRaftYIndex)
    {
        Raft _raft = GetRaft(argRaftXIndex, argRaftYIndex);
        if (_raft == null)
        {
            return false;
        }

        int _codeToChange = 10001;
        if (_raft.Code >= 10001)
        {
            _codeToChange = _raft.Code + 1;
        }

        RaftData _data = GameManager.Instance.GetRaftData(_codeToChange);
        if (_data == null)
        {
            return false;
        }

        if(!UseIngredient(_data.m_needIngredientCode, _data.m_needIngredientAmount))
        {
            GameManager.Instance.Alert("재료가 부족합니다");
            return false;
        }
        
        SetRaftState(_codeToChange, argRaftXIndex, argRaftYIndex);
        SetAboveObjectState(_raft.AboveObject.Code, argRaftXIndex, argRaftYIndex);

        return true;
    }

    /// <summary>
    /// get damage this raft
    /// </summary>
    /// <param name="argDamage">damage</param>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public void DamageRaft(int argDamage, int argRaftXIndex, int argRaftYIndex)
    {
        SoundManager.Instance.RaftDamageSound.Play();

        Raft _raft = GetRaft(argRaftXIndex, argRaftYIndex);

        if (_raft == null || _raft.Code <= 10000)
        {
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
        SoundManager.Instance.RaftDestroyAudio.Play();

        if(GetRaft(argRaftXIndex, argRaftYIndex).AboveObject.Code == 30001)
        {
            MotorCount--;
        }
        SetAboveObjectState(0, argRaftXIndex, argRaftYIndex);

        SetRaftState(0, argRaftXIndex, argRaftYIndex);

        if (m_playerController.PlayerXPos == argRaftXIndex &&
            m_playerController.PlayerYPos == argRaftYIndex)
        {
            m_playerController.GetDamage(-20);
            m_playerController.GoRemainRaft();
        }
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

    public void SetAboveObjectState(int argAboveObjCode, int argRaftXIndex, int argRaftYIndex)
    {
        GameManager _gManager = GameManager.Instance;
        Raft _raft = GetRaft(argRaftXIndex, argRaftYIndex);
        if (_raft.Code < 10000)
        {
            return;
        }

        //above object setting
        switch (argAboveObjCode)
        {
            case 30001:
                MotorCount++;
                break;
            case 30002:
                break;
            case 30003:
                break;
            case 30004:
                break;
            case 30005:
                break;
            default:
                _raft.AboveObject.gameObject.SetActive(false);
                _raft.AboveObject.Code = argAboveObjCode;
                return;
        }

        _raft.AboveObject.gameObject.SetActive(true);
        _raft.AboveObject.Code = argAboveObjCode;
        _raft.AboveObject.SpriteRenderer.sprite = _gManager.GetAboveObjData(argAboveObjCode).m_sprite;
    }

    /// <summary>
    /// build above obj button
    /// </summary>
    /// <param name="argCode">above obj code</param>
    public void BuildAboveObjBtn(int argCode)
    {
        if(argCode <= 30000)
        {
            return;
        }

        if(!CheckIngredient(
            GameManager.Instance.GetAboveObjData(argCode).m_needIngredientCode,
            GameManager.Instance.GetAboveObjData(argCode).m_needIngredientAmount))
        {
            GameManager.Instance.Alert("재료가 부족합니다");
            return;
        }

        m_playerController.NowAboveObjCode = argCode;
        m_playerController.BuildAboveObj();
    }

    /// <summary>
    /// get score
    /// </summary>
    public void GetScore(long argScore)
    {
        if (m_score + argScore <= 0)
        {
            m_score = 0;
            m_scoreText.text = m_score.ToString();
            return;
        }
        m_score += argScore;

        m_scoreText.text = m_score.ToString();
    }

    /// <summary>
    /// change max time flow
    /// </summary>
    public void ChangeMaxTime()
    {
        m_isGame = true;

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }

    /// <summary>
    /// game over
    /// </summary>
    public void GameOver()
    {
        GameManager.Instance.ChangeScene("Title");
    }

    /// <summary>
    /// player hp setting
    /// </summary>
    /// <param name="argToSetValue">value to setting</param>
    public void SetPlayerHpSlider()
    {
        m_playerHpSlider.maxValue = m_playerController.PlayerMaxHp;

        m_playerHpSlider.minValue = 0;

        m_playerHpSlider.value = m_playerController.PlayerHp;

        m_playerHpText.text = "HP " + m_playerController.PlayerHp + " / " +
            m_playerController.PlayerMaxHp;
    }


    /// <summary>
    /// player mp setting
    /// </summary>
    /// <param name="argToSetValue">value to setting</param>
    public void SetPlayerMpSlider()
    {
        m_playerMpSlider.maxValue = m_playerController.PlayerMaxMp;

        m_playerMpSlider.minValue = 0;

        m_playerMpSlider.value = m_playerController.PlayerMp;

        m_playerMpText.text = "MP " + m_playerController.PlayerMp + " / " +
            m_playerController.PlayerMaxMp;
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
    /// check ingredient amount
    /// </summary>
    /// <param name="argCode">code</param>
    /// <param name="argAmount">amount</param>
    public bool CheckIngredient(List<int> argCodeList, List<int> argAmountList)
    {
        if (argCodeList.Count != argAmountList.Count)
        {
            Debug.LogError("code and amount is not match!");
            return false;
        }

        for (int i = 0; i < argCodeList.Count; i++)
        {
            if (m_ingredientCountDic[argCodeList[i]].m_amount < -argAmountList[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// use ingredient
    /// </summary>
    /// <param name="argCode">code</param>
    /// <param name="argAmount">amount</param>
    public bool UseIngredient(List<int> argCodeList, List<int> argAmountList)
    {
        if (argCodeList.Count != argAmountList.Count)
        {
            Debug.LogError("code and amount is not match!");
            return false;
        }

        for (int i = 0; i < argCodeList.Count; i++)
        {
            if (!m_ingredientCountDic[argCodeList[i]].SetAmount(argAmountList[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// generate rafts at first
    /// </summary>
    void ResetRaft()
    {
        m_raftBlockData = new Raft[m_maxRaftXSize, m_maxRaftYSize];

        //first raft setting
        bool[,] _firstRaft = new bool[m_maxRaftXSize, m_maxRaftYSize];
        for (int i = 0; i < m_startRaftXSize; i++)
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
                    SetRaftState(10001, o, i);
                }
                else
                {
                    SetRaftState(0, o, i);
                }
            }
        }
    }

    /// <summary>
    /// reset above object
    /// </summary>
    void ResetAboveObj()
    {
        bool[,] _firstRaft = new bool[m_maxRaftXSize, m_maxRaftYSize];
        int _pos = m_startRaftYSize / 2 + 1;

        SetAboveObjectState(30001, 0, _pos);
    }

    /// <summary>
    /// ingredient scroll view setting
    /// </summary>
    void ResetIngredientScrollView()
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
    /// ingredient scroll view setting
    /// </summary>
    void ResetAboveObjScrollView()
    {
        foreach (KeyValuePair<int, AboveObjectData> val in GameManager.Instance.AboveObjDic)
        {
            GameObject _object = Instantiate(m_aboveObjectBtn, m_aboveObjectScrollViewContent.transform);
            _object.GetComponent<Image>().sprite = val.Value.m_sprite;
            _object.GetComponent<AboveObjBtn>().Code = val.Key;
        }
    }

    /// <summary>
    /// setting clear, game over gage
    /// </summary>
    void SetClearGamveOverGage()
    {
        m_gameOverGage.minValue = 0;
        m_gameOverGage.maxValue = m_maxClearDistance;
        m_gameOverGage.value = 0;

        m_clearGage.minValue = 0;
        m_clearGage.maxValue = m_maxClearDistance;
        m_clearGage.value = 0;

        InvokeRepeating("ClearGage", 0.0f, 0.1f);
    }

    /// <summary>
    /// get clear gage
    /// </summary>
    void ClearGage()
    {
        m_clearDistance += m_clearDistanceChange;

        m_clearGage.value = m_clearDistance;
        m_clearGageText.text = m_clearDistance + " / " + m_maxClearDistance;

        if (m_clearDistance >= m_maxClearDistance)
        {
            GameOver();
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
    public Animator TimeScaleDownFilterAni
    {
        get { return m_timeScaleDownFilterAni; }
    }
    public Slider BuildSlider
    {
        get { return m_buildSlider; }
    }
    public bool IsGame
    {
        get { return m_isGame; }
        set { m_isGame = value; }
    }
    public bool SkillFlag
    {
        get { return m_skillFlag; }
        set { m_skillFlag = value; }
    }
    public int MotorCount
    {
        get { return m_motorCount; }
        set
        {
            m_motorCount = value;

            m_clearDistanceChange = 0.1f + m_motorCount * 0.02f;
            m_floatGenerator.FloatSpeed = m_floatGenerator.FirstFloatSpeed + m_motorCount * 0.2f;
        }
    }
    public int MaxRaftXSize
    {
        get { return m_maxRaftXSize; }
    }
    public int MaxRaftYSize
    {
        get { return m_maxRaftYSize; }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Common")]
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
    /// player view sprite
    /// </summary>
    GameObject m_playerViewObject = null;

    /// <summary>
    /// selected raft gameobject
    /// </summary>
    GameObject m_selectRaft = null;

    /// <summary>
    /// repair flag
    /// </summary>
    bool m_repairFlag = true;

    /// <summary>
    /// raft build = true
    /// above obj build = false
    /// </summary>
    bool m_raftBuildFlag = true;

    /// <summary>
    /// above object code to build
    /// </summary>
    int m_nowAboveObjCode = 0;

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

    [Header("Build")]
    /// <summary>
    /// max value of build gage
    /// </summary>
    [SerializeField]
    int m_maxBuildGage = 0;

    /// <summary>
    /// build delay
    /// </summary>
    [SerializeField]
    float m_buildDelay = 0.0f;

    /// <summary>
    /// build flag
    /// </summary>
    bool m_buildFlag = true;

    [Header("Hp")]
    /// <summary>
    /// player max hp
    /// </summary>
    [SerializeField]
    int m_playerMaxHp = 0;

    /// <summary>
    /// player hp
    /// </summary>
    int m_playerHp = 0;

    [Header("Mp")]
    /// <summary>
    /// player max mp
    /// </summary>
    [SerializeField]
    int m_playerMaxMp = 0;

    /// <summary>
    /// mp recovery value
    /// </summary>
    [SerializeField]
    int m_mpRecoveryValue = 0;

    /// <summary>
    /// mp recovery delay
    /// </summary>
    [SerializeField]
    float m_mpRecoveryDelay = 0.0f;

    /// <summary>
    /// player mp
    /// </summary>
    int m_playerMp = 0;

    [Header("Skill")]
    /// <summary>
    /// raft repair speed
    /// </summary>
    [SerializeField]
    float m_repairDelay = 0.0f;

    /// <summary>
    /// repair strength
    /// </summary>
    [SerializeField]
    int m_repairStrength = 0;

    /// <summary>
    /// skill is on float speed setting
    /// this value never be zero
    /// </summary>
    [SerializeField]
    float m_skillOnSpeed = 0.0f;

    /// <summary>
    /// skill on mp use delay
    /// </summary>
    [SerializeField]
    float m_skillOnMpUseDelay = 0.0f;

    /// <summary>
    /// mp used by warp
    /// </summary>
    [SerializeField]
    int m_warpMpUse = 0;

    private void Awake()
    {
        m_playerViewObject = transform.Find("PlayerViewObject").gameObject;
        m_selectRaft = transform.Find("SelectRaft").gameObject;

        m_playerHp = m_playerMaxHp;
        m_playerMp = m_playerMaxMp;
    }

    private void Start()
    {
        MainGameManager.Instance.SetPlayerHpSlider();
        MainGameManager.Instance.SetPlayerMpSlider();
        BuildGageSetting();

        SetPlayerPosition(m_playerfirstXIndex, m_playerfirstYIndex);
        InvokeRepeating("MpRecover", m_mpRecoveryDelay, m_mpRecoveryDelay);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerkeyboardInput();
    }

    /// <summary>
    /// if get ingredient
    /// </summary>
    /// <param name="argCode">ingredient code</param>
    /// <param name="argAmount">amount</param>
    public void GetIngredient(int argCode, int argAmount)
    {
        SoundManager.Instance.IngredientGetSound.Play();

        MainGameManager.Instance.GetIngredient(argCode, argAmount);
    }
   
    /// <summary>
    /// get money
    /// </summary>
    /// <param name="argValue">money amount</param>
    public void GetMoney(int argValue)
    {
        SoundManager.Instance.MoneyGetSound.Play();

        MainGameManager.Instance.SetPlayerMoneyText(argValue);
    }

    public void BuildAboveObj()
    {
        if(MainGameManager.Instance.GetRaft(m_selectRaftX, m_selectRaftY).Code < 10000)
        {
            SetSelectRaftPos(m_playerXPos, m_playerYPos);
            GameManager.Instance.Alert("뗏목 위가 아니면 설치할 수 없습니다!");
            return;
        }

        if (!m_buildFlag)
        {
            ResetInvoke();
        }
        else
        {
            SoundManager.Instance.BuildSound.Play();

            m_buildFlag = false;
            m_raftBuildFlag = false;

            MainGameManager.Instance.BuildSlider.gameObject.SetActive(true);
            MainGameManager.Instance.BuildSlider.transform.position =
                new Vector2(m_selectRaft.transform.position.x, m_selectRaft.transform.position.y + 0.5f);

            InvokeRepeating("BuildGage", m_buildDelay, m_buildDelay);
        }
    }

    /// <summary>
    /// set player position
    /// </summary>
    /// <param name="argRaftXIndex">raft x index</param>
    /// <param name="argRaftYIndex">raft y index</param>
    public void SetPlayerPosition(int argRaftXIndex, int argRaftYIndex)
    {
        //exeption
        if(argRaftXIndex > MainGameManager.Instance.MaxRaftXSize - 1 ||
           argRaftYIndex > MainGameManager.Instance.MaxRaftYSize - 1 ||
           argRaftXIndex < 0 ||
           argRaftYIndex < 0)
        {
            return;
        }

        //get now position raft data
        Raft _raft = MainGameManager.Instance.GetRaft(argRaftXIndex, argRaftYIndex);
        if(_raft.Code > 10000)
        {
            //set position
            m_playerXPos = argRaftXIndex;
            m_playerYPos = argRaftYIndex;
            transform.position = _raft.gameObject.transform.position;

            SetSelectRaftPos(argRaftXIndex, argRaftYIndex);
        }
        else
        {
            //select empty space or build raft
            if (argRaftXIndex == m_selectRaftX && argRaftYIndex == m_selectRaftY)
            {
                BuildRaft();
                return;
            }
            SetSelectRaftPos(argRaftXIndex, argRaftYIndex);
        }
    }

    /// <summary>
    /// if skill state is on
    /// </summary>
    public void SkillIsOn()
    {
        if (MainGameManager.Instance.SkillFlag)
        {
            CancelInvoke("SkillOnMpUse");

            MainGameManager.Instance.SkillFlag = false;

            MainGameManager.Instance.ChangeMaxTime();

            MainGameManager.Instance.TimeScaleDownFilterAni.SetBool("IsOn", false);

            if (MainGameManager.Instance.GetRaft(m_selectRaftX, m_selectRaftY).Code < 10000)
            {
                SetSelectRaftPos(m_playerXPos, m_playerYPos);
                GameManager.Instance.Alert("이동할 수 없습니다");
                return;
            }
            else
            {
                if(m_playerXPos == m_selectRaftX && m_playerYPos == m_selectRaftY)
                {
                    return;
                }

                if (!SetMp(-m_warpMpUse))
                {
                    SetSelectRaftPos(m_playerXPos, m_playerYPos);
                    GameManager.Instance.Alert("MP가 부족합니다!");
                }
                SetPlayerPosition(m_selectRaftX, m_selectRaftY);
            }
        }
        else
        {
            ResetInvoke();
            InvokeRepeating("SkillOnMpUse", m_skillOnMpUseDelay, m_skillOnMpUseDelay);

            MainGameManager.Instance.SkillFlag = true;
            
            Time.timeScale = m_skillOnSpeed;
            Time.fixedDeltaTime = m_skillOnSpeed / 10 * Time.timeScale;

            MainGameManager.Instance.TimeScaleDownFilterAni.gameObject.SetActive(true);
            MainGameManager.Instance.TimeScaleDownFilterAni.SetBool("IsOn", true);
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
        ResetInvoke();

        if (PlayerHp + argDamage <= 0)
        {
            MainGameManager.Instance.GameOver();
        }

        PlayerHp += argDamage;
    }

    /// <summary>
    /// use mp
    /// </summary>
    /// <param name="argMp">mp to use</param>
    /// <returns></returns>
    public bool SetMp(int argMp)
    {
        if(PlayerMp + argMp < 0)
        {
            return false;
        }

        PlayerMp += argMp;
        return true;
    }

    /// <summary>
    /// player move
    /// 0 = up
    /// 1 = down
    /// 2 = right
    /// 3 = left
    /// </summary>
    /// <param name="argMoveType">move type</param>
    public void PlayerMoveInput(int argMoveType)
    {
        ResetInvoke();

        if (argMoveType == 0)
        {
            SetPlayerPosition(m_playerXPos, m_playerYPos - 1);
        }
        else if (argMoveType == 1)
        {
            SetPlayerPosition(m_playerXPos, m_playerYPos + 1);
        }
        else if (argMoveType == 2)
        {
            SetPlayerPosition(m_playerXPos + 1, m_playerYPos);
        }
        else if (argMoveType == 3)
        {
            SetPlayerPosition(m_playerXPos - 1, m_playerYPos);
        }
    }

    /// <summary>
    /// set select raft position
    /// </summary>
    /// <param name="argRaftXIndex"></param>
    /// <param name="argRaftYIndex"></param>
    /// <returns>is player position is change or not</returns>
    void SetSelectRaftPos(int argRaftXIndex, int argRaftYIndex)
    {
        if (argRaftXIndex > MainGameManager.Instance.MaxRaftXSize - 1 ||
        argRaftYIndex > MainGameManager.Instance.MaxRaftYSize - 1 ||
        argRaftXIndex < 0 ||
        argRaftYIndex < 0)
        {
            return;
        }

        m_selectRaftX = argRaftXIndex;
        m_selectRaftY = argRaftYIndex;

        m_selectRaft.transform.position = MainGameManager.Instance.GetRaft(argRaftXIndex, argRaftYIndex).
            gameObject.transform.position;
    }

    /// <summary>
    /// Mp auto recovery
    /// call Invoke repeating
    /// </summary>
    void MpRecover()
    {
        SetMp(m_mpRecoveryValue);
    }

    /// <summary>
    /// player keyboard input value
    /// </summary>
    void PlayerkeyboardInput()
    {
        if (MainGameManager.Instance.SkillFlag)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetSelectRaftPos(m_selectRaftX, m_selectRaftY - 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetSelectRaftPos(m_selectRaftX, m_selectRaftY + 1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetSelectRaftPos(m_selectRaftX + 1, m_selectRaftY);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetSelectRaftPos(m_selectRaftX - 1, m_selectRaftY);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                SkillIsOn();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PlayerMoveInput(0);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PlayerMoveInput(1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PlayerMoveInput(2);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PlayerMoveInput(3);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                SkillIsOn();
            }

            if (MainGameManager.Instance.SkillFlag)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildRaft();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                MainGameManager.Instance.BuildAboveObjBtn(30001);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                RepairRaft();
            }
        }
    }

    /// <summary>
    /// build gage setting
    /// </summary>
    void BuildGageSetting()
    {
        MainGameManager.Instance.BuildSlider.maxValue = m_maxBuildGage;
        MainGameManager.Instance.BuildSlider.minValue = 0;
        MainGameManager.Instance.BuildSlider.value = 0;
    }

    /// <summary>
    /// build raft
    /// </summary>
    void BuildRaft()
    {
        if (!m_buildFlag)
        {
            ResetInvoke();
        }
        else
        {
            int _nextRaft = MainGameManager.Instance.GetRaft(m_selectRaftX, m_selectRaftY).Code + 1;
            if(_nextRaft <= 10000)
            {
                _nextRaft = 10001;
            }
            
            if (!MainGameManager.Instance.CheckIngredient(
                GameManager.Instance.GetRaftData(_nextRaft).m_needIngredientCode,
                GameManager.Instance.GetRaftData(_nextRaft).m_needIngredientAmount))
            {
                GameManager.Instance.Alert("재료가 부족합니다");
                return;
            }

            SoundManager.Instance.BuildSound.Play();

            m_buildFlag = false;
            m_raftBuildFlag = true;

            MainGameManager.Instance.BuildSlider.gameObject.SetActive(true);
            MainGameManager.Instance.BuildSlider.transform.position =
                new Vector2(m_selectRaft.transform.position.x, m_selectRaft.transform.position.y + 0.5f);

            InvokeRepeating("BuildGage", m_buildDelay, m_buildDelay);
        }
    }

    /// <summary>
    /// build gage
    /// </summary>
    void BuildGage()
    {
        MainGameManager.Instance.BuildSlider.value += 1;
        
        if(MainGameManager.Instance.BuildSlider.value == MainGameManager.Instance.BuildSlider.maxValue)
        {
            if (m_raftBuildFlag)
            {
                MainGameManager.Instance.BuildSlider.value = 0;
                MainGameManager.Instance.BuildRaft(m_selectRaftX, m_selectRaftY);
            }
            else
            {
                if (!MainGameManager.Instance.UseIngredient(
                    GameManager.Instance.GetAboveObjData(m_nowAboveObjCode).m_needIngredientCode,
                    GameManager.Instance.GetAboveObjData(m_nowAboveObjCode).m_needIngredientAmount))
                {
                    ResetInvoke();
                    return;
                }

                MainGameManager.Instance.BuildSlider.value = 0;
                MainGameManager.Instance.SetAboveObjectState(m_nowAboveObjCode, m_selectRaftX, m_selectRaftY);
            }

            ResetInvoke();
        }
    }

    /// <summary>
    /// repair raft
    /// </summary>
    void RepairRaft()
    {
        if (!m_repairFlag || !m_buildFlag)
        {
            return;
        }

        if (MainGameManager.Instance.GetRaft(m_playerXPos, m_playerYPos).RaftHp ==
            MainGameManager.Instance.GetRaft(m_playerXPos, m_playerYPos).MaxRaftHp ||
            !SetMp(-1))
        {
            return;
        }

        Invoke("RepairFlagTrue", m_repairDelay);
        MainGameManager.Instance.GetRaft(m_playerXPos, m_playerYPos).RaftHp += m_repairStrength;
        m_repairFlag = false;
    }

    /// <summary>
    /// repair flag to true
    /// call invoke function
    /// </summary>
    void RepairFlagTrue()
    {
        m_repairFlag = true;
    }

    /// <summary>
    /// skill on mp use
    /// call invoke repeate
    /// </summary>
    void SkillOnMpUse()
    {
        SetMp(-1);
    }

    void ResetInvoke()
    {
        m_repairFlag = true;
        if (!m_buildFlag)
        {
            SoundManager.Instance.BuildSound.Stop();

            m_buildFlag = true;

            MainGameManager.Instance.BuildSlider.gameObject.SetActive(false);
            SetSelectRaftPos(m_playerXPos, m_playerYPos);
            BuildGageSetting();

            CancelInvoke("BuildGage");
        }
        if (MainGameManager.Instance.SkillFlag)
        {
            SkillIsOn();
        }
    }

    public int NowAboveObjCode
    {
        get { return m_nowAboveObjCode; }
        set { m_nowAboveObjCode = value; }
    }

    public int PlayerXPos
    {
        get { return m_playerXPos; }
    }

    public int PlayerYPos
    {
        get { return m_playerYPos; }
    }

    public int PlayerMaxHp
    {
        get { return m_playerMaxHp; }
        set { m_playerMaxHp = value; }
    }

    public int PlayerMaxMp
    {
        get { return m_playerMaxMp; }
        set { m_playerMaxMp = value; }
    }

    public int PlayerHp
    {
        get { return m_playerHp; }
        set
        {
            if(value >= m_playerMaxHp)
            {
                m_playerHp = m_playerMaxHp;
            }
            else if(value <= 0)
            {
                m_playerHp = 0;
                MainGameManager.Instance.GameOver();
            }
            else
            {
                m_playerHp = value;
            }

            MainGameManager.Instance.SetPlayerHpSlider();
        }
    }

    public int PlayerMp
    {
        get { return m_playerMp; }
        set
        {
            if (value >= m_playerMaxMp)
            {
                m_playerMp = m_playerMaxMp;
            }
            else if (value <= 0)
            {
                m_playerMp = 0;
            }
            else
            {
                m_playerMp = value;
            }

            MainGameManager.Instance.SetPlayerMpSlider();
        }
    }
}

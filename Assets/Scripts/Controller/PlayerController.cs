using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// build delay
    /// </summary>
    [SerializeField]
    float m_buildDelay = 0.0f;

    [Header("Mp")]
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
    bool m_buildFlag = true;

    /// <summary>
    /// repair flag
    /// </summary>
    bool m_repairFlag = true;

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
        MainGameManager.Instance.GetIngredient(argCode, argAmount);
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

            SetSelectRaftPos(argRaftXIndex, argRaftYIndex);
        }
        else
        {
            if(argRaftXIndex == m_selectRaftX && argRaftYIndex == m_selectRaftY)
            {
                MainGameManager.Instance.BuildRaft(m_selectRaftX, m_selectRaftY);

                SetSelectRaftPos(m_playerXPos, m_playerYPos);
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
            CancelInvoke();

            MainGameManager.Instance.SkillFlag = false;

            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;

            if(MainGameManager.Instance.GetRaft(m_selectRaftX, m_selectRaftY).Code < 10000)
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
            InvokeRepeating("SkillOnMpUse", m_skillOnMpUseDelay, m_skillOnMpUseDelay);

            MainGameManager.Instance.SkillFlag = true;
            
            Time.timeScale = m_skillOnSpeed;
            Time.fixedDeltaTime = m_skillOnSpeed / 10 * Time.timeScale;
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
        if(PlayerHp + argDamage <= 0)
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildRaft();
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                SkillIsOn();
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                RepairRaft();
            }
        }
    }

    void BuildGage()
    {

    }

    /// <summary>
    /// build raft
    /// </summary>
    void BuildRaft()
    {
        if (m_buildFlag)
        {
            return;
        }

        Invoke("BuildFlagTrue", m_buildDelay);
        MainGameManager.Instance.BuildRaft(m_selectRaftX, m_selectRaftY);
        m_buildFlag = false;
    }

    /// <summary>
    /// repair raft
    /// </summary>
    void RepairRaft()
    {
        if (!m_repairFlag)
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

    void BuildFlagTrue()
    {
        m_buildFlag = true;
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
            if (value >= m_maxPlayerMp)
            {
                m_playerMp = m_maxPlayerMp;
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

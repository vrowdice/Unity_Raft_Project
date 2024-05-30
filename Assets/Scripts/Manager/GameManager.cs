using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// game manager
    /// </summary>
    static GameManager g_gameManager;

    /// <summary>
    /// canvas
    /// </summary>
    Canvas m_canvas = null;

    /// <summary>
    /// alert object
    /// </summary>
    [SerializeField]
    GameObject m_alertObject = null;

    /// <summary>
    /// raft data to add
    /// </summary>
    [SerializeField]
    List<RaftData> m_raftDataList = new List<RaftData>();

    /// <summary>
    /// object data list to add
    /// </summary>
    [SerializeField]
    List<ObjectData> m_objDataList = new List<ObjectData>();

    /// <summary>
    /// obstacle data to add
    /// </summary>
    [SerializeField]
    List<ObstacleData> m_obstacleDataList = new List<ObstacleData>();

    /// <summary>
    /// ingredient data list to add
    /// </summary>
    [SerializeField]
    List<IngredientData> m_ingredientDataList = new List<IngredientData>();

    /// <summary>
    /// raft data dictionary
    /// </summary>
    Dictionary<int, RaftData> m_raftDataDic = new Dictionary<int, RaftData>();

    /// <summary>
    /// raft data dictionary
    /// </summary>
    Dictionary<int, ObjectData> m_objDataDic = new Dictionary<int, ObjectData>();

    /// <summary>
    /// raft data dictionary
    /// </summary>
    Dictionary<int, ObstacleData> m_obstacleDic = new Dictionary<int, ObstacleData>();

    /// <summary>
    /// raft data dictionary
    /// </summary>
    Dictionary<int, IngredientData> m_ingredientDic = new Dictionary<int, IngredientData>();

    private void Awake()
    {
        g_gameManager = this;
        DontDestroyOnLoad(this.gameObject);

        SetDicData();
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        catch
        {
            Debug.LogError("Canvas is null");
        }
    }

    /// <summary>
    /// return raft data as code
    /// </summary>
    /// <param name="argRaftCode">raft code</param>
    /// <returns>raftdata</returns>
    public RaftData GetRaftData(int argRaftCode)
    {
        try
        {
            return m_raftDataDic[argRaftCode];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// return object data as code
    /// </summary>
    /// <param name="argObjCode">object code</param>
    /// <returns>objcet data</returns>
    public ObjectData GetObjData(int argObjCode)
    {
        try
        {
            return m_objDataDic[argObjCode];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// return object data as code
    /// </summary>
    /// <param name="argObjCode">object code</param>
    /// <returns>objcet data</returns>
    public ObstacleData GetObstacleData(int argObstacleCode)
    {
        try
        {
            return m_obstacleDic[argObstacleCode];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// return object data as code
    /// </summary>
    /// <param name="argObjCode">object code</param>
    /// <returns>objcet data</returns>
    public IngredientData GetIngredientData(int argIngredientCode)
    {
        try
        {
            return m_ingredientDic[argIngredientCode];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// play alert animation
    /// </summary>
    public void Alert(string argAlertStr)
    {
        Instantiate(m_alertObject, m_canvas.transform).GetComponent<AlertPanel>().Alert(argAlertStr);
    }

    /// <summary>
    /// setting dictionary data
    /// </summary>
    void SetDicData()
    {
        for (int i = 0; i < m_raftDataList.Count; i++)
        {
            m_raftDataDic.Add(m_raftDataList[i].m_code, m_raftDataList[i]);
        }
        for (int i = 0; i < m_objDataList.Count; i++)
        {
            m_objDataDic.Add(m_objDataList[i].m_code, m_objDataList[i]);
        }
        for (int i = 0; i < m_obstacleDataList.Count; i++)
        {
            m_obstacleDic.Add(m_obstacleDataList[i].m_code, m_obstacleDataList[i]);
        }
        for (int i = 0; i < m_ingredientDataList.Count; i++)
        {
            m_ingredientDic.Add(m_ingredientDataList[i].m_code, m_ingredientDataList[i]);
        }
    }

    public Dictionary<int, ObstacleData> ObstacleDic
    {
        get
        {
            return m_obstacleDic;
        }
    }

    public Dictionary<int, IngredientData> IngredientDic
    {
        get
        {
            return m_ingredientDic;
        }
    }

    /// <summary>
    /// instance
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            return g_gameManager;
        }
    }
}

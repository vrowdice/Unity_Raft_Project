using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class will put in
/// float weight dictionary
/// </summary>
public class FloatWeight
{
    /// <summary>
    /// first setting weight
    /// </summary>
    public int m_mainWeight = 0;

    /// <summary>
    /// changed weight
    /// </summary>
    public int m_changeWeight = 0;

    public int ChangeWeight
    {
        get
        {
            return m_changeWeight;
        }
        set
        {
            if(m_changeWeight + value <= 0)
            {
                m_changeWeight = 0;
                return;
            }

            m_changeWeight = value;
        }
    }
}

public class FloatGenerator : MonoBehaviour
{
    [Header("Common Balance")]
    /// <summary>
    /// balace change (weight, gen speed)
    /// at this time
    /// </summary>
    [SerializeField]
    float m_balanceChangeTime = 0.0f;

    /// <summary>
    /// first setting float speed
    /// </summary>
    [SerializeField]
    float m_firstFloatSpeed = 0.0f;

    /// <summary>
    /// all floatSpeed
    /// </summary>
    float m_floatSpeed = 0.0f;

    [Header("Obstacle")]
    /// <summary>
    /// main obstacle object
    /// </summary>
    [SerializeField]
    GameObject m_obstacle = null;

    /// <summary>
    /// obstacle min generate speed
    /// </summary>
    [SerializeField]
    float m_obstacleMinGenSpeed = 0.0f;

    /// <summary>
    /// generate speed
    /// </summary>
    [SerializeField]
    float m_obstacleGenSpeed = 0.0f;

    /// <summary>
    /// obstacle generate speed
    /// change per second
    /// </summary>
    [SerializeField]
    float m_obstacleGenSpeedChange = 0.0f;

    /// <summary>
    /// obstacle all weight value
    /// </summary>
    int m_obstacleAllWeight = 0;

    /// <summary>
    /// obstalce parent empty obejct
    /// </summary>
    GameObject m_obstacleParentObj = null;

    /// <summary>
    /// obstacle weight change values
    /// </summary>
    Dictionary<int, FloatWeight> m_obstacleWeightDic = new Dictionary<int, FloatWeight>();

    [Header("Ingredient")]
    /// <summary>
    /// main ingredient object
    /// </summary>
    [SerializeField]
    GameObject m_ingredient = null;

    /// <summary>
    /// ingredient max generate speed
    /// </summary>
    [SerializeField]
    float m_ingredientMaxGenSpeed = 0.0f;

    /// <summary>
    /// generate speed
    /// </summary>
    [SerializeField]
    float m_ingredientGenSpeed = 0.0f;

    /// <summary>
    /// ingredient generate speed
    /// change per second
    /// </summary>
    [SerializeField]
    float m_ingredientGenSpeedChange = 0.0f;

    /// <summary>
    /// ingredient all weight value
    /// </summary>
    int m_ingredientAllWeight = 0;

    /// <summary>
    /// ingredient parent empty obejct
    /// </summary>
    GameObject m_ingredientParentObj = null;

    /// <summary>
    /// obstacle weight change values
    /// </summary>
    Dictionary<int, FloatWeight> m_ingredientWeightDic = new Dictionary<int, FloatWeight>();

    [Header("Money")]
    /// <summary>
    /// money object
    /// </summary>
    [SerializeField]
    GameObject m_moneyObject = null;

    /// <summary>
    /// generate speed
    /// </summary>
    [SerializeField]
    float m_moneyGenSpeed = 0.0f;

    /// <summary>
    /// ingredient parent empty obejct
    /// </summary>
    GameObject m_moneyParentObj = null;

    private void Awake()
    {
        m_obstacleParentObj = new GameObject("ObstacleParent");
        m_ingredientParentObj = new GameObject("IngredientParent");
        m_moneyParentObj = new GameObject("MoneyParent");
    }

    // Start is called before the first frame update
    void Start()
    {
        FloatSpeed = m_firstFloatSpeed;

        StartCoroutine(SetIngredient());
        StartCoroutine(SetObstacle());
        StartCoroutine(SetMoney());

        WeightDicSetting();
        InvokeRepeating("ChangeWeight", m_balanceChangeTime, m_balanceChangeTime);
        InvokeRepeating("ChangeGenSpeed", m_balanceChangeTime, m_balanceChangeTime);
    }

    IEnumerator SetObstacle()
    {
        while (MainGameManager.Instance.IsGame)
        {
            yield return new WaitForSeconds(m_obstacleGenSpeed);
            GenerateObstacle();
        }
    }

    IEnumerator SetIngredient()
    {
        while (MainGameManager.Instance.IsGame)
        {
            yield return new WaitForSeconds(m_ingredientGenSpeed);
            GenerateIngredient();
        }
    }

    IEnumerator SetMoney()
    {
        while (MainGameManager.Instance.IsGame)
        {
            yield return new WaitForSeconds(m_moneyGenSpeed);
            GenerateMoney();
        }
    }

    /// <summary>
    /// generate obstacle object and setting
    /// </summary>
    void GenerateObstacle()
    {
        int _randInt = Random.Range(-2, 3);
        float _randFloat = _randInt * 1.5f;
        GameObject _obj =  Instantiate(m_obstacle);
        Obstacle _obstacle = _obj.GetComponent<Obstacle>();

        int _randCode = GetRandomObstacleCode();

        _obj.transform.position = new Vector2(transform.position.x, _randFloat);
        _obstacle.SetObstacle(m_floatSpeed, 0.0f, _randCode);

        _obj.transform.SetParent(m_obstacleParentObj.transform);

        //not required once the raft image is defined
        _obstacle.ViewSprite.color = MainGameManager.Instance.GetColor(_randCode);
    }

    /// <summary>
    /// generate ingredient object and setting
    /// </summary>
    void GenerateIngredient()
    {
        int _randInt = Random.Range(-3, 4);
        GameObject _obj = Instantiate(m_ingredient);
        Ingredient _ingredient = _obj.GetComponent<Ingredient>();

        int _randCode = GetRandomIngredientCode();

        _obj.transform.position = new Vector2(transform.position.x, _randInt);
        if (_randInt <= 0)
        {
            _ingredient.SetIngredient(m_floatSpeed, -0.1f, _randCode);
        }
        else
        {
            _ingredient.SetIngredient(m_floatSpeed, 0.1f, _randCode);
        }

        _obj.transform.SetParent(m_ingredientParentObj.transform);

        //not required once the raft image is defined
        _ingredient.ViewSprite.color = MainGameManager.Instance.GetColor(_randCode);
    }

    /// <summary>
    /// generate money
    /// </summary>
    void GenerateMoney()
    {
        int _randInt = Random.Range(-3, 4);
        GameObject _obj = Instantiate(m_moneyObject);
        Money _money = _obj.GetComponent<Money>();

        _obj.transform.position = new Vector2(transform.position.x, _randInt);
        _obj.transform.SetParent(m_moneyParentObj.transform);
        _money.SetMoney(m_floatSpeed, 0.0f, 100);
    }

    /// <summary>
    /// returns a random obstacle code based on the weight
    /// </summary>
    /// <returns>code</returns>
    public int GetRandomObstacleCode()
    {
        int _rand = Random.Range(0, m_obstacleAllWeight);
        int _sum = 0;
        foreach (KeyValuePair<int, FloatWeight> val in m_obstacleWeightDic)
        {
            _sum += val.Value.m_changeWeight;
            if(_sum >= _rand)
            {
                return val.Key;
            }
        }
        return 0;
    }

    /// <summary>
    /// returns a random obstacle code based on the weight
    /// </summary>
    /// <returns>code</returns>
    public int GetRandomIngredientCode()
    {
        int _rand = Random.Range(0, m_ingredientAllWeight);
        int _sum = 0;
        foreach (KeyValuePair<int, FloatWeight> val in m_ingredientWeightDic)
        {
            _sum += val.Value.m_changeWeight;

            if (_sum >= _rand)
            {
                return val.Key;
            }
        }
        return 0;
    }

    /// <summary>
    /// change speed all object generated float and generate float
    /// </summary>
    void ChangeAllFloatSpeed()
    {
        for (int i = 0; i < m_obstacleParentObj.transform.childCount; i++)
        {
            m_obstacleParentObj.transform.GetChild(i).GetComponent<Obstacle>().SetSpeed(m_floatSpeed);
        }
        for (int i = 0; i < m_ingredientParentObj.transform.childCount; i++)
        {
            m_ingredientParentObj.transform.GetChild(i).GetComponent<Ingredient>().SetSpeed(m_floatSpeed);
        }
    }

    /// <summary>
    /// weight dictionary setting
    /// </summary>
    void WeightDicSetting()
    {
        foreach (KeyValuePair<int, ObstacleData> val in GameManager.Instance.ObstacleDic)
        {
            FloatWeight _float = new FloatWeight();

            _float.m_mainWeight = val.Value.m_generateWeight;
            _float.m_changeWeight = val.Value.m_generateWeight;

            m_obstacleWeightDic.Add(val.Key, _float);

            m_obstacleAllWeight += val.Value.m_generateWeight;
        }

        foreach (KeyValuePair<int, IngredientData> val in GameManager.Instance.IngredientDic)
        {
            FloatWeight _float = new FloatWeight();

            _float.m_mainWeight = val.Value.m_generateWeight;
            _float.m_changeWeight = val.Value.m_generateWeight;

            m_ingredientWeightDic.Add(val.Key, _float);

            m_ingredientAllWeight += val.Value.m_generateWeight;
        }
    }

    /// <summary>
    /// change weight
    /// with gamemanager change weight
    /// </summary>
    void ChangeWeight()
    {
        m_obstacleAllWeight = 0;
        foreach (KeyValuePair<int, FloatWeight> val in m_obstacleWeightDic)
        {
            val.Value.ChangeWeight += GameManager.Instance.GetObstacleData(val.Key).m_weightChange;
            m_obstacleAllWeight += val.Value.ChangeWeight;
        }

        m_ingredientAllWeight = 0;
        foreach (KeyValuePair<int, FloatWeight> val in m_ingredientWeightDic)
        {
            val.Value.ChangeWeight += GameManager.Instance.GetIngredientData(val.Key).m_weightChange;
            m_ingredientAllWeight += val.Value.ChangeWeight;
        }
    }

    /// <summary>
    /// change generate speed
    /// </summary>
    void ChangeGenSpeed()
    {
        if (m_obstacleMinGenSpeed >= m_obstacleGenSpeed)
        {
            m_obstacleGenSpeed = m_obstacleMinGenSpeed;
        }
        else
        {
            m_obstacleGenSpeed += m_obstacleGenSpeedChange;
        }

        if (m_ingredientMaxGenSpeed <= m_ingredientGenSpeed)
        {
            m_ingredientGenSpeed = m_ingredientMaxGenSpeed;
        }
        else
        {
            m_ingredientGenSpeed += m_ingredientGenSpeedChange;
        }
    }

    public float FirstFloatSpeed
    {
        get { return m_firstFloatSpeed; }
    }
    public float FloatSpeed
    {
        get { return m_floatSpeed; }
        set 
        { 
            m_floatSpeed = value;

            ChangeAllFloatSpeed();
        }
    }
}

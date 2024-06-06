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
    [Header("Obstacle")]
    /// <summary>
    /// main obstacle object
    /// </summary>
    [SerializeField]
    GameObject m_obstacle = null;

    /// <summary>
    /// obstacle speed
    /// </summary>
    [SerializeField]
    float m_obstacleSpeed = 0.0f;

    /// <summary>
    /// generate speed
    /// </summary>
    [SerializeField]
    float m_obstacleGenSpeed = 0.0f;

    /// <summary>
    /// obstacle all weight value
    /// </summary>
    int m_obstacleAllWeight = 0;

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
    /// obstacle speed
    /// </summary>
    [SerializeField]
    float m_ingredientSpeed = 0.0f;

    /// <summary>
    /// generate speed
    /// </summary>
    [SerializeField]
    float m_ingredientGenSpeed = 0.0f;

    /// <summary>
    /// ingredient all weight value
    /// </summary>
    int m_ingredientAllWeight = 0;

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
    /// obstacle speed
    /// </summary>
    [SerializeField]
    float m_moneySpeed = 0.0f;

    /// <summary>
    /// generate speed
    /// </summary>
    [SerializeField]
    float m_moneyGenSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetIngredient());
        StartCoroutine(SetObstacle());

        WeightDicSetting();
        InvokeRepeating("ChangeWeight", 5.0f, 5.0f);
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

    /// <summary>
    /// generate obstacle object and setting
    /// </summary>
    void GenerateObstacle()
    {
        int _randInt = Random.Range(-3, 4);
        float _randFloat = _randInt * 1.5f;

        GameObject _obj =  Instantiate(m_obstacle);
        Obstacle _obstacle = _obj.GetComponent<Obstacle>();

        _obj.transform.position = new Vector2(transform.position.x, _randFloat);
        _obstacle.SetObstacle(m_obstacleSpeed, 0.0f, GetRandomObstacleCode());
    }

    /// <summary>
    /// generate ingredient object and setting
    /// </summary>
    void GenerateIngredient()
    {
        int _randInt = Random.Range(-3, 4);
        GameObject _obj = Instantiate(m_ingredient);
        Ingredient _ingredient = _obj.GetComponent<Ingredient>();

        _obj.transform.position = new Vector2(transform.position.x, _randInt);
        if (_randInt <= 0)
        {
            _ingredient.SetIngredient(m_ingredientSpeed, -0.1f, GetRandomIngredientCode());
        }
        else
        {
            _ingredient.SetIngredient(m_ingredientSpeed, 0.1f, GetRandomIngredientCode());
        }
    }

    /// <summary>
    /// generate money
    /// </summary>
    void GenerateMoney()
    {
        int _randInt = Random.Range(-3, 4);
        float _randFloat = _randInt * 1.5f;

        GameObject _obj = Instantiate(m_moneyObject);
        //Obstacle _obstacle = _obj.GetComponent<Obstacle>();

        _obj.transform.position = new Vector2(transform.position.x, _randFloat);
        //_obstacle.SetObstacle(m_obstacleSpeed, 0.0f, GetRandomObstacleCode());
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
}

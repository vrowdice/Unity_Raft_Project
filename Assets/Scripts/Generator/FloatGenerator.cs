using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatGenerator : MonoBehaviour
{
    /// <summary>
    /// main game manager instance
    /// </summary>
    MainGameManager m_mainGameManager = null;

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
    List<int> m_obstacleWeightChange = new List<int>();

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
    List<int> m_ingredientWeightChange = new List<int>();

    private void Awake()
    {
        m_mainGameManager = GameObject.Find("MainGameManager").GetComponent<MainGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetIngredient());
        StartCoroutine(SetObstacle());

        foreach (KeyValuePair<int, ObstacleData> val in GameManager.Instance.ObstacleDic)
        {
            m_obstacleAllWeight += val.Value.m_generateWeight;
        }
        foreach (KeyValuePair<int, IngredientData> val in GameManager.Instance.IngredientDic)
        {
            m_ingredientAllWeight += val.Value.m_generateWeight;
        }
    }

    IEnumerator SetObstacle()
    {
        while (m_mainGameManager.IsGame)
        {
            yield return new WaitForSeconds(m_obstacleGenSpeed);
            GenerateObstacle();
        }
    }

    IEnumerator SetIngredient()
    {
        while (m_mainGameManager.IsGame)
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
        _obstacle.SetObstacle(m_mainGameManager ,m_obstacleSpeed, 0.0f, GetRandomObstacleCode());
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
    /// returns a random obstacle code based on the weight
    /// </summary>
    /// <returns>code</returns>
    public int GetRandomObstacleCode()
    {
        Dictionary<int, ObstacleData> _dic = GameManager.Instance.ObstacleDic;
        int _rand = Random.Range(0, m_obstacleAllWeight);
        int _sum = 0;

        foreach(KeyValuePair<int, ObstacleData> val in _dic)
        {
            _sum += val.Value.m_generateWeight;
            if(_sum >= _rand)
            {
                return val.Value.m_code;
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
        Dictionary<int, IngredientData> _dic = GameManager.Instance.IngredientDic;
        int _rand = Random.Range(0, m_ingredientAllWeight);
        int _sum = 0;

        foreach (KeyValuePair<int, IngredientData> val in _dic)
        {
            _sum += val.Value.m_generateWeight;
            if (_sum >= _rand)
            {
                return val.Value.m_code;
            }
        }
        return 0;
    }
}

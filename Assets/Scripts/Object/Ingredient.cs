using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    /// <summary>
    /// ths sprite to view
    /// </summary>
    [SerializeField]
    SpriteRenderer m_viewSprite = null;

    /// <summary>
    /// rigidbody2d
    /// </summary>
    Rigidbody2D m_rigidbody2D = null;

    /// <summary>
    /// object code
    /// </summary>
    int m_code = 0;

    /// <summary>
    /// obstacle x move speed
    /// </summary>
    float m_xSpeed = 0.0f;

    /// <summary>
    /// obstacle y move speed
    /// </summary>
    float m_ySpeed = 0.0f;

    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleControll();
    }

    /// <summary>
    /// move obstacle
    /// </summary>
    void ObstacleControll()
    {
        m_rigidbody2D.velocity = new Vector2(-m_xSpeed, m_ySpeed);
        if (transform.position.x <= -12.0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// set speed
    /// </summary>
    /// <param name="argXSpeed">x speed</param>
    /// <param name="argYSpeed">y speed</param>
    public void SetIngredient(float argXSpeed, float argYSpeed, int argCode)
    {
        if (argCode <= 0)
        {
            Debug.LogError("code is not allowed");
            Destroy(gameObject);
            return;
        }
        m_xSpeed = argXSpeed;
        m_ySpeed = argYSpeed;
        m_code = argCode;

        m_viewSprite.sprite = GameManager.Instance.GetIngredientData(m_code).m_sprite;
    }
}

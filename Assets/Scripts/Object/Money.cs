using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    /// <summary>
    /// rigidbody2d
    /// </summary>
    Rigidbody2D m_rigidbody2D = null;

    /// <summary>
    /// money value
    /// </summary>
    int m_moneyValue = 0;

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
        MoneyControll();
        MoneyDestroy();
    }

    /// <summary>
    /// move obstacle
    /// </summary>
    void MoneyControll()
    {
        if (MainGameManager.Instance.SkillFlag)
        {
            m_rigidbody2D.velocity = new Vector2(-m_xSpeed, m_ySpeed);
        }
        else
        {
            m_rigidbody2D.velocity = new Vector2(-m_xSpeed, m_ySpeed);
        }
    }

    /// <summary>
    /// destroy obstacles to the situation
    /// </summary>
    void MoneyDestroy()
    {
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
    public void SetMoney(float argXSpeed, float argYSpeed, int argMoneyValue)
    {
        m_moneyValue = argMoneyValue;
        m_xSpeed = argXSpeed;
        m_ySpeed = argYSpeed;
    }

    public int MoneyValue
    {
        get { return m_moneyValue; }
    }
}

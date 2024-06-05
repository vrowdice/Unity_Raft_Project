using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    /// <summary>
    /// ths sprite to view
    /// </summary>
    [SerializeField]
    SpriteRenderer m_viewSprite = null;

    /// <summary>
    /// main manager instance
    /// </summary>
    MainGameManager m_mainManager = null;

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
        transform.Translate(new Vector2(-m_xSpeed, m_ySpeed) * Time.deltaTime);
        if(transform.position.x <= -12.0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// set speed
    /// </summary>
    /// <param name="argXSpeed">x speed</param>
    /// <param name="argYSpeed">y speed</param>
    public void SetObstacle(MainGameManager argMainManager ,float argXSpeed,
         float argYSpeed, int argCode)
    {
        if(argCode <= 0)
        {
            Debug.LogError("code is not allowed");
            Destroy(gameObject);
            return;
        }

        m_mainManager = argMainManager;
        m_xSpeed = argXSpeed;
        m_ySpeed = argYSpeed;
        m_code = argCode;

        m_viewSprite.sprite = GameManager.Instance.GetObstacleData(m_code).m_sprite;
        m_viewSprite.color = Color.black;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Raft"))
        {
            List<Vector2> _destroyRaft = GameManager.Instance.GetObstacleData(m_code).m_destroyRaftPos;
            Raft _raft = collision.gameObject.GetComponent<Raft>();
            
            for(int i = 0; i < _destroyRaft.Count; i++)
            {
                MainGameManager.Instance.DamageRaft(
                    GameManager.Instance.ObstacleDic[m_code].m_damage,
                    _raft.RaftXIndexData + (int)_destroyRaft[i].x,
                    _raft.RaftYIndexData + (int)_destroyRaft[i].y);
            }

            Destroy(gameObject);
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientChecker : MonoBehaviour
{
    /// <summary>
    /// player controller
    /// </summary>
    [SerializeField]
    PlayerController m_playerController = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ingredient")
        {
            m_playerController.GetIngredient(
                collision.gameObject.GetComponent<Ingredient>().Code, 1);
            Destroy(collision.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveBtn : MonoBehaviour
{
    /// <summary>
    /// click
    /// </summary>
    public void Click(GameObject argGameObject)
    {
        if(argGameObject == null)
        {
            return;
        }
        
        if(argGameObject.activeSelf == false)
        {
            argGameObject.SetActive(true);
        }
        else
        {
            argGameObject.SetActive(false);
        }
    }
}

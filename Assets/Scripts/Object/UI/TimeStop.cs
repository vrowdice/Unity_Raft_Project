using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    public void ControllTimeStop()
    {
        if (MainGameManager.Instance.IsGame)
        {
            MainGameManager.Instance.IsGame = false;

            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = 0.02f;
        }
        else
        {
            MainGameManager.Instance.ChangeMaxTime();
        }
    }
}

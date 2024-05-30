using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// play alert animation
    /// </summary>
    /// <param name="argAlertStr">alert text</param>
    public void Alert(string argAlertStr)
    {
        GetComponentInChildren<Text>().text =
            argAlertStr;
    }

    /// <summary>
    /// destroy this object over animation
    /// </summary>
    public void AniOver()
    {
        Destroy(gameObject);
    }
}

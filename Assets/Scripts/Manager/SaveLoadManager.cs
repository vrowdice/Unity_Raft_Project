using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// data load
    /// </summary>
    /// <param name="argPath">data path</param>
    /// <returns>data string</returns>
    string Load(string argPath)
    {
        string _path = Application.persistentDataPath + "/" + argPath + ".json";
        string _data = string.Empty;
        StreamReader _sr = new StreamReader(_path, System.Text.Encoding.UTF8);
        _data = _sr.ReadToEnd();
        _sr.Close();

        return _data;
    }

    /// <summary>
    /// data save
    /// </summary>
    /// <param name="argPath">data path</param>
    /// <param name="argData">data string</param>
    void Save(string argPath, string argData)
    {
        string _path = Application.persistentDataPath + "/" + argPath + ".json";
        StreamWriter _sw = new StreamWriter(_path, false, System.Text.Encoding.UTF8);
        _sw.WriteLine(argData);
        _sw.Close();
    }
}

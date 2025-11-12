using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// α版用　仮の処理
/// </summary>
public class Alpha : Singleton<Alpha>
{
    bool start;

    void Awake()
    {
        //シーンを遷移しても残る
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        if (!start && FindObjectOfType(System.Type.GetType("MapManager")) == null)
        {
            start = true;
            SceneManager.LoadScene(1);
        }
    }
}

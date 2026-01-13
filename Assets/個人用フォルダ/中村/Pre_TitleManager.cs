using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pre_TitleManager : MonoBehaviour
{
    public void LoadS(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Exit()
    {
        //UnityEditor上でプレイを終了する場合
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //ビルドした実行データでプレイを終了する場合
        Application.Quit();
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : Singleton<TitleManager>
{
    //各Canvas
    [SerializeField] GameObject canvasParent;
    GameObject[] canvas;

    //操作説明関連の変数
    [SerializeField] GameObject manual_NextButton;
    [SerializeField] GameObject manual_PreButton;
    [SerializeField] GameObject manual_PageParent;
    GameObject[] manual_Pages;
    int manual_PageIndex;

    void Start()
    {
        SoundManager.Instance.PlayBGM(0);

        //Canvasの表示
        canvas = new GameObject[canvasParent.transform.childCount];
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i] = canvasParent.transform.GetChild(i).gameObject;
            canvas[i].SetActive(i <= 0);
        }
        canvasParent.SetActive(true);

        //操作説明関連のコンポーネントを取得
        manual_Pages = new GameObject[manual_PageParent.transform.childCount];
        for (int i = 0; i < manual_Pages.Length; i++)
        {
            manual_Pages[i] = manual_PageParent.transform.GetChild(i).gameObject;
        }
    }

    //ステージ(難易度)を選ぶ画面を表示する
    public void ChoiceStage(int num = -1)
    {
        if (num <= 0)
        {
            //選択画面を開く
            if (!canvas[1].activeSelf)
            {
                SoundManager.Instance.PlaySE_Sys(0);

                canvas[1].SetActive(true);
            }
            //選択画面を閉じる
            else
            {
                SoundManager.Instance.PlaySE_Sys(2);

                canvas[1].SetActive(false);
            }
        }
        else
        {
            SoundManager.Instance.PlaySE_Sys(1);

            //フロア数を保持してマップシーンへ遷移
            ParameterManager.Instance.floorNum = num;
            LoadS(1);
        }
    }

    //シーン読み込み
    public void LoadS(int i)
    {
        FadeManager.Instance.LoadSceneIndex(i, 0.5f);
    }

    //操作説明を開く
    public void ViewManual()
    {
        //操作説明を開く
        if (!canvas[2].activeSelf)
        {
            SoundManager.Instance.PlaySE_Sys(0);

            manual_Pages[manual_PageIndex].SetActive(false);
            manual_Pages[0].SetActive(true);
            manual_NextButton.SetActive(true);
            manual_PreButton.SetActive(false);
            manual_PageIndex = 0;

            canvas[2].SetActive(true);
        }
        //操作説明を閉じる
        else
        {
            SoundManager.Instance.PlaySE_Sys(2);

            canvas[2].SetActive(false);
        }
    }

    //操作説明のページを変える
    public void ManualPageChange(bool next = true)
    {
        SoundManager.Instance.PlaySE_Sys(0);

        //次のページへ
        if (next)
        {
            if (manual_PageIndex >= manual_Pages.Length - 1) return;

            manual_Pages[manual_PageIndex].SetActive(false);
            manual_PageIndex++;
            manual_Pages[manual_PageIndex].SetActive(true);

            if (manual_PageIndex >= manual_Pages.Length - 1) manual_NextButton.SetActive(false);
            manual_PreButton.SetActive(true);
        }
        //前のページへ
        else
        {
            if (manual_PageIndex <= 0) return;

            manual_Pages[manual_PageIndex].SetActive(false);
            manual_PageIndex--;
            manual_Pages[manual_PageIndex].SetActive(true);

            if (manual_PageIndex <= 0) manual_PreButton.SetActive(false);
            manual_NextButton.SetActive(true);
        }
    }

    //デバッグモード開始
    public void Debug()
    {
        //デバッグ画面を開く
        if (!canvas[3].activeSelf)
        {
            SoundManager.Instance.PlaySE_Sys(0);

            canvas[3].SetActive(true);
            DebugMode.Instance.DebugStart();
        }
        //デバッグ画面を閉じる
        else
        {
            SoundManager.Instance.PlaySE_Sys(2);

            canvas[3].SetActive(false);
        }
    }

    public void WorldLevelEasy()
    {
        MapManager.Instance.worldLevel = 0;
    }
    public void WorldLevelNormal()
    {
        MapManager.Instance.worldLevel = 1;
    }
    public void WorldLevelExtra()
    {
        MapManager.Instance.worldLevel = 2;
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

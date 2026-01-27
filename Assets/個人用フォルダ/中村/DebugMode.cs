using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugMode : Singleton<DebugMode>
{
    [SerializeField] GameObject[] window;
    [SerializeField] TextMeshProUGUI[] text_Choice;
    [SerializeField] TextMeshProUGUI[] text_Level;

    int[] unitId = new int[5];
    int[] unitLevel = new int[9];
    int unitIndex;

    //デバッグモード開始
    public void DebugStart()
    {
        unitIndex = 0;

        for (int i = 0; i <window.Length; i++)
        {
            window[i].SetActive(i <= 0);
        }

        for (int i = 0; i < 9; i++)
        {
            text_Choice[i].gameObject.SetActive(false);
            text_Level[i].text = "LV 1";
            unitLevel[i] = 1;
        }
    }

    //ユニットを選択
    public void ChoiceUnit(int id)
    {
        if (unitIndex >= 5 || text_Choice[id].gameObject.activeSelf) return;

        unitId[unitIndex] = id;
        text_Choice[id].gameObject.SetActive(true);
        text_Choice[id].text = (unitIndex + 1).ToString();

        unitIndex++;
    }

    //ユニットのレベルアップ
    public void UnitLevelUp(int id)
    {
        unitLevel[id]++;
        text_Level[id].text = "LV " + unitLevel[id].ToString();
    }

    //ステージ選択
    public void ChoiceStage(int id = -1)
    {
        if (unitIndex <= 0) return;

        //ステージ選択画面を開く
        if (id < 2)
        {
            //初期ステータスを設定
            ParameterManager.Instance.maxUnitPossession = 5;
            ParameterManager.Instance.maxInstallation = 10;
            ParameterManager.Instance.sameUnitMaxInstallation = 3;

            //選んだユニットを取得する
            for (int i = 0; i < unitIndex; i++)
            {
                ParameterManager.Instance.AddUnit(unitId[i]);
                ParameterManager.Instance.unitStatus[i].exp = (unitLevel[unitId[i]] - 1) * 10;

                for (int j = 1; j < unitLevel[unitId[i]]; j++)
                {
                    ParameterManager.Instance.LevelUp(i);
                }
            }

            window[0].SetActive(false);
            window[1].SetActive(true);
        }
        //選んだステージを開始する
        else
        {
            SoundManager.Instance.PlaySE_Sys(1);
            TitleManager.Instance.LoadS(id);
        }
    }

    //ウィンドウ切り替え
    public void WindowChange()
    {
        if (window[0].activeSelf) return;

        SoundManager.Instance.PlaySE_Sys(0);

        if (!window[1].activeSelf)
        {
            window[2].SetActive(false);
            window[1].SetActive(true);
        }
        else if (!window[2].activeSelf)
        {
            window[1].SetActive(false);
            window[2].SetActive(true);
        }
    }
}

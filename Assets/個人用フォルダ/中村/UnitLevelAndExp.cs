using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitLevelAndExp : Singleton<UnitLevelAndExp>
{
    [SerializeField] GameObject window;
    [SerializeField] GameObject[] units;
    [SerializeField] GameObject[] choiceWindow;
    [SerializeField] TextMeshProUGUI text_GetExp;
    [SerializeField] TextMeshProUGUI[] text_Lv;
    [SerializeField] TextMeshProUGUI[] text_Exp;

    /// <summary>
    /// 選択されたユニットの経験値割り振りとステータスの項目を表示する
    /// </summary>
    public void ChoiceUnit(int index)
    {
        if (!choiceWindow[index].activeSelf)
        {
            SoundManager.Instance.PlaySE_Sys(0);

            //他のウィンドウを閉じる
            for (int i = 0; i < choiceWindow.Length; i++)
            {
                choiceWindow[i].SetActive(false);
            }

            choiceWindow[index].SetActive(true);
        }
        else
        {
            SoundManager.Instance.PlaySE_Sys(2);
            choiceWindow[index].SetActive(false);
        }
    }

    /// <summary>
    /// ステータスの確認
    /// </summary>
    public void ViewStatus(int index)
    {
        //ステータス画面を開く
        if (!EventWindowManager.Instance.window_Status.gameObject.activeSelf)
        {
            EventWindowManager.Instance.ViewStatusIndex(index);
        }
        //ステータス画面を閉じる
        else
        {
            EventWindowManager.Instance.ViewStatusIndex(-1);
        }
    }

    /// <summary>
    /// 経験値の割り振り
    /// </summary>
    public void GiveExp(int index, int value)
    {
        if (value <= 0 || ParameterManager.Instance.getExp <= 0 ||
            ParameterManager.Instance.unitStatus[index].lv >= UnitsData.Instance.levelUpExp.Length) return;

        bool levelup = false;

        //割り振る経験値が0になるまで繰り返す
        while (value > 0)
        {
            int now_GetExp = ParameterManager.Instance.getExp;
            int now_Level = ParameterManager.Instance.unitStatus[index].lv;
            int now_Exp = ParameterManager.Instance.unitStatus[index].exp;
            int remainingNeedExp = UnitsData.Instance.levelUpExp[now_Level] - now_Exp;

            //必要経験値、所持経験値以上を割り振らないよう調整
            int giveValue = (value <= remainingNeedExp) ? value : remainingNeedExp;
            giveValue = (giveValue <= now_GetExp) ? giveValue : now_GetExp;

            //必要経験値に達したらレベルアップ
            if (giveValue >= remainingNeedExp)
            {
                ParameterManager.Instance.LevelUp(index);
                levelup = true;
            }

            ParameterManager.Instance.getExp -= giveValue;
            ParameterManager.Instance.unitStatus[index].exp += giveValue;
            value -= giveValue;
        }
        
        Instance.NewStatus(false);

        //レベルアップした場合は演出を入れる
        if (levelup)
        {
            SoundManager.Instance.PlaySE_Sys(2);
        }
        else
        {
            //クリック音
            SoundManager.Instance.PlaySE_Sys(0);
        }
    }
    /// <summary>
    /// 経験値を1ずつ割り振り
    /// </summary>
    public void Give1Exp(int index)
    {
        GiveExp(index, 1);
    }
    /// <summary>
    /// 経験値を10ずつ割り振り
    /// </summary>
    public void Give10Exp(int index)
    {
        GiveExp(index, 10);
    }

    /// <summary>
    /// ステータス、経験値割り振り画面を閉じる
    /// </summary>
    public void Exit()
    {
        SoundManager.Instance.PlaySE_Sys(3);

        NewStatus();
        window.SetActive(false);
    }

    /// <summary>
    /// テキストとアクティブ状態を更新
    /// </summary>
    public void NewStatus(bool init = true)
    {
        text_GetExp.text = "所持経験値：" + ParameterManager.Instance.getExp.ToString();

        for (int i = 0; i < ParameterManager.Instance.unitStatus.Length; i++)
        {
            if (init)
            {
                //画像と名前を読み込み
                units[i].GetComponent<Image>().sprite = UnitsData.Instance.iconBackSprite[ParameterManager.Instance.unitStatus[i].role];
                units[i].transform.GetChild(0).GetComponent<Image>().sprite = ParameterManager.Instance.unitStatus[i].sprite;
                units[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ParameterManager.Instance.unitStatus[i].name;

                choiceWindow[i].SetActive(false);
                units[i].SetActive(true);
            }

            text_Lv[i].text = "LV  " + ParameterManager.Instance.unitStatus[i].lv.ToString();
            text_Exp[i].text = (ParameterManager.Instance.unitStatus[i].lv < UnitsData.Instance.levelUpExp.Length) ? "EXP " + ParameterManager.Instance.unitStatus[i].exp.ToString() : "EXP...Max"; 
        }
        for (int i = ParameterManager.Instance.unitStatus.Length; i < units.Length; i++)
        {
            if (init)
            {
                units[i].SetActive(false);
            }
        }
    }
}

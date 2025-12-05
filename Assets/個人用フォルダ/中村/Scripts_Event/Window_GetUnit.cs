using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ユニット入手画面
/// </summary>
public class Window_GetUnit : MonoBehaviour
{
    //各ユニットのボタン用変数
    [SerializeField] GameObject unitButtonParent;
    [SerializeField] Button unitButtonPredab;

    //状態を表すフラグ
    public bool isActive;

    /// <summary>
    /// ユニット入手画面を開く、閉じる時に呼び出す　引数で表示するユニットの数を指定
    /// </summary>
    public void ViewUnits(int num = - 1)
    {
        //ユニット入手画面を開く
        if (!isActive && num >= 0)
        {
            //入手できるユニットが残っていない場合、または所持ユニット数が最大の場合は戻る
            if (UnitsData.Instance.unit.Length - ParameterManager.Instance.unitStatus.Length - 1 <= 0 ||
                ParameterManager.Instance.unitStatus.Length >= ParameterManager.Instance.maxUnitPossession)
            {
                //ユニットを入手できなかった場合のリザルト
                EventWindowManager.Instance.window_Event.Result("これ以上ユニットを増やせない！", "");
                gameObject.SetActive(false);
                return;
            }

            SoundManager.Instance.PlaySE_Sys(0);

            //ユニットをnum体表示
            GenerateUnitButton(num);

            //フラグを設定
            isActive = true;
        }
        //ユニット入手画面を閉じる
        else
        {
            SoundManager.Instance.PlaySE_Sys(2);

            //フラグを設定
            isActive = false;

            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ユニットの初期ステータス画面を開く　引数でIDを指定
    /// </summary>
    public void ViewStatus(int id)
    {
        if (!isActive) return;

        EventWindowManager.Instance.ViewStatus(id);
    }

    /// <summary>
    /// 味方にするユニットを選ぶ　引数でIDを指定
    /// </summary>
    public void ChoiceUnit(int id)
    {
        if (!isActive) return;

        //ユニットを入手
        int preUnitNum = ParameterManager.Instance.unitStatus.Length;
        ParameterManager.Instance.AddUnit(id);
        bool getUnit = (ParameterManager.Instance.unitStatus.Length > preUnitNum);

        //ユニット入手画面を閉じる
        ViewUnits();

        //リザルトを表示
        if (getUnit)
        {
            EventWindowManager.Instance.window_Event.Result(UnitsData.Instance.unit[id].name + "が仲間になった！", "", UnitsData.Instance.unit[id].sprite);
        }
        //ユニットを入手できなかった場合のリザルト
        else
        {
            EventWindowManager.Instance.window_Event.Result("これ以上ユニットを増やせない！", "");
        } 
    }

    //持っていないユニットの中から抽選してボタンを生成する　引数でボタンの数を指定
    void GenerateUnitButton(int num)
    {
        foreach (Transform n in unitButtonParent.transform) Destroy(n.gameObject); //全ての子オブジェクトを削除   

        unitButtonParent.SetActive(true);

        //入手済のユニットのIDを読み込み
        bool[] exclusionUnit = new bool[UnitsData.Instance.unit.Length]; 
        for (int i = 0; i < ParameterManager.Instance.unitStatus.Length; i++)
        {
            exclusionUnit[ParameterManager.Instance.unitStatus[i].id] = true;
        }

        for (int i = 0; i < num; i++)
        {
            //所持ユニットの数が残りのユニットより多い場合は戻る
            if (i > UnitsData.Instance.unit.Length - ParameterManager.Instance.unitStatus.Length - 1)
            {
                //ユニットを入手できなかった場合のリザルト
                if (i <= 0)
                {
                    ViewUnits();
                    EventWindowManager.Instance.window_Event.Result("これ以上ユニットを増やせない！", "");
                }

                return;
            }

            //ユニットのIDを抽選する
            int id = Random.Range(0, UnitsData.Instance.unit.Length);
            if (exclusionUnit[id])
            {
                for (int j = 0; j < Mathf.Max(ParameterManager.Instance.unitStatus.Length, 1) * 100; j++)
                {
                    id = Random.Range(0, UnitsData.Instance.unit.Length);
                    if (!exclusionUnit[id])
                    {
                        exclusionUnit[id] = true;
                        break;
                    }
                }
            }
            else
            {
                exclusionUnit[id] = true;
            }

            //ボタンを生成
            Button button = Instantiate(unitButtonPredab);
            button.transform.SetParent(unitButtonParent.transform);
            button.transform.localScale = unitButtonParent.transform.localScale;

            //アイコンを読み込み
            GameObject icon = button.transform.GetChild(0).gameObject;
            icon.GetComponent<Image>().sprite = UnitsData.Instance.iconBackSprite[UnitsData.Instance.unit[id].role];
            icon.transform.GetChild(0).GetComponent<Image>().sprite = UnitsData.Instance.unit[id].sprite;
            icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = UnitsData.Instance.unit[id].name;
            icon.transform.GetChild(2).gameObject.SetActive(false);

            //ユニットの情報を読み込み
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = UnitsData.Instance.unit[id].info;

            //ボタンのOnClickを割り当て
            int _i = id;
            button.onClick.AddListener(() => ChoiceUnit(_i));
            //ステータス確認ボタンのOnClickを割り当て
            button.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ViewStatus(_i));
        }
    }
}

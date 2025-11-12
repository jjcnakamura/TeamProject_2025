using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitLevelAndExp : Singleton<UnitLevelAndExp>
{
    [SerializeField] GameObject[] units;
    [SerializeField] TextMeshProUGUI[] text_Lv;
    [SerializeField] TextMeshProUGUI[] text_Exp;

    void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.C))
        {
            NewStatus();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ParameterManager.Instance.AddUnit(ParameterManager.Instance.unitStatus.Length);
        }
    }

    /// <summary>
    /// テキストとアクティブ状態を更新
    /// </summary>
    public void NewStatus()
    {
        for (int i = 0; i < ParameterManager.Instance.unitStatus.Length; i++)
        {
            text_Lv[i].text = ParameterManager.Instance.unitStatus[i].lv.ToString();
            text_Exp[i].text = ParameterManager.Instance.unitStatus[i].exp.ToString();

            units[i].SetActive(true);
        }
        for (int i = ParameterManager.Instance.unitStatus.Length; i < units.Length; i++)
        {
            units[i].SetActive(false);
        }
    }
}

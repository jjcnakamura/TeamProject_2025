using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitLevelAndExp : Singleton<UnitLevelAndExp>
{
    [SerializeField] GameObject[] units;
    [SerializeField] TextMeshProUGUI text_GetExp;
    [SerializeField] TextMeshProUGUI[] text_Lv;
    [SerializeField] TextMeshProUGUI[] text_Exp;

    /// <summary>
    /// テキストとアクティブ状態を更新
    /// </summary>
    public void NewStatus()
    {
        text_GetExp.text = "所持経験値..." + ParameterManager.Instance.getExp.ToString();

        for (int i = 0; i < ParameterManager.Instance.unitStatus.Length; i++)
        {
            text_Lv[i].text = "LV...   " + ParameterManager.Instance.unitStatus[i].lv.ToString();
            text_Exp[i].text = "EXP..." + ParameterManager.Instance.unitStatus[i].exp.ToString();

            units[i].SetActive(true);
        }
        for (int i = ParameterManager.Instance.unitStatus.Length; i < units.Length; i++)
        {
            units[i].SetActive(false);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class toggle : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles; // 9個のToggleを登録
    [SerializeField] private int maxChecked = 3;   // 同時にONにできる最大数

    private void Start()
    {
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle); });
        }
    }

    private void OnToggleChanged(Toggle changedToggle)
    {
        // 現在ONの数を数える
        int onCount = 0;
        foreach (var t in toggles)
        {
            if (t.isOn) onCount++;
        }

        // もし上限を超えていたら、今回ONにしたToggleをOFFに戻す
        if (onCount > maxChecked)
        {
            changedToggle.isOn = false;
        }/*
        if (onCount > maxChecked)//こっちを前のワールドで使ったキャラを引き継ぐ時にこれをはずすやつ
        {
            changedToggle.isOn = false;
        }*/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCharaEnter : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles; // 9個のToggleを登録
    private List<int> activeOrder = new List<int>(); // 一時的なリスト
    private int[] activeIndexes;                    // 最終的な配列

    private void Start()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i; // ★重要：ローカル変数に保存（ラムダ式で正しく参照するため）
            toggles[i].onValueChanged.AddListener((bool isOn) => OnToggleChanged(index, isOn));
        }
    }

    void OnToggleChanged(int index, bool isOn)
    {
        if (isOn)
        {
            // チェックが付いたら順番を記録
            if (!activeOrder.Contains(index))
                activeOrder.Add(index);
        }
        else
        {
            // チェックが外れたら削除
            activeOrder.Remove(index);
        }

        activeOrder.Sort();

        // List → 配列に変換
        activeIndexes = activeOrder.ToArray();

        Debug.Log("現在の順番: " + string.Join(", ", activeIndexes));
    }

    public void EnterButton()//ボタン用
    {
        var status = ParameterManager.Instance.unitStatus;
        for (int i = 0; i < activeIndexes.Length; i++)
        {
            ParameterManager.Instance.AddUnit(activeIndexes[i]);
            Debug.Log($"インデックス {i} の値は {activeIndexes[i]}");//設定したキャラをマップで表示するためのもの
        }
    }

    void Update()
    {
        
    }
}

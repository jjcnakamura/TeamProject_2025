using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCharaEnter : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles; // 9個のToggleを登録
    private List<int> activeOrder = new List<int>(); // 一時的なリスト
    public int[] activeIndexes;          // 最終的な配列

    public GameObject BackCanvas1;//進めない
    public GameObject BackCanvas2;//攻撃キャラがいないけどいいのか
    public GameObject BackCanvas3;//進めるけどいいのか

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
        //var status = ParameterManager.Instance.unitStatus;
        for (int i = 0; i < activeIndexes.Length; i++)
        {
            ParameterManager.Instance.AddUnit(activeIndexes[i]);
            //Debug.Log($"インデックス {i} の値は {activeIndexes[i]}");//設定したキャラをマップで表示するためのもの
        }
        MapManager.Instance.GameStart();
    }

    public void IfBackCanvasButton()
    {
        if (activeIndexes.Length == 0)
        {
            BackCanvas1.SetActive(true);//進めない
            return;
        }
        foreach (int n in activeIndexes)
        {
            if(activeIndexes.Length >= 3)
            {
                if (n != 0 || n != 1 || n != 2 || n != 3 || n != 5)
                {
                    if (n == 4 || n == 6 || n == 7 || n == 8)
                    {
                        BackCanvas2.SetActive(true);//攻撃キャラがいないけどいいのか
                        return;
                    }
                }
                if(n == 0 || n == 1 || n == 2 || n == 3 || n == 5)
                {
                    EnterButton();
                    return;
                }
            }
            if(n != 0 || n != 1 || n != 2 || n != 3 || n != 5)
            {
                if (n == 4 || n == 6 || n == 7 || n == 8)
                {
                    BackCanvas2.SetActive(true);//攻撃キャラがいないけどいいのか
                    return;
                }
            }
        }
        if (activeIndexes.Length == 1 || activeIndexes.Length == 2)
        {
            BackCanvas3.SetActive(true);//進めるけどいいのか
            return;
        }
    }

    public void BackCanvas(GameObject i)
    {
        i.SetActive(false);
    }
}

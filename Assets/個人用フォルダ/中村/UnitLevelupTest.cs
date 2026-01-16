using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レベルアップのテスト用
/// </summary>
public class UnitLevelupTest : MonoBehaviour
{
    [SerializeField] GameObject eventCanvas;

    void Awake()
    {
        eventCanvas.SetActive(true);
    }

    void Start()
    {
        ParameterManager.Instance.getExp = 2000;
        ParameterManager.Instance.maxUnitPossession = 5;
        ParameterManager.Instance.maxInstallation = 10;
        ParameterManager.Instance.sameUnitMaxInstallation = 3;
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(3);
        ParameterManager.Instance.AddUnit(5);
        ParameterManager.Instance.AddUnit(8);
        ParameterManager.Instance.AddUnit(6);

        UnitLevelAndExp.Instance.NewStatus();
    }
}

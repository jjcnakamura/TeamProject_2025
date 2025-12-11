using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : Singleton<DebugScript>
{

    void Awake()
    {
        //シーンを遷移しても残る
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //デバック用にユニット増やすやつ
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ParameterManager.Instance.AddUnit(0);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ParameterManager.Instance.AddUnit(1);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ParameterManager.Instance.AddUnit(2);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ParameterManager.Instance.AddUnit(3);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ParameterManager.Instance.AddUnit(4);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ParameterManager.Instance.AddUnit(5);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ParameterManager.Instance.AddUnit(6);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ParameterManager.Instance.AddUnit(7);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ParameterManager.Instance.AddUnit(8);
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        //ユニットの所持数が１増えるやつ
        if (Input.GetKeyDown(KeyCode.H))
        {
            ParameterManager.Instance.maxUnitPossession++;
        }

        //ユニットの配置数増やすやつ
        if (Input.GetKeyDown(KeyCode.J))
        {
            ParameterManager.Instance.maxInstallation++;
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        //同名ユニットの配置数増やすやつ
        if (Input.GetKeyDown(KeyCode.K))
        {
            ParameterManager.Instance.sameUnitMaxInstallation++;
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Start();
        }

        //ポイントmax
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.PointChange(999);
        }

        //ステージクリア
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (FindObjectOfType(System.Type.GetType("BattleManager")) != null)
                BattleManager.Instance.Clear();
        }
    }
}

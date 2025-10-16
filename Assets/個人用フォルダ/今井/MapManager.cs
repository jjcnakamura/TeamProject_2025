using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public ParameterManager.UnitStatus unitStatus;
    public GameObject MapImage;

    
    public int[] Stageint;//ステージ数
    public int[] MapSelectBlock;//分岐路数

    public int[] UnitUpGold;//必要ゴールド数
    public int Gold;//(仮)

    public GridLayoutGroup grid1;//GridLayoutGroupの指定上
    public GridLayoutGroup grid2;//GridLayoutGroupの指定中
    public GridLayoutGroup grid3;//GridLayoutGroupの指定下
    public float X = 0f;//GridLayoutGroupのspacingを変えてステージを収める因数

    void Update()
    {
        if (Stageint[0] >= 7)
        {
            int BlockX = Stageint[0] - 7;
            BlockX -= 5;
        }
        grid1.spacing = new Vector2(X, 0);
        if (Stageint[1] >= 7)
        {
            int BlockX = Stageint[1] - 7;
            BlockX -= 5;
        }
        grid2.spacing = new Vector2(X, 0);
        if (Stageint[2] >= 7)
        {
            int BlockX = Stageint[2] - 7;
            BlockX -= 5;
        }
        grid3.spacing = new Vector2(X, 0);
    }
    

    public void EventContDown()//設置時のコスト　(短縮系)
    {
        unitStatus.cost -= 1;//（仮）
    }
    public void EventrecastDown()//再配置までの時間　(短縮系)
    {
       unitStatus.recast -= 1;//（仮）
    }
    public void EventmaxInstallationUp()//ユニット最大配置数　(増加系)
    {
        ParameterManager.Instance.maxInstallation += 1;//（仮）
    }
    public void EventmaxUnitPossessionUp()//最大ユニット所持数　(増加系)
    {
        ParameterManager.Instance.maxUnitPossession += 1;//（仮）
    }
    public void EventsameUnitMaxInstallationUp()//同じユニットの最大配置数　(増加系)
    {
        ParameterManager.Instance.sameUnitMaxInstallation += 1;//（仮）
    }
    public void Event()//HP回復する（回復）
    {
        //（仮）
    }
    public void UnitLevelUpBottun()
    {
        if (Gold >= UnitUpGold[unitStatus.lv])
        {
            Gold -= UnitUpGold[unitStatus.lv];
            unitStatus.lv += 1;
            unitStatus.hp += 1;
            unitStatus.value += 1;
            unitStatus.interval += 1;
            unitStatus.distance += 1;
            unitStatus.range += 1;
            
        }
        if(Gold < UnitUpGold[unitStatus.lv])
        {
            //音かテキストを出して出来ないことをしめす
        }
    }
}

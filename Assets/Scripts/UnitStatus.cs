/// <summary>
/// ゲーム内で増減する各ユニットのパラメータ
/// </summary>
[System.Serializable]
public class UnitStatus
{
    public int id;      //どのユニットかを示すID
    public int role;    //ロール　0がDPS、1がタンク、2がサポート

    public int lv;      //レベル
    public int exp;     //所持経験値

    public int cost;    //設置時のコスト
    public int recast;  //再配置までの時間

    public int hp;      //耐久値（最大HP）
    public int value;   //DPSの場合は攻撃力、サポートの場合は回復量、ポイント増加量など
    public float interval; //行動速度（攻撃、回復をする間隔）
    public float distance; //攻撃、回復の射程
    public float range;    //範囲攻撃の範囲
}

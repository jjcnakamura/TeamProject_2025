/// <summary>
/// 各ユニットのパラメータ
/// </summary>
[System.Serializable]
public class UnitStatus
{
    public int role;   //ロール
    public int cost;   //コスト
    public int recast; //リキャスト時間

    public int hp;     //最大HP
    public int atk;    //攻撃力
    public int def;    //防御力
    public int heal;   //回復量
    public float speed;  //攻撃、回復の速度
    public float syatei; //攻撃、回復の射程
    public float hani;   //範囲攻撃の範囲
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public Route[] routePoint;
    Vector3 spawnPos;

    [Space(20)]

    [SerializeField] Status[] enemyStatus;

    float[] spawnTime;
    int spawnIndex;

    //bool waitCoroutine;
    bool endSpawn;

    [Space(20)]

    [SerializeField] GameObject spawnPoint; //敵の出現位置;

    void Start()
    {
        //敵の情報を配列に格納
        spawnTime = new float[enemyStatus.Length];
        for (int i = 0; i < enemyStatus.Length; i++)
        {
            //敵のPrefabをEnemiesDataから取得
            enemyStatus[i].prefab = EnemiesData.Instance.enemy[enemyStatus[i].id].prefab;

            //出現時間を配列に格納
            spawnTime[i] = enemyStatus[i].spawnTime;

            //敵の総出現数をカウント
            BattleManager.Instance.nowEnemyNum += enemyStatus[i].spawnNum;
        }

        //敵のルートを配列に格納
        for (int i = 0; i < routePoint.Length; i++)
        {
            routePoint[i].pos = new Vector3[routePoint[i].pointObj.Length + 1];

            int j;
            for (j = 0; j < routePoint[i].pointObj.Length; j++)
            {
                routePoint[i].pos[j] = routePoint[i].pointObj[j].transform.position;
            }
            routePoint[i].pos[j] = BattleManager.Instance.playerSide.transform.position;
        }

        //出現時の位置を取得
        spawnPos = spawnPoint.transform.position;
    }

    void Update()
    {
        if (!BattleManager.Instance.isMainGame) return; //メインゲーム中でなければ戻る

        if (!endSpawn && BattleManager.Instance.timer_EnemySpawn > spawnTime[spawnIndex]) StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if (endSpawn) yield break;

        int index = spawnIndex;
        spawnIndex++;
        if (spawnIndex >= enemyStatus.Length) endSpawn = true;

        //敵のステータスを設定
        GameObject enemy = enemyStatus[index].prefab;
        Enemy_Base enemyBase = enemy.GetComponent<Enemy_Base>();

        //位置と角度を設定
        enemyBase.spawnPoint = this;
        Quaternion targetDir = Quaternion.LookRotation(routePoint[enemyStatus[index].routeIndex].pos[0] - spawnPos);
        Quaternion spawnDir = new Quaternion(enemy.transform.rotation.x, targetDir.y, enemy.transform.rotation.z, targetDir.w);

        //パラメータを設定
        enemyBase.maxHp = enemyStatus[index].hp;
        enemyBase.hp = enemyBase.maxHp;
        enemyBase.value = enemyStatus[index].value;
        enemyBase.defaultValue = enemyBase.value;
        enemyBase.interval = enemyStatus[index].interval;
        enemyBase.distance = enemyStatus[index].distance;
        enemyBase.range = enemyStatus[index].range;
        enemyBase.moveSpeed = enemyStatus[index].moveSpeed;
        enemyBase.knockBackTime = enemyStatus[index].knockBackTime;

        enemyBase.routeIndex = enemyStatus[index].routeIndex;

        for (int i = 0; i < enemyStatus[index].spawnNum; i++)
        {
            //敵のインスタンスを生成
            GameObject instance = Instantiate(enemy);
            instance.transform.position = spawnPos;
            instance.transform.rotation = spawnDir;

            //一定時間待つ
            yield return new WaitForSeconds(enemyStatus[index].spawnInterval);
        }
    }

    //ステータスの構造体
    [System.Serializable]
    public struct Status
    {
        [SerializeField, Label("敵のID")] public int id;
        [SerializeField, Label("どのルートを通るか")] public int routeIndex;
        [SerializeField, Label("出現までの時間")] public float spawnTime;
        [SerializeField, Label("出現する間隔")] public float spawnInterval;
        [SerializeField, Label("出現数")] public int spawnNum;

        [Space(10)]

        public int hp;
        public int value;
        public float interval;
        public float distance;
        public float range;
        public float moveSpeed;
        public float knockBackTime;

        [System.NonSerialized] public GameObject prefab; //敵のPrefab
    }

    //敵のルートを格納する構造体
    [System.Serializable]
    public struct Route
    {
        public GameObject[] pointObj;
        [System.NonSerialized] public Vector3[] pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public Route[] routePoint;
    float spawnPosY;

    [Space(20)]

    [SerializeField] Status[] enemyStatus;

    float[] spawnTime;
    int spawnIndex;

    bool waitCoroutine;
    bool endSpawn;

    void Start()
    {
        //敵出現までの時間を配列に格納
        spawnTime = new float[enemyStatus.Length];
        for (int i = 0; i < spawnTime.Length; i++)
        {
            spawnTime[i] = enemyStatus[i].spawnTime;
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

        //出現時の高さを取得
        spawnPosY = transform.GetChild(0).position.y;
    }

    void Update()
    {
        if (endSpawn || waitCoroutine) return;

        if (BattleManager.Instance.timer_EnemySpawn > spawnTime[spawnIndex]) StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        if (endSpawn) yield break;

        //敵のステータスを設定
        GameObject enemy = enemyStatus[spawnIndex].prefab;
        Enemy_Base enemyBase = enemy.GetComponent<Enemy_Base>();

        //位置と角度を設定
        enemyBase.spawnPoint = this;
        Vector3 spawnPos = new Vector3(transform.position.x, spawnPosY, transform.position.z);
        Quaternion targetDir = Quaternion.LookRotation(routePoint[enemyStatus[spawnIndex].routeIndex].pos[0] - spawnPos);
        Quaternion spawnDir = new Quaternion(enemy.transform.rotation.x, targetDir.y, enemy.transform.rotation.z, targetDir.w);

        //パラメータを設定
        enemyBase.maxHp = enemyStatus[spawnIndex].hp;
        enemyBase.hp = enemyBase.maxHp;
        enemyBase.value = enemyStatus[spawnIndex].value;
        enemyBase.interval = enemyStatus[spawnIndex].interval;
        enemyBase.distance = enemyStatus[spawnIndex].distance;
        enemyBase.range = enemyStatus[spawnIndex].range;
        enemyBase.moveSpeed = enemyStatus[spawnIndex].moveSpeed;

        enemyBase.routeIndex = enemyStatus[spawnIndex].routeIndex;


        for (int i = 0; i < enemyStatus[spawnIndex].spawnNum; i++)
        {
            //敵のインスタンスを生成
            GameObject instance = Instantiate(enemy);
            instance.transform.position = spawnPos;
            instance.transform.rotation = spawnDir;

            //一定時間待つ
            waitCoroutine = true;
            yield return new WaitForSeconds(enemyStatus[spawnIndex].spawnInterval);
            waitCoroutine = false;
        }
        
        spawnIndex++;
        if (spawnIndex >= enemyStatus.Length) endSpawn = true;
    }

    //ステータスの構造体
    [System.Serializable]
    public struct Status
    {
        [SerializeField, Label("敵のPrefab")] public GameObject prefab;
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
    }

    //
    [System.Serializable]
    public struct Route
    {
        public GameObject[] pointObj;
        [System.NonSerialized] public Vector3[] pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] Status[] enemyStatus;

    float[] spawnTime;
    int spawnIndex;

    public Vector3[] routePoint { get; private set; }

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
        routePoint = new Vector3[transform.childCount + 1];
        for (int i = 0; i < transform.childCount; i++)
        {
            routePoint[i] = transform.GetChild(i).position;
        }
        routePoint[routePoint.Length - 1] = BattleManager.Instance.playerSide.transform.position;
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
        Vector3 spawnPos = new Vector3(transform.position.x, enemyStatus[spawnIndex].prefab.transform.position.y, transform.position.z);
        Quaternion targetDir = Quaternion.LookRotation(routePoint[0] - spawnPos);
        Quaternion spawnDir = new Quaternion(enemy.transform.rotation.x, targetDir.y, enemy.transform.rotation.z, targetDir.w);

        //パラメータを設定
        enemyBase.maxHp = enemyStatus[spawnIndex].hp;
        enemyBase.hp = enemyBase.maxHp;
        enemyBase.value = enemyStatus[spawnIndex].value;
        enemyBase.interval = enemyStatus[spawnIndex].interval;
        enemyBase.distance = enemyStatus[spawnIndex].distance;
        enemyBase.range = enemyStatus[spawnIndex].range;
        enemyBase.moveSpeed = enemyStatus[spawnIndex].moveSpeed;


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
}

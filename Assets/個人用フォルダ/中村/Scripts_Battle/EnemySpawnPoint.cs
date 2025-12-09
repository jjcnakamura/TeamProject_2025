using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public Route[] routePoint;      //敵が進む道
    Vector3 spawnPos;               //敵の出現位置

    GameObject[] routeLineParent;   //敵のルートの表示
    float blockScale = 2f;          //ステージ上の１ブロックの大きさ
    float routeArrowOffsetY = 0.5f; //敵のルート表示時の矢印の高さ

    [Space(20)]

    [SerializeField] Status[] enemyStatus; //生成する敵のステータス

    float[] spawnTime; //敵の出現時間
    int spawnIndex;    //次に出現する敵

    bool endSpawn;     //全ての敵が出現したか

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
            //出現時間からルート表示時間を引いた数字を配列に格納
            spawnTime[i] = Mathf.Max(enemyStatus[i].spawnTime - BattleManager.Instance.preEnemySpawnTime, 0);
            //敵の総出現数をカウント
            BattleManager.Instance.nowEnemyNum += enemyStatus[i].spawnNum;
            BattleManager.Instance.text_EnemyNum.text = BattleManager.Instance.nowEnemyNum.ToString();
        }

        //敵のルートの表示数を確保
        routeLineParent = new GameObject[enemyStatus.Length];

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

    //敵を生成する
    IEnumerator Spawn()
    {
        if (endSpawn) yield break;

        int index = spawnIndex;
        spawnIndex++;
        if (spawnIndex >= enemyStatus.Length) endSpawn = true;

        //敵のステータスを設定
        Enemy_Base enemy = enemyStatus[index].prefab.GetComponent<Enemy_Base>();

        //位置と角度を設定
        enemy.spawnPoint = this;
        Quaternion targetDir = Quaternion.LookRotation(routePoint[enemyStatus[index].routeIndex].pos[0] - spawnPos);
        Quaternion spawnDir = new Quaternion(enemy.transform.rotation.x, targetDir.y, enemy.transform.rotation.z, targetDir.w);

        //パラメータを設定
        enemy.maxHp = enemyStatus[index].hp;
        enemy.hp = enemy.maxHp;
        enemy.value = enemyStatus[index].value;
        enemy.defaultValue = enemy.value;
        enemy.interval = enemyStatus[index].interval;
        enemy.distance = enemyStatus[index].distance;
        enemy.range = enemyStatus[index].range;
        enemy.moveSpeed = enemyStatus[index].moveSpeed;
        enemy.defaultMoveSpeed = enemy.moveSpeed;
        enemy.knockBackTime = enemyStatus[index].knockBackTime;

        enemy.routeIndex = enemyStatus[index].routeIndex;

        enemy.se_Action = enemyStatus[index].se_Action;

        //敵のルートを指定した秒数表示
        SoundManager.Instance.PlaySE_Game(7, false);
        GenerateRouteLine(index);
        float mod = BattleManager.Instance.enemyRouteActiveTime % 1f;
        for (int i = 0; i < BattleManager.Instance.enemyRouteActiveTime; i++)
        {
            routeLineParent[index].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            routeLineParent[index].SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        routeLineParent[index].SetActive(false);
        yield return new WaitForSeconds(mod);
        GenerateRouteLine(index, true);
        yield return new WaitForSeconds(BattleManager.Instance.preEnemySpawnTime - BattleManager.Instance.enemyRouteActiveTime);

        for (int i = 0; i < enemyStatus[index].spawnNum; i++)
        {
            //敵のインスタンスを生成
            Enemy_Base instance = Instantiate(enemy);
            instance.transform.position = spawnPos;
            instance.transform.rotation = spawnDir;

            //HPバーを生成
            Hpbar hpbar = Instantiate(BattleManager.Instance.hpbarPrefab).GetComponent<Hpbar>();
            hpbar.transform.SetParent(BattleManager.Instance.hpbarParent.transform);
            hpbar.transform.localScale = new Vector3(1f, 1f, 1f);
            hpbar.transform.localRotation = new Quaternion();
            hpbar.targetEnemy = instance;
            instance.hpbarObj = hpbar.gameObject;

            //一定時間待つ
            yield return new WaitForSeconds(enemyStatus[index].spawnInterval);
        }
    }
    
    //敵のルートを表示する
    void GenerateRouteLine(int index, bool destroy = false)
    {
        //ルートの表示を削除する場合はそのまま戻る
        if (destroy && routeLineParent[index] != null)
        {
            Destroy(routeLineParent[index]);
            return;
        }
        //同じルートを表示する場合は元々表示されていたものを削除する
        if (!destroy && routeLineParent[index] != null)
        {
            Destroy(routeLineParent[index]);
        }

        float tolerance = blockScale / 2f;
        routeLineParent[index] = new GameObject();

        for (int i = -1; i < routePoint[enemyStatus[index].routeIndex].pos.Length - 1; i++)
        {
            float posX = (i >= 0) ? routePoint[enemyStatus[index].routeIndex].pos[i].x : spawnPos.x;
            float posZ = (i >= 0) ? routePoint[enemyStatus[index].routeIndex].pos[i].z : spawnPos.z;

            //横に移動する場合
            if (Mathf.Abs(posX - routePoint[enemyStatus[index].routeIndex].pos[i + 1].x) >= tolerance)
            {
                //生成する矢印オブジェクトの数を決定
                int brockNum = (int)(Mathf.Abs(posX - routePoint[enemyStatus[index].routeIndex].pos[i + 1].x) /blockScale);
                int arrowNum = (i >= 0) ? brockNum : brockNum - 1;
                //次のルートがプラス方向かマイナス方向か
                int dir = (posX < routePoint[enemyStatus[index].routeIndex].pos[i + 1].x) ? 1 : -1;
                float arrowPosX = posX;
                if (i < 0) arrowPosX += (blockScale * dir);
                //ブロックごとに矢印を生成する
                for (int j = 0; j < arrowNum; j++)
                {
                    //矢印を生成して位置、角度、大きさを調整
                    GameObject arrow = Instantiate(BattleManager.Instance.enemyRouteArrow);
                    arrow.transform.position = new Vector3(arrowPosX, transform.position.y + routeArrowOffsetY, posZ);
                    arrow.transform.Rotate(0, (dir < 0) ? 270f : 90f, 0);
                    arrow.transform.localScale = BattleManager.Instance.enemyRouteArrow.transform.localScale;
                    arrow.transform.SetParent(routeLineParent[index].transform);
                    //次の生成位置を決定
                    arrowPosX += (blockScale * dir);
                }
            }
            //縦に移動する場合
            else if (Mathf.Abs(posZ - routePoint[enemyStatus[index].routeIndex].pos[i + 1].z) >= tolerance)
            {
                //生成する矢印オブジェクトの数を決定
                int brockNum = (int)(Mathf.Abs(posZ - routePoint[enemyStatus[index].routeIndex].pos[i + 1].z) / blockScale);
                int arrowNum = (i >= 0) ? brockNum : brockNum - 1;
                //次のルートがプラス方向かマイナス方向か
                int dir = (posZ < routePoint[enemyStatus[index].routeIndex].pos[i + 1].z) ? 1 : -1;
                float arrowPosZ = posZ;
                if (i < 0) arrowPosZ += (blockScale * dir);
                //ブロックごとに矢印を生成する
                for (int j = 0; j < arrowNum; j++)
                {
                    //矢印を生成して位置、角度、大きさを調整
                    GameObject arrow = Instantiate(BattleManager.Instance.enemyRouteArrow);
                    arrow.transform.position = new Vector3(posX, transform.position.y + routeArrowOffsetY, arrowPosZ);
                    arrow.transform.Rotate(0, (dir < 0) ? 180f : 0f, 0);
                    arrow.transform.localScale = BattleManager.Instance.enemyRouteArrow.transform.localScale;
                    arrow.transform.SetParent(routeLineParent[index].transform);
                    //次の生成位置を決定
                    arrowPosZ += (blockScale * dir);
                }
            }
        }
    }

    //ステータスの構造体
    [System.Serializable]
    public struct Status
    {
        [Label("敵のID")] public int id;
        [Label("どのルートを通るか")] public int routeIndex;
        [Label("出現までの時間")] public float spawnTime;
        [Label("出現する間隔")] public float spawnInterval;
        [Label("出現数")] public int spawnNum;
        [Header("SEの番号")] public int[] se_Action;

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

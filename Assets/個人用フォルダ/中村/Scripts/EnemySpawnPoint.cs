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

    bool waitCoroutine;
    bool endSpawn;

    [Space(20)]

    [SerializeField] GameObject spawnPoint; //�G�̏o���ʒu;

    void Start()
    {
        //�G�o���܂ł̎��Ԃ�z��Ɋi�[
        spawnTime = new float[enemyStatus.Length];
        for (int i = 0; i < spawnTime.Length; i++)
        {
            spawnTime[i] = enemyStatus[i].spawnTime;
        }

        //�G�̃��[�g��z��Ɋi�[
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

        //�o�����̈ʒu���擾
        spawnPos = spawnPoint.transform.position;
    }

    void Update()
    {
        if (endSpawn || waitCoroutine) return;

        if (BattleManager.Instance.timer_EnemySpawn > spawnTime[spawnIndex]) StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        if (endSpawn) yield break;

        //�G�̃X�e�[�^�X��ݒ�
        GameObject enemy = enemyStatus[spawnIndex].prefab;
        Enemy_Base enemyBase = enemy.GetComponent<Enemy_Base>();

        //�ʒu�Ɗp�x��ݒ�
        enemyBase.spawnPoint = this;
        Quaternion targetDir = Quaternion.LookRotation(routePoint[enemyStatus[spawnIndex].routeIndex].pos[0] - spawnPos);
        Quaternion spawnDir = new Quaternion(enemy.transform.rotation.x, targetDir.y, enemy.transform.rotation.z, targetDir.w);

        //�p�����[�^��ݒ�
        enemyBase.maxHp = enemyStatus[spawnIndex].hp;
        enemyBase.hp = enemyBase.maxHp;
        enemyBase.value = enemyStatus[spawnIndex].value;
        enemyBase.interval = enemyStatus[spawnIndex].interval;
        enemyBase.distance = enemyStatus[spawnIndex].distance;
        enemyBase.range = enemyStatus[spawnIndex].range;
        enemyBase.moveSpeed = enemyStatus[spawnIndex].moveSpeed;
        enemyBase.knockBackTime = enemyStatus[spawnIndex].knockBackTime;

        enemyBase.routeIndex = enemyStatus[spawnIndex].routeIndex;


        for (int i = 0; i < enemyStatus[spawnIndex].spawnNum; i++)
        {
            //�G�̃C���X�^���X�𐶐�
            GameObject instance = Instantiate(enemy);
            instance.transform.position = spawnPos;
            instance.transform.rotation = spawnDir;

            //��莞�ԑ҂�
            waitCoroutine = true;
            yield return new WaitForSeconds(enemyStatus[spawnIndex].spawnInterval);
            waitCoroutine = false;
        }
        
        spawnIndex++;
        if (spawnIndex >= enemyStatus.Length) endSpawn = true;
    }

    //�X�e�[�^�X�̍\����
    [System.Serializable]
    public struct Status
    {
        [SerializeField, Label("�G��Prefab")] public GameObject prefab;
        [SerializeField, Label("�ǂ̃��[�g��ʂ邩")] public int routeIndex;
        [SerializeField, Label("�o���܂ł̎���")] public float spawnTime;
        [SerializeField, Label("�o������Ԋu")] public float spawnInterval;
        [SerializeField, Label("�o����")] public int spawnNum;

        [Space(10)]

        public int hp;
        public int value;
        public float interval;
        public float distance;
        public float range;
        public float moveSpeed;
        public float knockBackTime;
    }

    //
    [System.Serializable]
    public struct Route
    {
        public GameObject[] pointObj;
        [System.NonSerialized] public Vector3[] pos;
    }
}

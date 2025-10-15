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
        //�G�o���܂ł̎��Ԃ�z��Ɋi�[
        spawnTime = new float[enemyStatus.Length];
        for (int i = 0; i < spawnTime.Length; i++)
        {
            spawnTime[i] = enemyStatus[i].spawnTime;
        }

        //�G�̃��[�g��z��Ɋi�[
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

        //�G�̃X�e�[�^�X��ݒ�
        GameObject enemy = enemyStatus[spawnIndex].prefab;
        Enemy_Base enemyBase = enemy.GetComponent<Enemy_Base>();

        //�ʒu�Ɗp�x��ݒ�
        enemyBase.spawnPoint = this;
        Vector3 spawnPos = new Vector3(transform.position.x, enemyStatus[spawnIndex].prefab.transform.position.y, transform.position.z);
        Quaternion targetDir = Quaternion.LookRotation(routePoint[0] - spawnPos);
        Quaternion spawnDir = new Quaternion(enemy.transform.rotation.x, targetDir.y, enemy.transform.rotation.z, targetDir.w);

        //�p�����[�^��ݒ�
        enemyBase.maxHp = enemyStatus[spawnIndex].hp;
        enemyBase.hp = enemyBase.maxHp;
        enemyBase.value = enemyStatus[spawnIndex].value;
        enemyBase.interval = enemyStatus[spawnIndex].interval;
        enemyBase.distance = enemyStatus[spawnIndex].distance;
        enemyBase.range = enemyStatus[spawnIndex].range;
        enemyBase.moveSpeed = enemyStatus[spawnIndex].moveSpeed;


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
    }
}

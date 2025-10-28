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

    [SerializeField] GameObject spawnPoint; //�G�̏o���ʒu;

    void Start()
    {
        //�G�̏���z��Ɋi�[
        spawnTime = new float[enemyStatus.Length];
        for (int i = 0; i < enemyStatus.Length; i++)
        {
            //�G��Prefab��EnemiesData����擾
            enemyStatus[i].prefab = EnemiesData.Instance.enemy[enemyStatus[i].id].prefab;

            //�o�����Ԃ�z��Ɋi�[
            spawnTime[i] = enemyStatus[i].spawnTime;

            //�G�̑��o�������J�E���g
            BattleManager.Instance.nowEnemyNum += enemyStatus[i].spawnNum;
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
        if (!BattleManager.Instance.isMainGame) return; //���C���Q�[�����łȂ���Ζ߂�

        if (!endSpawn && BattleManager.Instance.timer_EnemySpawn > spawnTime[spawnIndex]) StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if (endSpawn) yield break;

        int index = spawnIndex;
        spawnIndex++;
        if (spawnIndex >= enemyStatus.Length) endSpawn = true;

        //�G�̃X�e�[�^�X��ݒ�
        Enemy_Base enemy = enemyStatus[index].prefab.GetComponent<Enemy_Base>();

        //�ʒu�Ɗp�x��ݒ�
        enemy.spawnPoint = this;
        Quaternion targetDir = Quaternion.LookRotation(routePoint[enemyStatus[index].routeIndex].pos[0] - spawnPos);
        Quaternion spawnDir = new Quaternion(enemy.transform.rotation.x, targetDir.y, enemy.transform.rotation.z, targetDir.w);

        //�p�����[�^��ݒ�
        enemy.maxHp = enemyStatus[index].hp;
        enemy.hp = enemy.maxHp;
        enemy.value = enemyStatus[index].value;
        enemy.defaultValue = enemy.value;
        enemy.interval = enemyStatus[index].interval;
        enemy.distance = enemyStatus[index].distance;
        enemy.range = enemyStatus[index].range;
        enemy.moveSpeed = enemyStatus[index].moveSpeed;
        enemy.knockBackTime = enemyStatus[index].knockBackTime;

        enemy.routeIndex = enemyStatus[index].routeIndex;

        for (int i = 0; i < enemyStatus[index].spawnNum; i++)
        {
            //�G�̃C���X�^���X�𐶐�
            Enemy_Base instance = Instantiate(enemy);
            instance.transform.position = spawnPos;
            instance.transform.rotation = spawnDir;

            //HP�o�[�𐶐�
            Hpbar hpbar = Instantiate(BattleManager.Instance.hpbarPrefab).GetComponent<Hpbar>();
            hpbar.transform.SetParent(BattleManager.Instance.hpbarParent.transform);
            hpbar.transform.localScale = new Vector3(1f, 1f, 1f);
            hpbar.transform.localRotation = new Quaternion();
            hpbar.targetEnemy = instance;
            instance.hpbarObj = hpbar.gameObject;

            //��莞�ԑ҂�
            yield return new WaitForSeconds(enemyStatus[index].spawnInterval);
        }
    }

    //�X�e�[�^�X�̍\����
    [System.Serializable]
    public struct Status
    {
        [SerializeField, Label("�G��ID")] public int id;
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

        [System.NonSerialized] public GameObject prefab; //�G��Prefab
    }

    //�G�̃��[�g���i�[����\����
    [System.Serializable]
    public struct Route
    {
        public GameObject[] pointObj;
        [System.NonSerialized] public Vector3[] pos;
    }
}

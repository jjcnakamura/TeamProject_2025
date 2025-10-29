using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public Route[] routePoint;      //�G���i�ޓ�
    Vector3 spawnPos;               //�G�̏o���ʒu

    GameObject[] routeLineParent;   //�G�̃��[�g�̕\��
    float blockScale = 2f;          //�X�e�[�W��̂P�u���b�N�̑傫��
    float routeArrowOffsetY = 0.5f; //�G�̃��[�g�\�����̖��̍���

    [Space(20)]

    [SerializeField] Status[] enemyStatus; //��������G�̃X�e�[�^�X

    float[] spawnTime; //�G�̏o������
    int spawnIndex;    //���ɏo������G

    bool endSpawn;     //�S�Ă̓G���o��������

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
            //�o�����Ԃ��烋�[�g�\�����Ԃ�������������z��Ɋi�[
            spawnTime[i] = Mathf.Max(enemyStatus[i].spawnTime - BattleManager.Instance.preEnemySpawnTime, 0);
            //�G�̑��o�������J�E���g
            BattleManager.Instance.nowEnemyNum += enemyStatus[i].spawnNum;
        }

        //�G�̃��[�g�̕\�������m��
        routeLineParent = new GameObject[enemyStatus.Length];

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

    //�G�𐶐�����
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

        //�G�̃��[�g���w�肵���b���\��
        GenerateRouteLine(index);
        for (int i = 0; i < BattleManager.Instance.enemyRouteActiveTime; i++)
        {
            routeLineParent[index].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            routeLineParent[index].SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        GenerateRouteLine(index, true);
        yield return new WaitForSeconds(BattleManager.Instance.preEnemySpawnTime - BattleManager.Instance.enemyRouteActiveTime);

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
    
    //�G�̃��[�g��\������
    void GenerateRouteLine(int index, bool destroy = false)
    {
        //���[�g�̕\�����폜����ꍇ�͂��̂܂ܖ߂�
        if (destroy && routeLineParent[index] != null)
        {
            Destroy(routeLineParent[index]);
            return;
        }
        //�������[�g��\������ꍇ�͌��X�\������Ă������̂��폜����
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

            //���Ɉړ�����ꍇ
            if (Mathf.Abs(posX - routePoint[enemyStatus[index].routeIndex].pos[i + 1].x) >= tolerance)
            {
                //����������I�u�W�F�N�g�̐�������
                int brockNum = (int)(Mathf.Abs(posX - routePoint[enemyStatus[index].routeIndex].pos[i + 1].x) /blockScale);
                int arrowNum = (i >= 0) ? brockNum : brockNum - 1;
                //���̃��[�g���v���X�������}�C�i�X������
                int dir = (posX < routePoint[enemyStatus[index].routeIndex].pos[i + 1].x) ? 1 : -1;
                float arrowPosX = posX;
                if (i < 0) arrowPosX += (blockScale * dir);
                //�u���b�N���Ƃɖ��𐶐�����
                for (int j = 0; j < arrowNum; j++)
                {
                    //���𐶐����Ĉʒu�A�p�x�A�傫���𒲐�
                    GameObject arrow = Instantiate(BattleManager.Instance.enemyRouteArrow);
                    arrow.transform.position = new Vector3(arrowPosX, transform.position.y + routeArrowOffsetY, posZ);
                    arrow.transform.Rotate(0, (dir < 0) ? 270f : 90f, 0);
                    arrow.transform.localScale = BattleManager.Instance.enemyRouteArrow.transform.localScale;
                    arrow.transform.SetParent(routeLineParent[index].transform);
                    //���̐����ʒu������
                    arrowPosX += (blockScale * dir);
                }
            }
            //�c�Ɉړ�����ꍇ
            else if (Mathf.Abs(posZ - routePoint[enemyStatus[index].routeIndex].pos[i + 1].z) >= tolerance)
            {
                //����������I�u�W�F�N�g�̐�������
                int brockNum = (int)(Mathf.Abs(posZ - routePoint[enemyStatus[index].routeIndex].pos[i + 1].z) / blockScale);
                int arrowNum = (i >= 0) ? brockNum : brockNum - 1;
                //���̃��[�g���v���X�������}�C�i�X������
                int dir = (posZ < routePoint[enemyStatus[index].routeIndex].pos[i + 1].z) ? 1 : -1;
                float arrowPosZ = posZ;
                if (i < 0) arrowPosZ += (blockScale * dir);
                //�u���b�N���Ƃɖ��𐶐�����
                for (int j = 0; j < arrowNum; j++)
                {
                    //���𐶐����Ĉʒu�A�p�x�A�傫���𒲐�
                    GameObject arrow = Instantiate(BattleManager.Instance.enemyRouteArrow);
                    arrow.transform.position = new Vector3(posX, transform.position.y + routeArrowOffsetY, arrowPosZ);
                    arrow.transform.Rotate(0, (dir < 0) ? 180f : 0f, 0);
                    arrow.transform.localScale = BattleManager.Instance.enemyRouteArrow.transform.localScale;
                    arrow.transform.SetParent(routeLineParent[index].transform);
                    //���̐����ʒu������
                    arrowPosZ += (blockScale * dir);
                }
            }
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

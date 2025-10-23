using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �S�Ă̓G���퓬���Ɏ��p�����[�^�[
/// </summary>
public class Enemy_Base : MonoBehaviour
{
    [Header("Enemy_Base")]

    //Collider
    public BoxCollider col_Body;

    [Space(10)]

    //�Q�[�����̃p�����[�^�[
    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;
    public float moveSpeed;
    public float knockBackTime;

    //�^�C�}�[
    float timer_KnockBack;

    //���ɐi�ޏꏊ�Ɋւ���ϐ�
    public EnemySpawnPoint spawnPoint;
    public int routeIndex;
    public int currentRoute = 0;

    //���������Ɋւ���ϐ�
    float rotateSpeed = 8f;
    Quaternion dir;

    //��Ԃ�\���t���O
    public bool isMove, isRotation, isTarget, isKnockBack, isStatusChange, isDead;

    void Start()
    {
        isMove = true;
    }

    protected virtual void FixedUpdate()
    {
        KnockBack();
        DeadCheck();
        Move();
    }

    //�_���[�W
    public bool Damage(int damage)
    {
        if (isDead) return true;

        hp = Mathf.Max(hp - damage, 0);
        timer_KnockBack = 0;
        isKnockBack = true;

        //HP��0�ɂȂ����ꍇ���S
        isDead = (hp <= 0);
        if (isDead)
        {
            col_Body.gameObject.SetActive(false);
            isMove = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    void KnockBack()
    {
        if (!isKnockBack) return;

        if (timer_KnockBack < knockBackTime)
        {
            timer_KnockBack += Time.fixedDeltaTime;

            //���̃_���[�W���[�V����
            transform.eulerAngles += new Vector3(0, 1000 * Time.fixedDeltaTime, 0);
        }
        else
        {
            //���̃_���[�W���[�V����
            Quaternion targetDir = Quaternion.LookRotation(spawnPoint.routePoint[routeIndex].pos[currentRoute] - transform.position);
            Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
            DirectionChange(lookDir);

            timer_KnockBack = 0;
            isKnockBack = false;
        }
    }
    //���S���Ă���ꍇ�͎��g���폜����
    void DeadCheck()
    {
        if (isDead && !isKnockBack)
        {
            BattleManager.Instance.nowEnemyNum--;
            BattleManager.Instance.text_EnemyNum.text = BattleManager.Instance.nowEnemyNum.ToString();
            Destroy(gameObject);
        }
    }
    //�v���C���[�̐w�n�Ɍ������Ĉړ����鏈��
    void Move()
    {
        if (isMove && !isKnockBack)
        {
            Vector3 targetPos = new Vector3(spawnPoint.routePoint[routeIndex].pos[currentRoute].x, transform.position.y, spawnPoint.routePoint[routeIndex].pos[currentRoute].z);

            if (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //���̃��[�g�|�C���g��
                if (currentRoute < spawnPoint.routePoint[routeIndex].pos.Length - 1)
                {
                    currentRoute++;
                    //������ύX
                    if (!isTarget)
                    {
                        Quaternion targetDir = Quaternion.LookRotation(spawnPoint.routePoint[routeIndex].pos[currentRoute] - transform.position);
                        Quaternion lookDir = new Quaternion(transform.rotation.x, targetDir.y, transform.rotation.z, targetDir.w);
                        DirectionChange(lookDir);
                    }
                }
                //�v���C���[�̐w�n�ɓ��B�����ꍇ
                else
                {
                    //�v���C���[�Ƀ_���[�W��^���ď���
                    BattleManager.Instance.Damage();
                    BattleManager.Instance.nowEnemyNum--;
                    BattleManager.Instance.text_EnemyNum.text = BattleManager.Instance.nowEnemyNum.ToString();
                    Destroy(gameObject);
                }
            }
        }

        //������ύX
        if (isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, dir, rotateSpeed);
        }
    }
    /// <summary>
    /// ���Ɍ���������w��@��Q������true�ɂ���Ƃ��̈ʒu��D�悵�Č���
    /// </summary>
    public void DirectionChange(Quaternion targetDir, bool targetChange = false)
    {
        dir = targetDir;
        if (targetChange) isTarget = true;

        isRotation = true;
    }
}

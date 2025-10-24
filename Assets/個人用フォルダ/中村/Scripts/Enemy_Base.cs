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
    public int defaultValue;
    public int value;
    public float interval;
    public float distance;
    public float range;
    public float moveSpeed;
    public float knockBackTime;

    //���ɐi�ޏꏊ�Ɋւ���ϐ�
    public EnemySpawnPoint spawnPoint;
    public int routeIndex;
    public int currentRoute = 0;

    //���������Ɋւ���ϐ�
    float rotateSpeed = 8f;
    Quaternion dir;

    //�o�t�A�f�o�t�p�̕ϐ�
    List<int> buffValue = new List<int>();
    List<int> debuffValue = new List<int>();
    int maxBuffValue, minDebuffValue;
    int buffNum, deBuffNum;

    //�^�C�}�[
    float timer_KnockBack;

    //��Ԃ�\���t���O
    public bool isMove, isRotation, isTarget, isKnockBack, isBuff, isDebuff, isDeadCheck, isDead;

    protected virtual void Start()
    {
        isMove = true;
        isDeadCheck = true;
    }

    protected virtual void Update()
    {
        if (!BattleManager.Instance.isMainGame) return; //���C���Q�[�����łȂ���Ζ߂�

        DeadCheck();
    }

    protected virtual void FixedUpdate()
    {
        if (!BattleManager.Instance.isMainGame) return; //���C���Q�[�����łȂ���Ζ߂�

        KnockBack();
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
    //�X�e�[�^�X�Ƀo�t���f�o�t��������i�����ɂ��������ꍇ�͒l���傫������D��j
    public void StatusChange(int val, bool flag)
    {
        //�o�t
        if (val > 0)
        {
            //�o�t��������
            if (flag)
            {
                //�o�t����ǉ�
                buffNum++;
                buffValue.Add(val);

                //���݂̃o�t�ʂ��傫���ꍇ
                if (val > maxBuffValue)
                {
                    maxBuffValue = val;
                    value = Mathf.Max(defaultValue + val + minDebuffValue, 1);
                }

                isBuff = true;
            }
            //�o�t����������
            else if (buffNum > 0)
            {
                //���݂̃o�t�ʂƓ������ꍇ�͐V�����Q�Ƃ���o�t�ʂ����߂�
                bool isMax = (val >= maxBuffValue);
                bool isRemove = false;
                int maxVal = 0;

                //�o�t�������炷
                buffNum--;
                for (int i = buffValue.Count - 1; i >= 0; i--)
                {
                    if (isMax && buffValue[i] <= maxVal) maxVal = buffValue[i];

                    if (buffValue[i] == val)
                    {
                        if (!isRemove && isMax)
                        {
                            buffValue.RemoveAt(i);
                            isRemove = true;
                        }
                        else if (!isMax)
                        {
                            buffValue.RemoveAt(i);
                            break;
                        }
                    }
                }
                //�V�����Q�Ƃ���o�t�ʂ����߂�
                if (isMax)
                {
                    maxBuffValue = maxVal;
                    value = Mathf.Max(value + val, 1);
                }
                //�o�t����0�ɂȂ����ꍇ�̓X�e�[�^�X��߂�
                if (buffNum <= 0)
                {
                    maxBuffValue = 0;
                    value = Mathf.Max(defaultValue + val + minDebuffValue, 1);

                    isBuff = false;
                }
            }
        }
        //�f�o�t
        if (val < 0)
        {
            //�f�o�t��������
            if (flag)
            {
                //�f�o�t����ǉ�
                deBuffNum++;
                debuffValue.Add(val);

                //���݂̃f�o�t�ʂ��傫���ꍇ
                if (val < minDebuffValue)
                {
                    minDebuffValue = val;
                    value = Mathf.Max(defaultValue + val + maxBuffValue, 1);
                }

                isDebuff = true;
            }
            //�f�o�t����������
            else if (deBuffNum > 0)
            {
                //���݂̃f�o�t�ʂƓ������ꍇ�͐V�����Q�Ƃ���f�o�t�ʂ����߂�
                bool isMin = (val <= minDebuffValue);
                bool isRemove = false;
                int minVal = 0;

                //�f�o�t�������炷
                deBuffNum--;
                for (int i = debuffValue.Count - 1; i >= 0; i--)
                {
                    if (isMin && debuffValue[i] <= minVal) minVal = debuffValue[i];

                    if (debuffValue[i] == val)
                    {
                        if (!isRemove && isMin)
                        {
                            debuffValue.RemoveAt(i);
                            isRemove = true;
                        }
                        else if (!isMin)
                        {
                            debuffValue.RemoveAt(i);
                            break;
                        }
                    }
                }
                //�V�����Q�Ƃ���f�o�t�ʂ����߂�
                if (isMin)
                {
                    minDebuffValue = minVal;
                    value = Mathf.Max(defaultValue + val + maxBuffValue, 1);
                }
                //�f�o�t����0�ɂȂ����ꍇ�̓X�e�[�^�X��߂�
                if (deBuffNum <= 0)
                {
                    minDebuffValue = 0;
                    value = defaultValue + maxBuffValue;

                    isDebuff = false;
                }
            }
        }
    }
    //���S���Ă���ꍇ�͎��g���폜����
    void DeadCheck()
    {
        if (!isDeadCheck) return;

        if (isDead && !isKnockBack)
        {
            isDeadCheck = false;

            BattleManager.Instance.nowEnemyNum = Mathf.Max(BattleManager.Instance.nowEnemyNum - 1, 0);
            BattleManager.Instance.text_EnemyNum.text = BattleManager.Instance.nowEnemyNum.ToString();
            Destroy(gameObject);
        }
    }
    //�v���C���[�̐w�n�Ɍ������Ĉړ����鏈��
    void Move()
    {
        if (isMove && !isKnockBack && !isDead)
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
                    //�v���C���[�Ƀ_���[�W��^���Ď��S
                    BattleManager.Instance.Damage();
                    isDead = true;
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

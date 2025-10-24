using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �S���j�b�g���퓬���Ɏ��p�����[�^�[
/// </summary>
public class BattleUnit_Base : MonoBehaviour
{
    [Header("BattleUnit_Base")]

    //Collider
    public BoxCollider col_Body;

    [Space(10)]

    //�Q�[�����̃p�����[�^�[
    public int zoneIndex;  //�ǂ��ɔz�u����Ă��邩

    public int role;
    public int maxHp;
    public int hp;
    public int defaultValue;
    public int value;
    public float interval;
    public float distance;
    public float range;

    //���������Ɋւ���ϐ�
    float rotateSpeed = 8f;
    Quaternion dir;

    //�o�t�A�f�o�t�p�̕ϐ�
    List<int> buffValue = new List<int>();
    List<int> debuffValue = new List<int>();
    int maxBuffValue, minDebuffValue;
    int buffNum, deBuffNum;  

    //��Ԃ�\���t���O
    public bool isBattle, isRotation, isTarget, isBuff, isDebuff, isDead;

    protected virtual void Update()
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //�퓬���łȂ��ꍇ�͖߂�

        DeadCheck();
    }

    protected virtual void FixedUpdate()
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return; //�퓬���łȂ��ꍇ�͖߂�

        Rotate();
    }

    //�_���[�W
    public bool Damage(int damage)
    {
        if (!isBattle || !BattleManager.Instance.isMainGame) return false; //�퓬���łȂ��ꍇ�͖߂�
        if (isDead) return true;

        hp = Mathf.Max(hp - damage, 0);

        //HP��0�ɂȂ����ꍇ���S
        isDead = (hp <= 0);
        if (isDead)
        {
            col_Body.gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
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
        if (isDead)
        {
            BattleManager.Instance.OutUnit(zoneIndex);
        }
    }
    //�_���Ă���G�̕�������������
    void Rotate()
    {
        //������ύX
        if (isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, dir, rotateSpeed);
        }
    }

    /// <summary>
    /// �z�u����Ă��č폜�����ꍇ�̏���
    /// </summary>
    public void Out()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// ����������w��@��Q������true�ɂ���Ƃ��̈ʒu��D�悵�Č���
    /// </summary>
    public void DirectionChange(Quaternion targetDir, bool targetChange = false)
    {
        dir = targetDir;
        if (targetChange)isTarget = true;

        isRotation = true;
    }
}

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
    public int value;
    public float interval;
    public float distance;
    public float range;

    //���������Ɋւ���ϐ�
    float rotateSpeed = 8f;
    Quaternion dir;

    //��Ԃ�\���t���O
    public bool isBattle, isRotation, isTarget, isStatusChange, isDead;

    protected virtual void FixedUpdate()
    {
        if (!isBattle) return; //�퓬���łȂ��ꍇ�͖߂�

        DeadCheck();
        Rotate();
    }

    //�_���[�W
    public bool Damage(int damage)
    {
        if (!isBattle) return false; //�퓬���łȂ��ꍇ�͖߂�
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

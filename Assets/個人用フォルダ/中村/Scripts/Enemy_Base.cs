using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    //�Q�[�����̃p�����[�^�[
    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;
    public float moveSpeed;

    //���ɐi�ޏꏊ�Ɋւ���ϐ�
    public EnemySpawnPoint spawnPoint;
    public int routeIndex;
    int currentRoute = 0;

    //���������Ɋւ���ϐ�
    float rotateSpeed = 8f;
    Quaternion dir;

    //��Ԃ�\���t���O
    bool isMove = true, isRotation, isTarget;

    /// <summary>
    /// �_���[�W���󂯂鏈���@�����Ń_���[�W�ʂ��w��
    /// </summary>
    public void Damage(int damage)
    {

    }

    void FixedUpdate()
    {
        //�v���C���[�̐w�n�Ɍ������Ĉړ����鏈��
        if (isMove)
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
                    Destroy(gameObject);
                }
            }
        }

        //������ύX�i�����Ȃ��j
        if (isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, dir, rotateSpeed);
        }
    }

    /// <summary>
    /// ���Ɍ����񍐂��w��@��Q������true�ɂ���Ƃ��̈ʒu��D�悵�Č���
    /// </summary>
    public void DirectionChange(Quaternion targetDir, bool targetChange = false)
    {
        dir = targetDir;
        isTarget = targetChange;

        isRotation = true;
    }
}

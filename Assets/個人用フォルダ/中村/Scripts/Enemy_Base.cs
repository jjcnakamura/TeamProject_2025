using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;
    public float moveSpeed;

    public EnemySpawnPoint spawnPoint;
    int routeIndex = 0;

    bool isMove = true, isTarget;

    void FixedUpdate()
    {
        //�v���C���[�̐w�n�Ɍ������Ĉړ����鏈��
        if (isMove)
        {
            Vector3 nextPos = new Vector3(spawnPoint.routePoint[routeIndex].x, transform.position.y, spawnPoint.routePoint[routeIndex].z);

            if (transform.position != nextPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //���̃��[�g�|�C���g��
                if (routeIndex < spawnPoint.routePoint.Length - 1)
                {
                    routeIndex++;
                    //������ύX
                    if (!isTarget)
                    {

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
    }
}

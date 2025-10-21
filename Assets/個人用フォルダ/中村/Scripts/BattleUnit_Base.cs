using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_Base : MonoBehaviour
{
    [Header("BattleUnit_Base")]

    //Collider
    public BoxCollider col_Body;

    [Space(10)]

    //�Q�[�����̃p�����[�^�[
    public int zoneIndex;  //�ǂ��ɔz�u����Ă��邩

    public int role;
    public int cost;
    public int recast;

    public int maxHp;
    public int hp;
    public int value;
    public float interval;
    public float distance;
    public float range;

    //�^�C�}�[
    float timer_Recast;

    //��Ԃ�\���t���O
    public bool isBattle;

    //�_���[�W
    public void Damage(int damage)
    {
        if (!isBattle) return; //�퓬���łȂ��ꍇ�͖߂�


    }

    /// <summary>
    /// �z�u����Ă��č폜�����ꍇ�̏���
    /// </summary>
    public void Out()
    {
        Destroy(gameObject);
    }
}

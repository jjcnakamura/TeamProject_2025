using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit_Base : MonoBehaviour
{
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

    /// <summary>
    /// �z�u����Ă���ꍇ�ɍ폜����鏈��
    /// </summary>
    public void Out()
    {
        Destroy(gameObject);
    }
}

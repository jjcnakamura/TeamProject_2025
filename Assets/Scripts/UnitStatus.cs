/// <summary>
/// �Q�[�����ő�������e���j�b�g�̃p�����[�^
/// </summary>
[System.Serializable]
public class UnitStatus
{
    public int id;      //�ǂ̃��j�b�g��������ID
    public int role;    //���[���@0��DPS�A1���^���N�A2���T�|�[�g

    public int lv;      //���x��
    public int exp;     //�����o���l

    public int cost;    //�ݒu���̃R�X�g
    public int recast;  //�Ĕz�u�܂ł̎���

    public int hp;      //�ϋv�l�i�ő�HP�j
    public int value;   //DPS�̏ꍇ�͍U���́A�T�|�[�g�̏ꍇ�͉񕜗ʁA�|�C���g�����ʂȂ�
    public float interval; //�s�����x�i�U���A�񕜂�����Ԋu�j
    public float distance; //�U���A�񕜂̎˒�
    public float range;    //�͈͍U���͈̔�
}

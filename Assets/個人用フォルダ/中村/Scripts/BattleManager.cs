using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �퓬�V�[���̊Ǘ��p
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    public GameObject playerSide;

    //�v���C���[�̃p�����[�^�[�p�ϐ�
    int maxPlayerHp;
    [Space(10)]  public int playerHp;
    int maxPoint;
    public int point;

    //���j�b�g�̃p�����[�^�[�p�ϐ�
    BattleUnit_Base[] battleUnitStatus;         //�z�u����Ă���e���j�b�g�̃X�e�[�^�X
    [SerializeField] GameObject testUnit;       //���j�b�g�̔z�u�e�X�g�p
    [SerializeField] GameObject unitPullZone;   //���j�b�g�������Ă���{�^���̏W�܂�
    [SerializeField] PullUnit unitPullButton;   //���j�b�g�������Ă���{�^��
    Vector3 pullUnitSizeOffset = new Vector3(0.4f, 0.4f, 0.4f); //���j�b�g���������ꍇ�ɂ�����T�C�Y�␳
    GameObject dragUnit;                        //���݃h���b�O���Ă��郆�j�b�g
    int dragUnitIndex;                          //�h���b�O���Ă��郆�j�b�g�̗v�f�ԍ�
    [SerializeField] GameObject unitZoneParent; //���j�b�g�̔z�u�ꏊ�̐e�I�u�W�F�N�g
    UnitZone[] unitZone;                        //���j�b�g�̔z�u�ꏊ

    public float timer_EnemySpawn { get; private set; }

    public bool isMainGame, isClear, isGameOver, isUnitDrag, isOnMouseUnitZone;

    void Start()
    {
        //�t���O��ݒ�
        isMainGame = true;

        //�f�o�b�O�p�@�����L����3�̂����[�h
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(1);
        ParameterManager.Instance.AddUnit(2);

        //�v���C���[�̏����p�����[�^�[��ݒ�
        maxPlayerHp = ParameterManager.Instance.hp;
        playerHp = maxPlayerHp;
        maxPoint = ParameterManager.Instance.point;
        point = maxPoint;

        //���j�b�g�������Ă���{�^����UI��ɔz�u
        foreach (Transform n in unitPullZone.transform) Destroy(n.gameObject); //�S�Ă̎q�I�u�W�F�N�g���폜
        for (int i = 0; i < ParameterManager.Instance.unitStatus.Length; i++)
        {
            //�C���X�^���X�𐶐�
            PullUnit instance = Instantiate(unitPullButton);
            instance.transform.SetParent(unitPullZone.transform);

            //�T�C�Y������Ȃ��悤�ɒ���
            RectTransform rect = instance.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 0);
            instance.transform.rotation = new Quaternion();
            instance.transform.localScale = new Vector3(1, 1, 1);

            //�L���������蓖��
            instance.index = i;
        }

        //���j�b�g�̔z�u�ꏊ���擾
        unitZone = new UnitZone[unitZoneParent.transform.childCount];
        for (int i = 0; i < unitZone.Length; i++)
        {
            unitZone[i] = unitZoneParent.transform.GetChild(i).GetComponent<UnitZone>();
            unitZone[i].index = i;
        }
        //���j�b�g�̔z�u�ꏊ�̐��ɍ��킹�Ĕz�u�\���j�b�g��������
        battleUnitStatus = new BattleUnit_Base[unitZone.Length];
    }

    void Update()
    {
        //���j�b�g�h���b�O���̏���
        if (isUnitDrag)
        {
            dragUnit.transform.position = MouseManager.Instance.worldPos;

            if (Input.GetKeyUp(KeyCode.Mouse0) && !isOnMouseUnitZone) LetgoUnit();
        }
    }

    void FixedUpdate()
    {
        timer_EnemySpawn += Time.fixedDeltaTime;
    }

    //���j�b�g��I��
    public void PullUnit(int unitIndex)
    {
        isUnitDrag = true;

        dragUnitIndex = unitIndex;
        dragUnit = Instantiate(testUnit);
        dragUnit.transform.localScale -= pullUnitSizeOffset;
        dragUnit.transform.rotation = new Quaternion(0, 180f, 0, 0);
    }
    //�h���b�O���Ă��郆�j�b�g�𗣂�
    public void LetgoUnit()
    {
        Destroy(dragUnit);
        isUnitDrag = false;
    }
    //���j�b�g��z�u����
    public void PlaceUnit(int zoneIndex)
    {
        dragUnit.transform.localScale += pullUnitSizeOffset;
        dragUnit.transform.position = unitZone[zoneIndex].unitPoint;

        battleUnitStatus[zoneIndex] = dragUnit.GetComponent<BattleUnit_Base>();

        isUnitDrag = false;
    }
    //�z�u����Ă��郆�j�b�g���폜
    public void OutUnit(int zoneIndex)
    {
        if (!unitZone[zoneIndex].placed) return;

        battleUnitStatus[zoneIndex].Out();
    }

    public void Damage()
    {
        if (!isMainGame) return;

        playerHp--;
        Debug.Log("�_���[�W");

        if (playerHp < 0)
        {
            isMainGame = false;
            isGameOver = true;

            Debug.Log("�Q�[���I�[�o�[");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �퓬�V�[���̊Ǘ��p
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    //�eCanvas
    [SerializeField] GameObject canvasParent;
    GameObject[] canvas;

    //�v���C���[�̃p�����[�^�[�p�ϐ�
    public GameObject playerSide;
    int maxPlayerHp;
    public int playerHp;
    int maxPoint;
    public int point;

    //���j�b�g�̃p�����[�^�[�p�ϐ�
    BattleUnit_Base[] battleUnitStatus; //�z�u����Ă���e���j�b�g�̃X�e�[�^�X
    GameObject[] battleUnitPrefab;                              //�{�^�����琶������郆�j�b�g��Prefab
    [Space(10)] [SerializeField] GameObject unitPullZone;       //���j�b�g�������Ă���{�^���̏W�܂�
    [SerializeField] PullUnit unitPullButton;                   //���j�b�g�������Ă���{�^��
    Vector3 pullUnitSizeOffset = new Vector3(0.4f, 0.4f, 0.4f); //���j�b�g���������ꍇ�ɂ�����T�C�Y�␳
    GameObject dragUnit;                        //���݃h���b�O���Ă��郆�j�b�g
    int dragUnitIndex;                          //�h���b�O���Ă��郆�j�b�g�̗v�f�ԍ�
    [SerializeField] GameObject unitZoneParent; //���j�b�g�̔z�u�ꏊ�̐e�I�u�W�F�N�g
    [SerializeField] GameObject floorParent;    //�G���ʂ铹�̐e�I�u�W�F�N�g
    UnitZone[] unitZone;                        //���j�b�g�̔z�u�ꏊ
    //���݃h���b�O���Ă��郆�j�b�g���ǂ��ɔz�u�ł��邩
    public bool place_UnitZone { get; private set; }
    public bool place_Floor { get; private set; }

    //�G�o���̎��Ԃ��J�E���g����^�C�}�[
    public float timer_EnemySpawn { get; private set; }

    //�Q�[���̏�Ԃ�\���t���O
    public bool isMainGame, isClear, isGameOver, isUnitDrag, isOnMouseUnitZone;

    void Start()
    {
        //Canvas�̕\��
        canvas = new GameObject[canvasParent.transform.childCount];
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i] = canvasParent.transform.GetChild(i).gameObject;
            canvas[i].SetActive(i == 0);
        }

        //�t���O��ݒ�
        isMainGame = true;

        //�f�o�b�O�p�@�L���������[�h
        ParameterManager.Instance.maxUnitPossession = 5;
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(0);

        //�v���C���[�̏����p�����[�^�[��ݒ�
        maxPlayerHp = ParameterManager.Instance.hp;
        playerHp = maxPlayerHp;
        maxPoint = ParameterManager.Instance.point;
        point = maxPoint;

        //�g�p���郆�j�b�g��Prefab��ǂݍ���
        battleUnitPrefab = new GameObject[ParameterManager.Instance.unitStatus.Length];
        GameObject[] loadUnits = Resources.LoadAll<GameObject>("Units");
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            battleUnitPrefab[i] = loadUnits[ParameterManager.Instance.unitStatus[i].id];
        }

        //���j�b�g�������Ă���{�^����UI��ɔz�u
        foreach (Transform n in unitPullZone.transform) Destroy(n.gameObject); //�S�Ă̎q�I�u�W�F�N�g���폜
        for (int i = 0; i < battleUnitPrefab.Length; i++)
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
        int unitZoneNum = unitZoneParent.transform.childCount;
        unitZone = new UnitZone[unitZoneNum + floorParent.transform.childCount];
        for (int i = 0; i < unitZoneNum; i++)
        {
            unitZone[i] = unitZoneParent.transform.GetChild(i).GetComponent<UnitZone>();
            unitZone[i].index = i;
            unitZone[i].unitZone = true;
        }
        for (int i = unitZoneNum; i < unitZone.Length; i++)
        {
            unitZone[i] = floorParent.transform.GetChild(i - unitZoneNum).GetComponent<UnitZone>();
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
        dragUnit = Instantiate(battleUnitPrefab[unitIndex]);
        dragUnit.transform.localScale -= pullUnitSizeOffset;
        dragUnit.transform.rotation = new Quaternion(0, 180f, 0, 0);

        //�ǂ��ɔz�u�o���邩�i���j
        place_UnitZone = true;
        place_Floor = false;
    }
    //�h���b�O���Ă��郆�j�b�g�𗣂�
    public void LetgoUnit()
    {
        Destroy(dragUnit);
        isUnitDrag = false;

        place_UnitZone = false;
        place_Floor = false;
    }
    //���j�b�g��z�u����
    public void PlaceUnit(int zoneIndex)
    {
        dragUnit.transform.localScale += pullUnitSizeOffset;
        dragUnit.transform.position = unitZone[zoneIndex].unitPoint;

        int unitIndex = dragUnitIndex;

        //�X�e�[�^�X��ǂݍ���
        battleUnitStatus[zoneIndex] = dragUnit.GetComponent<BattleUnit_Base>();
        battleUnitStatus[zoneIndex].zoneIndex = zoneIndex;
        battleUnitStatus[zoneIndex].role = ParameterManager.Instance.unitStatus[unitIndex].role;
        battleUnitStatus[zoneIndex].cost = ParameterManager.Instance.unitStatus[unitIndex].cost;
        battleUnitStatus[zoneIndex].recast = ParameterManager.Instance.unitStatus[unitIndex].recast;
        battleUnitStatus[zoneIndex].maxHp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].hp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].value = ParameterManager.Instance.unitStatus[unitIndex].value;
        battleUnitStatus[zoneIndex].interval = ParameterManager.Instance.unitStatus[unitIndex].interval;
        battleUnitStatus[zoneIndex].distance = ParameterManager.Instance.unitStatus[unitIndex].distance;
        battleUnitStatus[zoneIndex].range = ParameterManager.Instance.unitStatus[unitIndex].range;

        battleUnitStatus[zoneIndex].isBattle = true;

        place_UnitZone = false;
        place_Floor = false;
        isUnitDrag = false;
    }
    //�z�u����Ă��郆�j�b�g���폜
    public void OutUnit(int zoneIndex)
    {
        if (!unitZone[zoneIndex].placed) return;

        battleUnitStatus[zoneIndex].Out();
        battleUnitStatus[zoneIndex] = null;
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

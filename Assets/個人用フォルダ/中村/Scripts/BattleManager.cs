using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �퓬�V�[���̊Ǘ��p
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    //�eCanvas
    [SerializeField] GameObject canvasParent;
    GameObject[] canvas;

    [Space(10)]

    //�v���C���[�̃p�����[�^�[�p�ϐ�
    public GameObject playerSide;                //�v���C���[�̐w�n�i�^���[�ɓ�����ꏊ�j
    [SerializeField] TextMeshProUGUI text_Hp;    //�v���C���[�i�^���[�j��HP�p�e�L�X�g
    [SerializeField] TextMeshProUGUI text_Point; //�|�C���g�p�e�L�X�g
    public TextMeshProUGUI text_EnemyNum;        //���݂̓G�̐��p�e�L�X�g
    int maxPlayerHp;
    public int playerHp;
    int maxPoint;
    public int point;

    [Space(10)]

    public int nowEnemyNum; //���݂̓G�̐�

    int pointUpVal = 1;     //���Ԃő�������|�C���g��
    float pointUpTime = 1f; //�|�C���g�̎��ԑ����ɂ�����b��
    float timer_PointUp;    //�|�C���g�̎��ԑ����p�^�C�}�[

    [Space(10)]

    //���j�b�g�̃p�����[�^�[�p�ϐ�
    [SerializeField] GameObject unitPullButtonParent;           //���j�b�g�������Ă���{�^���̐e�I�u�W�F�N�g
    [SerializeField] PullUnit unitPullButton;                   //���j�b�g�������Ă���{�^��
    GameObject[] battleUnitPrefab;                              //�{�^�����琶������郆�j�b�g��Prefab
    Vector3 pullUnitSizeOffset = new Vector3(0.4f, 0.4f, 0.4f); //���j�b�g���������ꍇ�ɂ�����T�C�Y�␳
    GameObject dragUnit;                              //���݃h���b�O���Ă��郆�j�b�g
    int dragUnitIndex;                                //�h���b�O���Ă��郆�j�b�g�̗v�f�ԍ�
    [SerializeField] GameObject unitZoneParent;       //���j�b�g�̔z�u�ꏊ�̐e�I�u�W�F�N�g
    [SerializeField] GameObject floorParent;          //�G���ʂ铹�̐e�I�u�W�F�N�g
    UnitZone[] unitZone;                              //���j�b�g�̔z�u�ꏊ
    public bool place_UnitZone { get; private set; }  //���݃h���b�O���Ă��郆�j�b�g���ǂ��ɔz�u�ł��邩
    public bool place_Floor { get; private set; }     //���݃h���b�O���Ă��郆�j�b�g���ǂ��ɔz�u�ł��邩

    BattleUnit_Base[] battleUnitStatus;               //�z�u����Ă���e���j�b�g�̃X�e�[�^�X
    [System.NonSerialized] public int[] unitCost;     //�e���j�b�g�̃R�X�g
    [System.NonSerialized] public float[] unitRecast; //�e���j�b�g�̃��L���X�g

    //�G�o���̎��Ԃ��J�E���g����^�C�}�[
    public float timer_EnemySpawn { get; private set; }

    //�Q�[���̏�Ԃ�\���t���O
    public bool isMainGame, isClear, isGameOver, isUnitDrag, isUnitPlace, isOnMouseUnitZone;

    void Start()
    {
        //�f�o�b�O�p�@�L���������[�h
        ParameterManager.Instance.maxUnitPossession = 5;
        ParameterManager.Instance.AddUnit(0);
        ParameterManager.Instance.AddUnit(1);
        ParameterManager.Instance.AddUnit(2);
        ParameterManager.Instance.AddUnit(3);
        ParameterManager.Instance.AddUnit(4);

        //�v���C���[�̏����p�����[�^�[��ݒ�
        maxPlayerHp = ParameterManager.Instance.hp;
        playerHp = maxPlayerHp;
        text_Hp.text = playerHp.ToString();
        maxPoint = ParameterManager.Instance.point;
        point = maxPoint;
        text_Point.text = point.ToString();

        text_EnemyNum.text = nowEnemyNum.ToString();

        //�g�p���郆�j�b�g��Prefab��ǂݍ���
        battleUnitPrefab = new GameObject[ParameterManager.Instance.unitStatus.Length];
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            battleUnitPrefab[i] = ParameterManager.Instance.unitStatus[i].prefab;
        }

        //���j�b�g�������Ă���{�^����UI��ɔz�u
        foreach (Transform n in unitPullButtonParent.transform) Destroy(n.gameObject); //�S�Ă̎q�I�u�W�F�N�g���폜
        unitCost = new int[battleUnitPrefab.Length];
        unitRecast = new float[battleUnitPrefab.Length];
        for (int i = 0; i < battleUnitPrefab.Length; i++)
        {
            //�C���X�^���X�𐶐�
            PullUnit pullUnit = Instantiate(unitPullButton);
            pullUnit.transform.SetParent(unitPullButtonParent.transform);

            //�T�C�Y������Ȃ��悤�ɒ���
            RectTransform rect = pullUnit.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 0);
            pullUnit.transform.rotation = new Quaternion();
            pullUnit.transform.localScale = new Vector3(1, 1, 1);

            //�L����ID�A�R�X�g�A���L���X�g�����蓖��
            pullUnit.index = i;
            pullUnit.text_Cost.text = ParameterManager.Instance.unitStatus[i].cost.ToString();
            unitCost[i] = ParameterManager.Instance.unitStatus[i].cost;
            unitRecast[i] = ParameterManager.Instance.unitStatus[i].recast;
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

        //Canvas�̕\��
        canvas = new GameObject[canvasParent.transform.childCount];
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i] = canvasParent.transform.GetChild(i).gameObject;
            canvas[i].SetActive(i == 0);
        }

        //�t���O��ݒ�
        isMainGame = true;
    }

    void Update()
    {
        if (!isMainGame) return; //���C���Q�[�����łȂ���Ζ߂�

        ClearCheck(); //�G��S�ē|������N���A�ɂ���

        //���j�b�g�h���b�O���̏���
        if (isUnitDrag)
        {
            dragUnit.transform.position = MouseManager.Instance.worldPos;
            if (Input.GetKeyUp(KeyCode.Mouse0) && !isOnMouseUnitZone) LetgoUnit();
        }
    }

    void FixedUpdate()
    {
        if (!isMainGame) return; //���C���Q�[�����łȂ���Ζ߂�

        PointUp();                               //�|�C���g�̎��ԑ���
        timer_EnemySpawn += Time.fixedDeltaTime; //�G�̏o�����ԃJ�E���g
    }

    //���j�b�g��I��
    public void PullUnit(int unitIndex)
    {
        isUnitDrag = true;

        dragUnitIndex = unitIndex;
        dragUnit = Instantiate(battleUnitPrefab[unitIndex]);
        dragUnit.transform.localScale -= pullUnitSizeOffset;
        dragUnit.transform.rotation = new Quaternion(0, 180f, 0, 0);

        //�ǂ��ɔz�u�o���邩
        place_UnitZone = ParameterManager.Instance.unitStatus[unitIndex].place_UnitZone;
        place_Floor = ParameterManager.Instance.unitStatus[unitIndex].place_Floor;
    }
    //�h���b�O���Ă��郆�j�b�g�𗣂�
    public void LetgoUnit()
    {
        Destroy(dragUnit);
        
        place_UnitZone = false;
        place_Floor = false;
        isUnitDrag = false;
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
        battleUnitStatus[zoneIndex].maxHp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].hp = ParameterManager.Instance.unitStatus[unitIndex].hp;
        battleUnitStatus[zoneIndex].value = ParameterManager.Instance.unitStatus[unitIndex].value;
        battleUnitStatus[zoneIndex].interval = ParameterManager.Instance.unitStatus[unitIndex].interval;
        battleUnitStatus[zoneIndex].distance = ParameterManager.Instance.unitStatus[unitIndex].distance;
        battleUnitStatus[zoneIndex].range = ParameterManager.Instance.unitStatus[unitIndex].range;

        battleUnitStatus[zoneIndex].isBattle = true;

        //�R�X�g���̃|�C���g�����炷
        PointChange(-ParameterManager.Instance.unitStatus[unitIndex].cost);

        place_UnitZone = false;
        place_Floor = false;
        isUnitDrag = false;

        isUnitPlace = true;
    }
    //�|�C���g�̎��ԑ���
    void PointUp()
    {
        if (timer_PointUp < pointUpTime)
        {
            timer_PointUp += Time.fixedDeltaTime;
        }
        else
        {
            timer_PointUp = 0;
            PointChange(pointUpVal);
        }
    }
    //�G��S�ē|����������
    void ClearCheck()
    {
        if (isClear) return;

        //�G�̐���0�ɂȂ�����N���A
        if (nowEnemyNum <= 0) Clear();
    }
    //�X�e�[�W�N���A
    void Clear()
    {
        isMainGame = false;
        isClear = true;

        //�X�e�[�W�N���A��ʂ�\��
        canvas[1].SetActive(true);
    }
    //�Q�[���I�[�o�[
    void GameOver()
    {
        isMainGame = false;
        isGameOver = true;

        //�Q�[���I�[�o�[��ʂ�\��
        canvas[2].SetActive(true);
    }

    //�z�u����Ă��郆�j�b�g���폜
    public void OutUnit(int zoneIndex)
    {
        if (!unitZone[zoneIndex].placed) return;

        battleUnitStatus[zoneIndex].Out();
        battleUnitStatus[zoneIndex] = null;

        unitZone[zoneIndex].placed = false;
    }
    //�|�C���g�𑝌�����
    public void PointChange(int val)
    {
        point = Mathf.Min(Mathf.Max(point + val, 0), 999); //�ő�l��999
        text_Point.text = point.ToString();
    }
    //�v���C���[�i�^���[�j�ւ̃_���[�W
    public void Damage()
    {
        if (!isMainGame) return;

        playerHp = Mathf.Max(playerHp - 1, 0);
        text_Hp.text = playerHp.ToString();

        //HP��0�ɂȂ�����Q�[���I�[�o�[
        if (playerHp <= 0)
        {
            isMainGame = false;
            isGameOver = true;

            GameOver();
        }
    }
}

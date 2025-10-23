using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class MapManager : MonoBehaviour
{
    public ParameterManager.UnitStatus unitStatus;
    public GameObject[] MapStageImage;//�}�b�v�Ŏg���v���n�u�Ȃ�
    public GameObject[] MapRoute;//�}�b�v�̃��[�g
    public GameObject[] MapEnterButton;//�}�b�v�ɂ���G���^�[�{�^��
    public Transform nextStage;//���݂̃X�e�[�W��i�߂邽�߂̏ꏊ
    public Transform BossEnemy;//���̃t���A�̃{�X

    public int floor;// ���݂̃t���A��
    public int[] Stageint;//�X�e�[�W��
    public int max = 5;//�X�e�[�W�ő吔
    public int min = 2;//�X�e�[�W�ŏ���

    public int[] UnitUpGold;//�K�v�S�[���h��
    public int Gold;//(��)

    private void Start()
    {
        MakeRoute(); //��
    }
    void Update()
    {
        GoNextStage();
        DontDestroyOnLoad(gameObject);
    }

    public void GoNextStage() //���߂����[�g�̃X�e�[�W��i�߂鏈��
    {
        Transform child = nextStage.GetChild(0);
        StageInfo stageinfo = child.GetComponent<StageInfo>();
        if (nextStage != null)
        {
            if (stageinfo != null)
            {
                stageinfo.Start = true;
                Debug.Log("�����" + stageinfo.Stage + "��" + stageinfo.namber + "�ł�");
            }
            else
            {
                Debug.Log("�������Ȃ�");
            }
        }
    }
    public void OnButtonPressed(Transform Pos)//�{�^���ɓ����Ă��郋�[�g�Ō��߂�
    {
        if (Pos == null)
        {
            Debug.LogWarning("�e�̎Q�Ƃ��ݒ肳��Ă��܂���I");
            return;
        }

        if (nextStage.childCount == 0)
        {
            if(Pos.childCount == 0)
            {
                Transform Boss = BossEnemy.GetChild(0);
                Boss.SetParent(nextStage, true);
                Boss.localPosition = Vector3.zero;
            }
            Transform child = Pos.GetChild(0);
            child.SetParent(nextStage, true);
            child.localPosition = Vector3.zero;
        }
    }
    public void PassiveeEnterButton()//�}�b�v��Enter�{�^�����������߂̂�� ENTER�{�^���ɕt����p
    {
        if (MapEnterButton == null || MapEnterButton.Length == 0)
        {
            Debug.LogWarning("�z��ɃI�u�W�F�N�g���ݒ肳��Ă��܂���");
            return;
        }

        foreach (GameObject obj in MapEnterButton)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    public void SelectRoute(int x)//�I�񂾃��[�g�ȊO���p�b�V�u�ɂ�����́@ENTER�{�^���ɕt����p
    {
        // �z�񂪋�̏ꍇ�͉������Ȃ�
        if (MapRoute == null || MapRoute.Length == 0)
        {
            Debug.LogWarning("�I�u�W�F�N�g�z�񂪋�ł�");
            return;
        }

        // keepIndex ���͈͊O�Ȃ�C��
        if (x < 0 || x >= MapRoute.Length)
        {
            Debug.LogWarning("keepIndex ���͈͊O�ł�");
            return;
        }

        for (int i = 0; i < MapRoute.Length; i++)
        {
            if (i != x && MapRoute[i] != null)
            {
                MapRoute[i].SetActive(false);
            }
        }/*�ꉞ�S���̃��[�g���p�b�V�u�ɂ�����
        foreach (GameObject obj in MapRoute)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }*/
    }
    public void MakeRoute()//���[�g��̃X�e�[�W�����
    {
        if (Stageint == null || Stageint.Length == 0)
        {
            Stageint = new int[3];
        }

        for (int i = 0; i < Stageint.Length; i++)
        {
            int randomNum = Random.Range(min, max);
            Stageint[i] = randomNum;
            //Debug.Log($"�͈� {min}�`{max} �̒����� {randomNum} ���o��");
        }
        Debug.Log("�o�͌��ʈꗗ:");
        for (int i = 0; i < Stageint.Length; i++)
        {
            Debug.Log($"numbers[{i}] = {Stageint[i]}");
        }
        for (int i = 0; i < Stageint[0]; i++)
        {
            int Stage = Random.Range(0, 2);
            GameObject Route = Instantiate(MapStageImage[Stage], MapRoute[0].transform);
            Debug.Log($"�͈� 0�`2 �̒����� {Stage} ���o��");
        }

        for (int i = 0; i < Stageint[1]; i++)
        {
            int Stage = Random.Range(0, 2);
            GameObject Route = Instantiate(MapStageImage[Stage], MapRoute[1].transform);
        }

        for (int i = 0; i < Stageint[2]; i++)
        {
            int Stage = Random.Range(0, 2);
            GameObject Route = Instantiate(MapStageImage[Stage], MapRoute[2].transform);
        }
    }
    public void EventContDown()//�ݒu���̃R�X�g�@(�Z�k�n)
    {
        unitStatus.cost -= 1;//�i���j
    }
    public void EventrecastDown()//�Ĕz�u�܂ł̎��ԁ@(�Z�k�n)
    {
       unitStatus.recast -= 1;//�i���j
    }
    public void EventmaxInstallationUp()//���j�b�g�ő�z�u���@(�����n)
    {
        ParameterManager.Instance.maxInstallation += 1;//�i���j
    }
    public void EventmaxUnitPossessionUp()//�ő僆�j�b�g�������@(�����n)
    {
        ParameterManager.Instance.maxUnitPossession += 1;//�i���j
    }
    public void EventsameUnitMaxInstallationUp()//�������j�b�g�̍ő�z�u���@(�����n)
    {
        ParameterManager.Instance.sameUnitMaxInstallation += 1;//�i���j
    }
    public void Event()//HP�񕜂���i�񕜁j
    {
        ParameterManager.Instance.hp += 1;//�i���j
    }
    public void UnitLevelUpBottun()
    {
        if (Gold >= UnitUpGold[unitStatus.lv])
        {
            Gold -= UnitUpGold[unitStatus.lv];
            unitStatus.lv += 1;//���x��
            unitStatus.hp += 1;//�ϋv�l�i�ő�HP�j
            unitStatus.value += 1;//DPS�̏ꍇ�͍U���́A�T�|�[�g�̏ꍇ�͉񕜗ʁA�|�C���g�����ʂȂ�
            unitStatus.interval += 1;//�s�����x�i�U���A�񕜂�����Ԋu�j
            unitStatus.distance += 1;//�U���A�񕜂̎˒�
            unitStatus.range += 1;//�͈͍U���͈̔�
            //�����炷
        }
        if(Gold < UnitUpGold[unitStatus.lv])
        {
            //�����e�L�X�g���o���ďo���Ȃ����Ƃ����߂�
        }
    }
    public void LoadScene(int i)//�V�[���𗬂��p�@�{�^���p
    {
        SceneManager.LoadScene(i);
    }
}

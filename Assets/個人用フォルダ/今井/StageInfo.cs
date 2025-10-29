using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// �X�e�[�W�̏���ςރX�N���v�g
/// </summary>
public class StageInfo : MonoBehaviour
{
    [Header("�X�e�[�W���̏��")]
    public bool Start;
    public int Stage;//�o�g�����C�x���g��
    public int namber;//���̎��
    public string StageName;//�e�L�X�g�ŉ����s����
    public string StageNaiyou;//�X�e�[�W�ŉ������邩�C�x���g��p
    public GameObject[] image;
    public int Enemyint;//�G�̐�
    public TextMeshProUGUI[] StageInfoText;
    public bool StageEnd;
    public bool FloorEnd;

    void Update()
    {
        StageInfoText[0].text = StageName; //�������邩���e�L�X�g�Ŕ��f
        StageInfoText[1].text = StageNaiyou;//���e
        image[0].SetActive(Start);
        if (StageName == "�o�g��")//�o�g���X�e�[�W�̎��͕\��
        {
            StageInfoText[1].text = Enemyint.ToString();
        }
        if (StageEnd == true && FloorEnd == true)//��
        {

        }
        if (StageEnd == true)//�����̃X�e�[�W���I�����������鏈��
        {
            Destroy(gameObject);
        }
    }
}

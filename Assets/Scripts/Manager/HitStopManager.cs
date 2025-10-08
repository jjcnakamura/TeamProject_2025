using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : Singleton<HitStopManager>
{
    [SerializeField, TextArea(1, 5)] string manual;

    float elapsedTime = 0f;     //�q�b�g�X�g�b�v���̌o�ߎ���
    bool hitStopFLG;
    int settingnumber;

    //�q�b�g�X�g�b�v�̋�����\����(Structure)�Őݒ�
    [Header("�q�b�g�X�g�b�v�ݒ�")]
    public HitStopParameter[] hitStop;

    [System.Serializable]
    public struct HitStopParameter
    {
        [SerializeField, Label("ScaleTime")] public float timeScale;   //Time.timeScale�ɐݒ肷��l 
        [SerializeField, Label("���ʎ���")] public float slowTime;    //���Ԃ�x�����Ă��鎞��
    }

    void Start()
    {
    }

    void Update()
    {
        //GameManager��puase�t���O�������Ă��Ȃ����
        //if (!GameManager.Instance.pause) HitStopTime();
    }

    //�q�b�g�X�g�b�v������
    public void HitStopStart(int number)
    {
        settingnumber = number;
        elapsedTime = 0f;
        hitStopFLG = true;
        Time.timeScale = hitStop[settingnumber].timeScale;
    }

    //�q�b�g�X�g�b�v�����㎞�Ԍv��
    void HitStopTime()
    {
        if (hitStopFLG)
        {
            //���Ԍv��
            elapsedTime += Time.unscaledDeltaTime;

            //���Ԍo�߂Ō��̑����ɖ߂�
            if (elapsedTime >= hitStop[settingnumber].slowTime)
            {
                Time.timeScale = 1f;
                hitStopFLG = false;
            }
        }
    }
}

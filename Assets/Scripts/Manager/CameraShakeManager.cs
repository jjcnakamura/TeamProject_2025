/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;  //��Cinemachine���X�N���v�g�Ő��䂷��̂ɕK�v

public class CameraShakeManager : Singleton<CameraShakeManager>
{
    //�J�����̐U���𔭐�������R���|�[�l���g
    [Header("�U���̔�����")]
    [SerializeField] CinemachineImpulseSource impulseSource;

    //�U���̋�����\����(Structure)�Őݒ�
    //���\���̂Ƃ͕����̕ϐ����܂Ƃ߂ĊǗ��������
    [Header("�U���̋����ݒ�")]
    public ShakeParameter[] shake;

    [System.Serializable]
    public struct ShakeParameter
    {
        [SerializeField, Label("�U���̋���")] public float amplitudeGain;
        [SerializeField, Label("�U���ׂ̍���")] public float frequencyGain;
        [SerializeField, Label("�U������")] public float sustainTime;
    }

    //�J�����̐U���𔭐�������
    public void CameraImpulse(int number)
    {
        //true�ɂ����timescale�𖳎�����
        CinemachineImpulseManager.Instance.IgnoreTimeScale = true;

        //CinemachineImpulseSource�̊e�ݒ荀�ڂ̒l��
        //�\���̂Őݒ肵�Ă���l�ɕύX
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = shake[number].amplitudeGain;
        impulseSource.m_ImpulseDefinition.m_FrequencyGain = shake[number].frequencyGain;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = shake[number].sustainTime;

        //�U�������郁�\�b�h���Ăяo��
        //���̃��\�b�h�͌��XCinemachine�̃X�N���v�g�ɂ������
        impulseSource.GenerateImpulse();
    }
}
*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;  //←Cinemachineをスクリプトで制御するのに必要

public class CameraShakeManager : Singleton<CameraShakeManager>
{
    //カメラの振動を発生させるコンポーネント
    [Header("振動の発生源")]
    [SerializeField] CinemachineImpulseSource impulseSource;

    //振動の強弱を構造体(Structure)で設定
    //※構造体とは複数の変数をまとめて管理するもの
    [Header("振動の強さ設定")]
    public ShakeParameter[] shake;

    [System.Serializable]
    public struct ShakeParameter
    {
        [SerializeField, Label("振動の強さ")] public float amplitudeGain;
        [SerializeField, Label("振動の細かさ")] public float frequencyGain;
        [SerializeField, Label("振動時間")] public float sustainTime;
    }

    //カメラの振動を発生させる
    public void CameraImpulse(int number)
    {
        //trueにするとtimescaleを無視する
        CinemachineImpulseManager.Instance.IgnoreTimeScale = true;

        //CinemachineImpulseSourceの各設定項目の値を
        //構造体で設定している値に変更
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = shake[number].amplitudeGain;
        impulseSource.m_ImpulseDefinition.m_FrequencyGain = shake[number].frequencyGain;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = shake[number].sustainTime;

        //振動させるメソッドを呼び出す
        //このメソッドは元々Cinemachineのスクリプトにあるもの
        impulseSource.GenerateImpulse();
    }
}
*/

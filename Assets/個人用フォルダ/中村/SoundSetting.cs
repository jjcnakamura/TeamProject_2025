using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] GameObject[] inactiveObj;

    [Space(10)]

    [SerializeField] GameObject window;

    public Slider bgmVolSlider;
    public Slider seVolSlider;

    public TextMeshProUGUI text_BgmVol;
    public TextMeshProUGUI text_SeVol;

    //音量調整画面を開く
    public void SoundSettingWindow()
    {
        //音量調整画面を開く
        if (!window.activeSelf)
        {
            SoundManager.Instance.PlaySE_Sys(1);

            //保存された音量をロード
            int vol1 = PlayerPrefs.GetInt("BGMVol");
            int vol2 = PlayerPrefs.GetInt("SEVol");

            //SliderとTextの値を設定
            bgmVolSlider.value = vol1;
            seVolSlider.value = vol2;
            text_BgmVol.text = (vol1 * 10).ToString() + "%";
            text_SeVol.text = (vol2 * 10).ToString() + "%";

            //音量設定以外のオブジェクトを非表示に
            for (int i = 0; i < inactiveObj.Length; i++)
            {
                inactiveObj[i].SetActive(false);
            }

            window.SetActive(true);
        }
        //音量調整画面を閉じる
        else
        {
            SoundManager.Instance.PlaySE_Sys(2);

            //音量設定をセーブ
            PlayerPrefs.SetInt("BGMVol", (int)bgmVolSlider.value);
            PlayerPrefs.SetInt("SEVol", (int)seVolSlider.value);

            //音量設定以外のオブジェクトを再表示
            for (int i = 0; i < inactiveObj.Length; i++)
            {
                inactiveObj[i].SetActive(true);
            }

            window.SetActive(false);
        }
    }

    //SilderによるBGM音量の調整
    public void BgmVolumeChange()
    {
        text_BgmVol.text = SoundManager.Instance.BgmVolumeChange((int)bgmVolSlider.value);
    }

    //SilderによるSE音量の調整
    public void SeVolumeChange()
    {
        text_SeVol.text = SoundManager.Instance.SeVolumeChange((int)seVolSlider.value);
    }
}

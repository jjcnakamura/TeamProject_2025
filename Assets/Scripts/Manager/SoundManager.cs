using System.Collections;
using UnityEngine;
using UnityEngine.Audio;    //AudioMixerを使用するのに必要
using SaveData_Settings;    //自前で作ったSaveData_Settingsの使用に必要
using UnityEngine.UI;       //音量調節用のSliderを扱うのに必要
using TMPro;

public class SoundManager : Singleton<SoundManager>
{
    //--------------------------------------
    //ゲームに必要なサウンドを管理する
    //--------------------------------------

    //使用方法
    //このスクリプトでゲームに使用する全てのAudioを管理する
    //BGM、SystemSE、GameSEの3種類にAudioClipを分けているので、
    //追加したい場合は、エディターで配列の数を増やして追加する

    //ゲーム起動時、AudioClipごとにAudioSorceを自動的に追加している
    //AudioMixerでグループごとにまとめて音量を変更できる

    //カテゴリごとに音を再生するメソッドを分けているので、
    //音を鳴らしたいタイミングで各メソッドを呼び出してあげれば良い
    //例　PlaySE_Sys

    //注意
    //自前で用意したNameSpace「SaveData_Settings」を使い、
    //音量の保存、ロードを行っているので、そのスクリプトがないとエラーになる
    //SaveData_Settingsのスクリプトはプロジェクトに存在しているだけで良い


    [Header("音量コントロールのMixer")]
    [SerializeField] public AudioMixer mixer;   //音量をコントロールする
    [SerializeField, Label("Masterグループ")] public AudioMixerGroup MasterGroup;
    [SerializeField, Label("BGMグループ")] public AudioMixerGroup BGMGroup;
    [SerializeField, Label("SEグループ")] public AudioMixerGroup SEGroup;
    [Space(10)]
    //各カテゴリごとにAudioClipを入れる変数を配列で用意
    public AudioClip[] bgmClip;
    public AudioClip[] bgm_BattleClip;
    public AudioClip[] se_SysClip;
    public AudioClip[] se_GameClip;
    public AudioClip[] se_JingleClip;

    //各音声用のAudioSourceを用意する
    [System.NonSerialized] public AudioSource BGMSource;
    [System.NonSerialized] public AudioSource SE_SysSource;
    [System.NonSerialized] public AudioSource[] SE_GameSource;
    [System.NonSerialized] public AudioSource[] SE_JingleSource;

    //音量の段階（SliderのValueで設定）
    float[] vol_BGM = {-80f,-30f,-27,-24f,-21f,-18f,-15f,-12.5f,-10f,-7.5f,-5f };
    float[] vol_SE  = {-80f, -14f, -12f, -9f, -7f, -5f, -3f, -1f, 1f, 3f, 5f };

    //フェード用に音量を保持
    float bgmVol;

    //プレイヤーの音量調節の操作用変数
    public Slider bgmVolSlider;
    public Slider seVolSlider;
    public TextMeshProUGUI bgmVolText;
    public TextMeshProUGUI seVolText;


    //シーン開始直後（Startメソッドより早く）に処理
    void Awake()
    {
        Load.Audio(); //保存された音量をロード

        //AddComponentでAudioSourceを追加、ループ設定、優先度、MixerGroupの設定
        //BGM
        BGMSource = gameObject.AddComponent<AudioSource>();
        BGMSource.loop = true;
        BGMSource.priority = 0;
        BGMSource.outputAudioMixerGroup = BGMGroup;

        //基本的に複数のSystem用SEが同時になることはないため
        //System用のSEを鳴らす処理ではAudioSourceは１つだけ
        SE_SysSource = gameObject.AddComponent<AudioSource>();
        SE_SysSource.loop = false;
        SE_SysSource.priority = 1;
        SE_SysSource.playOnAwake = false;
        SE_SysSource.outputAudioMixerGroup = SEGroup;

        //メインのゲーム中に使用するSEは複数の音が同時に鳴ることが多いため
        //SEのクリップ数と同じだけAudioSourceを用意する
        SE_GameSource = new AudioSource[se_GameClip.Length];

        for (int i = 0; i < se_GameClip.Length; i++)
        {
            if (se_GameClip[i] != null)
            {
                SE_GameSource[i] = gameObject.AddComponent<AudioSource>();
                SE_GameSource[i].loop = false;
                SE_GameSource[i].priority = 1;
                SE_GameSource[i].playOnAwake = false;
                SE_GameSource[i].clip = se_GameClip[i];
                SE_GameSource[i].outputAudioMixerGroup = SEGroup;
            }
        }

        SE_JingleSource = new AudioSource[se_JingleClip.Length];

        for (int i = 0; i < se_JingleClip.Length; i++)
        {
            if (se_JingleClip[i] != null)
            {
                SE_JingleSource[i] = gameObject.AddComponent<AudioSource>();
                SE_JingleSource[i].loop = false;
                SE_JingleSource[i].priority = 1;
                SE_JingleSource[i].playOnAwake = false;
                SE_JingleSource[i].clip = se_JingleClip[i];
                SE_JingleSource[i].outputAudioMixerGroup = BGMGroup;
            }
        }
    }

    void Start()
    {
        //保存された音量をロード
        int vol1 = PlayerPrefs.GetInt("BGMVol");
        int vol2 = PlayerPrefs.GetInt("SEVol");

        mixer.SetFloat("BGVol", vol_BGM[vol1]);
        mixer.SetFloat("SEVol", vol_SE[vol2]);

        bgmVol = vol_BGM[vol1];

        if (bgmVolSlider != null && seVolSlider != null &&
            bgmVolText != null && seVolText != null)
        {
            //SliderとTextの値を設定
            bgmVolSlider.value = vol1;
            seVolSlider.value = vol2;

            bgmVolText.text = (vol1 * 10).ToString() + "%";
            seVolText.text = (vol2 * 10).ToString() + "%";
        }
    }

    //BGMを外部から呼び出す時
    public void PlayBGM(int i)
    {
        BGMSource.clip = bgmClip[i];
        BGMSource.Play();
    }

    //BattleBGMを外部から呼び出す時
    public void PlayBGM_Battle(int i)
    {
        BGMSource.clip = bgm_BattleClip[i];
        BGMSource.Play();
    }

    //BGMを外部から中断時
    public void PauseBGM()
    {
        BGMSource.Pause();
    }

    //BGMを外部から停止時
    public void StopBGM()
    {
        BGMSource.Stop();
    }

    //SytemSEを外部から呼び出す時
    public void PlaySE_Sys(int i)
    {
        SE_SysSource.clip = se_SysClip[i];
        SE_SysSource.Play();
    }

    //SytemSEを外部から停止時
    public void StopSE_Sys(int i)
    {
        SE_SysSource.clip = se_SysClip[i];
        SE_SysSource.Stop();
    }

    //GameSEを外部から呼び出す時
    public void PlaySE_Game(int i, bool overlap = true)
    {
        //overlapがfalseの場合はすでに再生中のSEは再生しない
        if (overlap || !SE_GameSource[i].isPlaying)
        SE_GameSource[i].Play();
    }

    //GameSEを外部から重複ありで呼び出す時
    public void PlaySE_OneShot_Game(int i, bool overlap = true)
    {
        //overlapがfalseの場合はすでに再生中のSEは再生しない
        if (overlap || !SE_GameSource[i].isPlaying)
        SE_GameSource[i].PlayOneShot(SE_GameSource[i].clip);
    }

    //JingleSEを外部から呼び出す時
    public void PlaySE_Jingle(int i)
    {
        SE_JingleSource[i].Play();
    }

    //Silderによる音量の調整
    //（第1引数でBGM、第2引数でSEのボリューム）
    public void VolumeChange(int vol1, int vol2)
    {
        mixer.SetFloat("BGVol", vol_BGM[vol1]);
        mixer.SetFloat("SEVol", vol_SE[vol2]);

        bgmVol = vol_BGM[vol1];
    }

    //SilderによるBGM音量の調整
    public void BgmVolumeChange()
    {
        mixer.SetFloat("BGVol", vol_BGM[(int)bgmVolSlider.value]);
        bgmVol = vol_BGM[(int)bgmVolSlider.value];

        bgmVolText.text = ((int)bgmVolSlider.value * 10).ToString() + "%";
    }

    //SilderによるSE音量の調整
    public void SeVolumeChange()
    {
        mixer.SetFloat("SEVol", vol_SE[(int)seVolSlider.value]);

        seVolText.text = ((int)seVolSlider.value * 10).ToString() + "%";
    }

    //画面がフェードアウトする時
    //音量も一緒にフェードアウト フェードするのはBGMのみ
    public IEnumerator FadeOut(float interval)
    {
        float time = 0;
        while (time <= interval)
        {
            float bgm = Mathf.Lerp(bgmVol, -80f, time / interval);
            mixer.SetFloat("BGVol", bgm);
            time += Time.deltaTime;
            yield return null;
        }
    }

    //画面がフェードアウトする時
    //音量も一緒にフェードイン
    public IEnumerator FadeIn(float interval)
    {
        float time = 0;
        while (time <= interval)
        {
            float bgm = Mathf.Lerp(-80f,bgmVol, time / interval);
            mixer.SetFloat("BGVol", bgm);
            time += Time.deltaTime;
            yield return null;
        }
    }
}

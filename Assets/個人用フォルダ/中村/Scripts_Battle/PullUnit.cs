using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI上のユニットをクリックで取得する処理
/// </summary>
public class PullUnit : MonoBehaviour
{
    public int index;                                             //どのユニットか

    [Space(10)]

    [SerializeField] float noClickOffsetY;                        //クリック不可能な場合にY軸を下げる　その値
    Vector3 defaultPos;
    Vector3 noClickPos;

    [Space(10)]

    [SerializeField] Image image;                                 //画像の親
    [SerializeField] Image unitImage;                             //ユニットの画像

    [Space(10)]

    [SerializeField] Image[] images;                              //自身に含まれる画像コンポーネント

    [Space(10)]

    [SerializeField] TextMeshProUGUI[] texts;                     //ユニット名のテキスト
    [SerializeField, Label("DownAlpha(0～255)")] float downAlpha; //非アクティブ時に下げる透明度(0～255)
    float[] alpha_Images, alpha_Texts;                            //各コンポーネントの初期透明度

    [Space(10)]

    public TextMeshProUGUI text_Cost;                          //コストを表すテキスト
    public TextMeshProUGUI text_SameMaxInstallation;           //同時に何体配置できるかを表すテキスト
    [SerializeField] TextMeshProUGUI text_RecastTime;          //リキャスト時間を表すテキスト
    [SerializeField] GameObject noClickWindow;                 //クリック不可能な場合に表示するオブジェクト

    float timer_Recast;                                        //リキャスト用タイマー

    //状態を表すフラグ
    bool isNoPull, isRecast, isDrag, isEnabled;

    void Start()
    {
        //当たり判定のサイズを見た目に合わせる
        RectTransform rectT = GetComponent<RectTransform>();
        BoxCollider col = GetComponent<BoxCollider>();
        col.size = new Vector3(rectT.sizeDelta.x, rectT.sizeDelta.y, 1);

        //クリック不可能時の位置を決める
        defaultPos = image.rectTransform.localPosition;
        noClickPos = new Vector3(defaultPos.x, defaultPos.y - noClickOffsetY, defaultPos.z);

        //クリック不可能オブジェクトを非表示
        text_RecastTime.gameObject.SetActive(false);
        noClickWindow.SetActive(false);

        //初期状態の透明度を保存
        alpha_Images = new float[images.Length];
        for (int i = 0; i < alpha_Images.Length; i++)
        {
            alpha_Images[i] = images[i].color.a;
        }
        alpha_Texts = new float[texts.Length];
        for (int i = 0; i < alpha_Texts.Length; i++)
        {
            alpha_Texts[i] = texts[i].color.a;
        }
        downAlpha = (Mathf.Min(downAlpha, 255) > 0) ? Mathf.Min(downAlpha, 255) / 255 : (Mathf.Min(downAlpha, 255) * -1) / 255;

        //背景の画像を読み込み
        image.sprite = UnitsData.Instance.iconBackSprite[ParameterManager.Instance.unitStatus[index].role];

        //ユニットの画像を読み込み
        unitImage.sprite = ParameterManager.Instance.unitStatus[index].sprite;

        //ユニット名を読み込み
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].text == "ユニット名")
            {
                texts[i].text = "";
                string uName = ParameterManager.Instance.unitStatus[index].name;

                //５文字を超える場合は改行
                int maxNameLength = 5;
                if (uName.Length >= maxNameLength)
                {
                    int loop = (uName.Length / maxNameLength > 0) ? (uName.Length / maxNameLength) + 1 : 0;
                    for (int j = 0; j < loop; j++)
                    {
                        int removeNum = (uName.Length >= maxNameLength) ? maxNameLength : uName.Length;
                        texts[i].text += uName.Substring(0, removeNum);
                        texts[i].text += (j < loop - 1) ? "\n" : "";
                        uName = uName.Remove(0, removeNum);
                    }
                }
                else
                {
                    texts[i].text = uName;
                }

                break;
            }
        }

        isEnabled = true;
    }

    void Update()
    {
        if (!BattleManager.Instance.isMainGame) return; //メインゲーム中でなければ戻る

        //コスト用のポイントが足りていない、またはユニットの配置数が最大の場合はtrueになる
        isNoPull = (BattleManager.Instance.point < BattleManager.Instance.unitCost[index] ||
                    BattleManager.Instance.isMaxInstallation ||
                    BattleManager.Instance.unitMaxInstallation[index]) ?
                    true : false;

        //クリック不可能フラグが立っている場合は位置を下げ、クリック不可能オブジェクトを表示
        if (isNoPull && !noClickWindow.activeSelf || isRecast && !noClickWindow.activeSelf)
        {
            image.rectTransform.localPosition = noClickPos;
            noClickWindow.SetActive(true);
        }
        else if (!isNoPull && !isRecast && noClickWindow.activeSelf)
        {
            SoundManager.Instance.PlaySE_Game(5);

            image.rectTransform.localPosition = defaultPos;
            noClickWindow.SetActive(false);
        }

        //このコンポーネントからユニットがドラッグされている場合
        if (isDrag)
        {
            //このコンポーネントのユニットが置かれた場合の処理
            if (BattleManager.Instance.isUnitPlace)
            {
                //リキャスト開始
                timer_Recast = BattleManager.Instance.unitRecast[index] - Time.fixedDeltaTime;
                text_RecastTime.gameObject.SetActive(true);
                isRecast = true;

                if (isDrag && !isEnabled)
                {
                    //全ての子オブジェクトを再表示
                    ReEnabled();
                }

                BattleManager.Instance.isUnitPlace = false;
                isDrag = false;
            }
            else if (!BattleManager.Instance.isUnitDrag)
            {
                if (isDrag && !isEnabled)
                {
                    //全ての子オブジェクトを再表示
                    ReEnabled();
                }

                isDrag = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!BattleManager.Instance.isMainGame) return; //メインゲーム中でなければ戻る

        //リキャスト時間をカウント
        if (isRecast)
        {
            if (timer_Recast > 0)
            {
                timer_Recast -= Time.fixedDeltaTime;
                text_RecastTime.text = Mathf.Max(Mathf.CeilToInt(timer_Recast), 1f).ToString();
            }
            else
            {
                text_RecastTime.gameObject.SetActive(false);
                isRecast = false;
            }
        }
    }

    //マウスクリックでユニットを持つ
    void OnMouseDown()
    {
        if (!BattleManager.Instance.isMainGame) return; //メインゲーム中でなければ戻る

        if (isNoPull || isRecast || isDrag) return;

        //全ての子オブジェクトを非表示
        NoEnabled();

        BattleManager.Instance.PullUnit(index);
        isDrag = true;
    }
    //マウスが離れた場合
    void OnMouseExit()
    {
        if (!BattleManager.Instance.isMainGame) return; //メインゲーム中でなければ戻る

        if (isDrag && !isEnabled)
        {
            //全ての子オブジェクトを再表示
            ReEnabled();
        }
    }

    //自身を非表示に
    void NoEnabled()
    {
        if (!isEnabled) return;

        //画像を薄く
        for (int i = 0; i < alpha_Images.Length; i++)
        {
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, images[i].color.a - downAlpha);
        }
        //テキストを薄く
        for (int i = 0; i < alpha_Texts.Length; i++)
        {
            texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, texts[i].color.a - downAlpha);
        }

        isEnabled = false;
    }

    //自身を再表示
    void ReEnabled()
    {
        if (isEnabled) return;

        //画像を再表示
        for (int i = 0; i < alpha_Images.Length; i++)
        {
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, alpha_Images[i]);
        }
        //テキストを再表示
        for (int i = 0; i < alpha_Texts.Length; i++)
        {
            texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, alpha_Texts[i]);
        }

        isEnabled = true;
    }
}

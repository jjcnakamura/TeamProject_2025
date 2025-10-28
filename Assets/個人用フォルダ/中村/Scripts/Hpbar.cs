using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [System.NonSerialized] public BattleUnit_Base targetUnit; //HPを表示するユニット
    [System.NonSerialized] public Enemy_Base targetEnemy;     //HPを表示する敵

    Transform target;

    RectTransform parentRect;
    Slider slider;

    //Sliderの位置をどれだけずらすか
    Vector2 offset = new Vector2(36.9602f, 50f);
    float maxTargetPosX = 20f;

    bool isStart, isUnit;

    void Update()
    {
        //Start
        if (!isStart)
        {
            //ユニットと敵どっちのコンポーネントか
            if (targetUnit != null)
            {
                target = targetUnit.transform;
                isUnit = true;
            }
            else if (targetEnemy != null)
            {
                target = targetEnemy.transform;
                isUnit = false;
            }
            else
            {
                return;
            }

            parentRect = BattleManager.Instance.hpbarParent.GetComponent<RectTransform>();
            slider = GetComponent<Slider>();
            isStart = true;
        }
        //Update
        else
        {
            if (!BattleManager.Instance.isMainGame) return; //メインゲーム中でなければ戻る

            //Sliderの位置をキャラに合わせる
            Vector3 targetWorldPos = target.position;
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, targetScreenPos, Camera.main, out Vector2 lcalPos);
            float xOffset = offset.x * (target.position.x / maxTargetPosX);
            Vector2 pos = new Vector2(lcalPos.x + xOffset, lcalPos.y + offset.y);
            transform.localPosition = pos;

            //HPをSliderに反映
            slider.value = (isUnit) ? (float)targetUnit.hp / (float)targetUnit.maxHp : (float)targetEnemy.hp / (float)targetEnemy.maxHp;
        }
    }
}

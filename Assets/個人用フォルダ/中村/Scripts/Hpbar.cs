using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [System.NonSerialized] public BattleUnit_Base targetUnit; //HP��\�����郆�j�b�g
    [System.NonSerialized] public Enemy_Base targetEnemy;     //HP��\������G

    Transform target;

    RectTransform parentRect;
    Slider slider;

    //Slider�̈ʒu���ǂꂾ�����炷��
    Vector2 offset = new Vector2(36.9602f, 50f);
    float maxTargetPosX = 20f;

    bool isStart, isUnit;

    void Update()
    {
        //Start
        if (!isStart)
        {
            //���j�b�g�ƓG�ǂ����̃R���|�[�l���g��
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
            if (!BattleManager.Instance.isMainGame) return; //���C���Q�[�����łȂ���Ζ߂�

            //Slider�̈ʒu���L�����ɍ��킹��
            Vector3 targetWorldPos = target.position;
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, targetScreenPos, Camera.main, out Vector2 lcalPos);
            float xOffset = offset.x * (target.position.x / maxTargetPosX);
            Vector2 pos = new Vector2(lcalPos.x + xOffset, lcalPos.y + offset.y);
            transform.localPosition = pos;

            //HP��Slider�ɔ��f
            slider.value = (isUnit) ? (float)targetUnit.hp / (float)targetUnit.maxHp : (float)targetEnemy.hp / (float)targetEnemy.maxHp;
        }
    }
}

using UnityEngine;

/// <summary>
/// 戦闘ステージクリア後にマップシーンに戻ったか判定する
/// </summary>
public class BattleClearCheck : MonoBehaviour
{
    void Start()
    {
        if (FindObjectOfType(System.Type.GetType("MapManager")) != null && !MapManager.Instance.gameObject.activeSelf)
        {
            //マップを再表示
            MapManager.Instance.gameObject.SetActive(true);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class toggle : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles; // 9��Toggle��o�^
    [SerializeField] private int maxChecked = 3;   // ������ON�ɂł���ő吔

    private void Start()
    {
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle); });
        }
    }

    private void OnToggleChanged(Toggle changedToggle)
    {
        // ����ON�̐��𐔂���
        int onCount = 0;
        foreach (var t in toggles)
        {
            if (t.isOn) onCount++;
        }

        // ��������𒴂��Ă�����A����ON�ɂ���Toggle��OFF�ɖ߂�
        if (onCount > maxChecked)
        {
            changedToggle.isOn = false;
        }
    }
}

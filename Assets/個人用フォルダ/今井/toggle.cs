using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class toggle : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles; // 9ŒÂ‚ÌToggle‚ð“o˜^
    [SerializeField] private int maxChecked = 3;   // “¯Žž‚ÉON‚É‚Å‚«‚éÅ‘å”

    private void Start()
    {
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle); });
        }
    }

    private void OnToggleChanged(Toggle changedToggle)
    {
        // Œ»ÝON‚Ì”‚ð”‚¦‚é
        int onCount = 0;
        foreach (var t in toggles)
        {
            if (t.isOn) onCount++;
        }

        // ‚à‚µãŒÀ‚ð’´‚¦‚Ä‚¢‚½‚çA¡‰ñON‚É‚µ‚½Toggle‚ðOFF‚É–ß‚·
        if (onCount > maxChecked)
        {
            changedToggle.isOn = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvent : MonoBehaviour
{
    public bool[] Events;
    public bool allFalse;

    public void GoEvent()
    {
        Transform stageinfo = MapManager.Instance.nextStage.parent;
        StageInfo Stage = stageinfo.GetComponent<StageInfo>();
        if(Stage != null)
        {
            if (Stage.StageName == "ƒCƒxƒ“ƒg")
            {
                int[] num = EventWindowManager.Instance.EventRandomChoice(4);
                int i = num[UnityEngine.Random.Range(0, num.Length)];
                EventWindowManager.Instance.CallEventAt(i);
                Events[i] = true;
            }
        }
    }

    void Update()
    {
        for(int i = 0; i < Events.Length; i++)
{
            if (Events[i] == true)
            {
                allFalse = true;
                break;
            }
        }

        if (allFalse)
        {
            Events = new bool[5];
            Array.Fill(Events, false);
            allFalse = false;
            // ‚·‚×‚Ä false ‚Ì‚Æ‚«‚Ìˆ—
        }
        
    }
}

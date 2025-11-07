using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Singleton<FlagManager>
{
    [System.NonSerialized] public bool isSpeedUp;

    void Awake()
    {
        //ƒV[ƒ“‚ğ‘JˆÚ‚µ‚Ä‚àc‚é
        if (gameObject.transform.parent != null) gameObject.transform.parent = null;
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}

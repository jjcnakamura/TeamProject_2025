using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{
    [SerializeField] Vector3 scale;
    [SerializeField] float time;
    [SerializeField] float interval;
    [SerializeField] bool unscaleTime;

    Vector3 startScale, endScale;
    bool inversion;
    float timer;

    void Start()
    {
        startScale = transform.localScale;
        endScale = startScale + scale;
    }

    void Update()
    {
        if(timer <= time)
        {
            timer += (unscaleTime) ? Time.unscaledDeltaTime : Time.deltaTime;

            if (!inversion)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, timer / time);
            }
            else
            {
                transform.localScale = Vector3.Lerp(endScale, startScale, timer / time);
            }
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        if (timer <= time + interval)
        {
            timer += (unscaleTime) ? Time.unscaledDeltaTime : Time.deltaTime;
        }
        else
        {
            inversion = !inversion;
            timer = 0;
        }
    }
}

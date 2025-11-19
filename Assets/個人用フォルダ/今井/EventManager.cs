using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameObject[] EventCanvas;
    public bool event0;
    public bool event1;
    public bool event2;
    public bool event3;
    public bool event4;
    public bool event5;
    public bool event6;
    public bool event7;
    public bool event8;
    public bool event9;

    public bool fastReset;

    private void Start()
    {
        event0 = false; event1 = false; event2 = false; event3 = false; event4 = false; event5 = false; event6 = false;event7 = false;event8 = false;event9 = false;
    }

    void Update()
    {

    }

    public void EventReset()
    {
        
    }
    public void Event0()
    {
        event0 = false;
    }
    public void Event1()
    {
        event1 = false;
    }
    public void Event2()
    {
        event2 = false;
    }
    public void Event3()
    {
        event3 = false;
    }
    public void Event4()
    {
        event4 = false; 
    }
    public void Event5()
    {
        event5 = false; 
    }
    public void Event6()
    {
        event6 = false; 
    }
    public void Event7()
    {
        event7 = false; 
    }
    public void Event8()
    {
        event8 = false; 
    }
    public void Event9()
    {
        event9 = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    void Update()
    {
        
    }
    public void LoadS(int i)
    {
        SceneManager.LoadScene(i);
    }
}

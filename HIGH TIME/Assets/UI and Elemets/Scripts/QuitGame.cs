using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{ 
    public void QuitApplication()
    {

        Debug.Log("QUit");
        Application.Quit();

    }
}

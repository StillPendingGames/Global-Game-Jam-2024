using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Utilities : MonoBehaviour
{
    // Start is called before the first frame update
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeScene(string NewScene)
    {
        SceneManager.LoadScene(NewScene);
    }
}

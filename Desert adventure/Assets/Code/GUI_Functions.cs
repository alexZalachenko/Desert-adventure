using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GUI_Functions : MonoBehaviour {

    static Stack<string> c_prevScene = new Stack<string>();

    public void OnClickPlay()
    {
        c_prevScene.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Game");
    }

    public void OnClickOptions()
    {
        c_prevScene.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Options");
    }

    public void OnClicExit()
    {
        if (c_prevScene.Count == 0)
            Application.Quit();
        else
            SceneManager.LoadScene(c_prevScene.Pop());
    }
}
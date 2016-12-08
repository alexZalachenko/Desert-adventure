using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GUI_Functions : MonoBehaviour {

    static Stack<string> c_prevScene = new Stack<string>();

    public void BackInMenu()
    {
        if (c_prevScene.Count == 0)
            Application.Quit();
        else
            SceneManager.LoadScene(c_prevScene.Pop());
    }

    public void DisableInGameOptions()
    {
        Time.timeScale = 1;
        GameObject.Find("InGameOptionsMenu").SetActive(false);
    }

    public void LoadScene(string p_scene)
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            c_prevScene.Clear();
            Time.timeScale = 1;
        }
        else
        c_prevScene.Push(SceneManager.GetActiveScene().name);

        SceneManager.LoadScene(p_scene);
    }

    public void SetSoundOptions(bool p_newState)
    {
        SoundManager.Instance.SoundActive = p_newState;
    }
}
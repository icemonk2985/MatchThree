using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    //Instance Variable
    public static GameSceneManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Destroy()
    {
        if (instance == this)
            instance = null;
    }

    public void LoadThisScene(string _SceneToLoad)
    {
        if (_SceneToLoad != "" && _SceneToLoad != " ")
        {
            Time.timeScale = 1.0f;
            if (_SceneToLoad == "Quit")
            {
                //Quit
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(_SceneToLoad);
            }
        }
        else
        {
            Debug.LogError("Game Start Manager - No Scene has been Selected");
        }
    }

    public IEnumerator LoadThisSceneAfterDelay(string _SceneToLoad, float _Delay)
    {
        yield return new WaitForSeconds(_Delay);
        LoadThisScene(_SceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLimitedMoves()
    {
        LoadThisScene("LimitedMoves");
    }

    public void LoadLimitedTime()
    {
        LoadThisScene("LimitedTime");
    }

    public void LoadFreePlay()
    {
        LoadThisScene("FreePlay");
    }

    public void GameOverImmediate()
    {
        LoadThisScene("MenuScene");
    }

    public void GameOverDelay()
    {
        StartCoroutine(LoadThisSceneAfterDelay("MenuScene", 4.5f));
    }
}
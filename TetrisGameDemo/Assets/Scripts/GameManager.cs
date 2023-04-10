using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event System.Action GameOverEvent;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameOverEvent += GameStopper;
    }

    public void GameOver()
    {
        GameOverEvent?.Invoke();
    }


    private void GameStopper()
    {
        Time.timeScale = 0;
    }


    public void LoadlevelScene(int level)
    {
        StartCoroutine(LoadLevelSceneAsync(level));
    }

    private IEnumerator LoadLevelSceneAsync(int level)
    {
        yield return null;
        SceneManager.LoadSceneAsync(level);
        Time.timeScale = 1;
    }

}

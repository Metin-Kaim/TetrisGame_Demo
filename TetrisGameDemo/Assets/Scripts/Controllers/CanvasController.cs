using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject _gameOverPanel;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);
    }

    public void OnEnable()
    {
        GameManager.Instance.GameOverEvent += GameOverPanel;
    }

    public void OnDisable()
    {
        GameManager.Instance.GameOverEvent -= GameOverPanel;
    }
    public void GameOverPanel()
    {
        _gameOverPanel.SetActive(true);
    }

    public void LoadLevel(int level)
    {
        GameManager.Instance.LoadlevelScene(level);
    }

}

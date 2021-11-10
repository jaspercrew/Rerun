using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SpriteRenderer _renderer;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject fg = GameObject.FindGameObjectWithTag("Foreground");
        _renderer = fg ? fg.GetComponent<SpriteRenderer>() : null;
        
        SaveScript.Load();
    }

    private void Update() {
        if (Input.GetKeyDown("r")) {
            RestartLevel();
        }
        
        
    }
    public void GamePause() {
        Time.timeScale = 0;
        if (_renderer)
            _renderer.enabled = true;
    }

    public void GameResume() {
        Time.timeScale = 1;
        if (_renderer)
            _renderer.enabled = false;
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
    
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveGameAndSettings()
    {
        SaveScript.Save();
    }

    private void OnApplicationQuit()
    {
        SaveGameAndSettings();
    }
}

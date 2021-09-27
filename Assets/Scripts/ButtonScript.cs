using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {
    private SpriteRenderer _renderer;
    // Start is called before the first frame update
    void Start() {
        _renderer = GameObject.FindGameObjectWithTag("Foreground").GetComponent<SpriteRenderer>();

    }

    public void GamePause() {
        Time.timeScale = 0;
        _renderer.enabled = true;
    }

    public void GameResume() {
        Time.timeScale = 1;
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
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

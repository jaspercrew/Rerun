using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SpriteRenderer _renderer;

    // Start is called before the first frame update
    private void Start() {
        _renderer = GameObject.FindGameObjectWithTag("Foreground").GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(1))
    //     {
    //         Debug.Log("m1");
    //         ScreenCapture.CaptureScreenshot(Application.dataPath + "/screenshots/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png");
    //         UnityEditor.AssetDatabase.Refresh();
    //     }
    //     
    //     if (Input.GetKeyDown("t"))
    //     {
    //         Debug.Log("t");
    //         StartCoroutine(FindObjectOfType<SpawnScript>().SpawnCharacter());
    //     }
    //     
    // }
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
    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(1))
    //     {
    //         Debug.Log("m1");
    //         ScreenCapture.CaptureScreenshot(Application.dataPath + "/screenshots/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png");
    //         UnityEditor.AssetDatabase.Refresh();
    //     }
    //     
    //     if (Input.GetKeyDown("t"))
    //     {
    //         Debug.Log("t");
    //         StartCoroutine(FindObjectOfType<SpawnScript>().SpawnCharacter());
    //     }
    //     
    // }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
    
    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

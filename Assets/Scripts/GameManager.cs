using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is ca lled before the first frame update
    private void Start() {
        GameObject.FindGameObjectWithTag("Menu Canvas").layer = 5;

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("m1");
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/screenshots/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png");
            UnityEditor.AssetDatabase.Refresh();
        }
        
        if (Input.GetKeyDown("t"))
        {
            Debug.Log("t");
            StartCoroutine(FindObjectOfType<SpawnScript>().SpawnCharacter());
        }
        
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
    
    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

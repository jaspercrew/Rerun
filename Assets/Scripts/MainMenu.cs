using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject chapterSelectPrefab;

    private const int Back = 0;
    private const int MinLevel = 1;
    private const int MaxLevel = 4;
    private const int Last = 5;
    private const int Next = 6;
    
    private void Start()
    {
        Transform parent = transform.parent;
        // Console.WriteLine("hello");
        
        // Debug.Log(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/not_a_real_scene.unity"));
        // Debug.Log(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Level 2-2.unity"));
        

        GameObject[] chapterSelects = new GameObject[Constants.NumWorlds];
        // Debug.Log(chapterSelects);
        
        for (int world = 0; world < chapterSelects.Length; world++)
        {
            int worldNum = world + 1;
            GameObject chapterSelect = Instantiate(chapterSelectPrefab, parent);
            chapterSelect.name = "Chapter " + worldNum + " Level Select";
            chapterSelect.SetActive(false);
            chapterSelects[world] = chapterSelect;
            // Debug.Log(chapterSelects[world]);

            for (int level = 0; level <= MaxLevel - MinLevel; level++)
            {
                int levelNum = level + 1;
                Transform button = chapterSelect.transform.GetChild(level + MinLevel);
                button.GetChild(1).GetComponent<TextMeshProUGUI>().text = worldNum + "-" + levelNum;
                
                bool isLevelAvailable = (world == 0 && level == 0) || // first level
                                        // last level in previous world
                                        (level == 0 && SaveScript.IsLevelComplete(world - 1, -1)) ||
                                        // previous level in this world
                                        (SaveScript.IsLevelComplete(world, level - 1));
                
                if (!SaveScript.LevelExists(world, level)) {
                    // Debug.Log("disabling " + worldNum + "-" + levelNum);
                    button.gameObject.SetActive(false);
                }
                else if (isLevelAvailable)
                {
                    // Debug.Log("unlocking " + worldNum + "-" + levelNum);
                    button.GetChild(0).gameObject.SetActive(false); // disable lock
                    button.GetChild(1).gameObject.SetActive(true); // enable text
                    button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(worldNum, levelNum));
                }
                else
                {
                    // Debug.Log("locking " + worldNum + "-" + levelNum);
                    button.GetChild(0).gameObject.SetActive(true); // enable lock
                    button.GetChild(1).gameObject.SetActive(false); // disable text
                }
                
            }
        }
        
        for (int chapter = 0; chapter < chapterSelects.Length; chapter++)
        {
            int c = chapter; // apparently this is needed?
            // Debug.Log(c);

            Button backButton = chapterSelects[c].transform.GetChild(Back).GetComponent<Button>();
            backButton.onClick.AddListener(() => chapterSelects[c].SetActive(false));
            backButton.onClick.AddListener(() => gameObject.SetActive(true)); // re-enable main menu

            if (c == 0)
            {
                chapterSelects[c].transform.GetChild(Last).gameObject.SetActive(false);
            }
            else
            {
                Button lastButton = chapterSelects[c].transform.GetChild(Last).GetComponent<Button>();
                lastButton.onClick.AddListener(() => chapterSelects[c - 1].SetActive(true));
                lastButton.onClick.AddListener(() => chapterSelects[c].SetActive(false));
            }

            if (c == chapterSelects.Length - 1)
            {
                chapterSelects[c].transform.GetChild(Next).gameObject.SetActive(false);
            }
            else
            {
                Button nextButton = chapterSelects[c].transform.GetChild(Next).GetComponent<Button>();
                // Debug.Log(c + ", " + (chapterSelects.Length - 1));
                nextButton.onClick.AddListener(() => chapterSelects[c + 1].SetActive(true));
                nextButton.onClick.AddListener(() => chapterSelects[c].SetActive(false));
            }
        }
        
        Button playButton = transform.GetChild(0).GetComponent<Button>();
        // Debug.Log(transform.GetChild(0).gameObject.name);
        playButton.onClick.AddListener(() => chapterSelects[0].SetActive(true));
        playButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private static void LoadLevel(int worldNum, int levelNum)
    {
        SceneManager.LoadScene("Level " + worldNum + "-" + levelNum);
    }
}

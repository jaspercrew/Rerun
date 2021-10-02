using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [Min(1)] public int numReruns = 1;
    public GameObject coinPrefab;
    public float coinLoopTime = 5;
    private float _reciprocalLoopTime;

    private int _runsLeft;
    // private GameObject _spawn;
    private GameObject _player;

    private GameObject _coinsParent;
    private GameObject[] _coins;

    private float _reciprocalCoinsLen;
    //private SpriteRenderer _rewindEffect;

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _reciprocalLoopTime = 1f / coinLoopTime;
        // _reciprocalCoinsLen = (_coins.Length == 0)? -1 : 1 / _coins.Length;
        
        _runsLeft = numReruns;
        // _spawn = GameObject.FindGameObjectWithTag("Spawn");
        // _rewindEffect = GameObject.FindGameObjectWithTag("RewindEffect")
        //         .GetComponent<SpriteRenderer>();
        //_rewindEffect.enabled = false;

        _coinsParent = new GameObject("Stars")
        {
            transform =
            {
                parent = transform
            }
        };
        PlaceStars();
    }

    private void PlaceStars()
    {
        foreach (Transform child in _coinsParent.transform)
        {
            Destroy(child.gameObject);
        }

        _coins = new GameObject[_runsLeft];
        _reciprocalCoinsLen = 1f / _runsLeft;

        for (int i = 0; i < _coins.Length; i++)
        {
            _coins[i] = Instantiate(coinPrefab, _coinsParent.transform, true);
            // c.transform.position = CoinPos(i);
        }
    }
    
    private Vector3 CoinPos(int i)
    {
        float t = ((Time.time * _reciprocalLoopTime) % 1 + i * _reciprocalCoinsLen) 
                  * 2f * Mathf.PI;
        Vector3 sca = transform.localScale;
        Vector3 pos = transform.position;
        float x = pos.x + sca.x * Mathf.Sin(t);
        float y = pos.y + sca.y * Mathf.Cos(t);
        return new Vector3(x, y, pos.z);
    }


    // Start is called before the first frame update
    // private void Start()
    // {
    //     if (_spawn == null)
    //     {
    //         Debug.LogError("couldn't find spawn object!");
    //     }
    // }
    
    
    private void Update()
    {
        for (int i = 0; i < _runsLeft; i++)
        {
            Transform c = _coins[i].transform;
            c.position = CoinPos(i);
        }
    }


    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (!player.CompareTag("Player")) return;
        
        // player reached portal
        _runsLeft--;

        StartCoroutine(_runsLeft > 0 ?
                _player.GetComponent<RewindManager>().DelayedRewind(.18f) :
                _player.GetComponent<RewindManager>().Disappear(.18f));


        if (_runsLeft == 0)
        {
            try
            {
                string sceneName = SceneManager.GetActiveScene().name;
                string[] parts = sceneName.Split(' ');
                string[] levelParts = parts[1].Split('-');
                int world = int.Parse(levelParts[0]);
                int level = int.Parse(levelParts[1]);
                SaveScript.UpdateLevel(world, level);
            }
            catch
            {
                Debug.LogError("ERROR: couldn't save level completion because " +
                               "scene name couldn't be parsed");
            }
        }

        PlaceStars();
    }

    // private static void BeginNextLevel() {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
}

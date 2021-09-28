using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [Min(1)] public int numReruns = 1;
    public GameObject coinPrefab;
    public float coinLoopTime = 5;

    private int _runsLeft;
    private GameObject _spawn;
    private GameObject _player;

    private GameObject _coinsParent;
    private GameObject[] _coins;
    //private SpriteRenderer _rewindEffect;

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player");
        
        _runsLeft = numReruns;
        _spawn = GameObject.FindGameObjectWithTag("Spawn");
        // _rewindEffect = GameObject.FindGameObjectWithTag("RewindEffect")
        //         .GetComponent<SpriteRenderer>();
        //_rewindEffect.enabled = false;

        _coinsParent = new GameObject("stars")
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

        for (int i = 0; i < _coins.Length; i++)
        {
            GameObject c = Instantiate(coinPrefab, 
                _coinsParent.transform, true);
            _coins[i] = c;
            // c.transform.position = CoinPos(i);
        }
    }
    
    private Vector2 CoinPos(int i)
    {
        float t = ((Time.time % coinLoopTime) / coinLoopTime + (float) i / _coins.Length) 
                  * 2f * Mathf.PI;
        var sca = transform.localScale;
        var pos = transform.position;
        float x = pos.x + sca.x * Mathf.Sin(t);
        float y = pos.y + sca.y * Mathf.Cos(t);
        return new Vector2(x, y);
    }


    // Start is called before the first frame update
    private void Start()
    {
        if (_spawn == null)
        {
            Debug.LogError("couldn't find spawn object!");
        }
    }
    
    
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

        PlaceStars();
    }

    // private static void BeginNextLevel() {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
}

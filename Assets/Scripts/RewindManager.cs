using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour {
    private GameObject _character;
    private Transform _charTransform;

    // private GameObject _spawn;
    private SpriteRenderer _rewindEffect;
    private SpriteRenderer _fore;
    private PostProcessingManager _ppm;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

    public bool isRewinding;
    public bool isCompleting;
    //public bool isDelayedRewinding = false;
    
    private List<PointInTime> _pointsInTime;
    
    private Vector3 _vel = Vector3.zero;
    private RectTransform _levelComplete;

    private void Awake() {
        _character = GameObject.FindGameObjectWithTag("Player");
        _charTransform = _character.transform;
        _ppm = FindObjectOfType<PostProcessingManager>();
        // _spawn = GameObject.FindGameObjectWithTag("Spawn");
        _pointsInTime = new List<PointInTime>();
        _rigidbody = _character.GetComponent<Rigidbody2D>();
        _collider = _character.GetComponent<BoxCollider2D>();
        _rewindEffect = GameObject.FindGameObjectWithTag("RewindEffect").GetComponent<SpriteRenderer>();
        _rewindEffect.enabled = false;
        _fore = GameObject.FindGameObjectWithTag("Foreground").GetComponent<SpriteRenderer>();
        _fore.enabled = false;
        
        _levelComplete = GameObject.FindGameObjectWithTag("LevelComplete")
            .GetComponent<RectTransform>();
    }
    
    
    public IEnumerator DelayedRewind(float delayTime)
    {
        _character.GetComponent<Rigidbody2D>().velocity += 
            Vector2.right * Input.GetAxisRaw("Horizontal") * _character.GetComponent<CharController>().speed;
        
        AudioManager.Instance.Play("Slow");
        _character.GetComponent<CharController>().isMovementEnabled = false;
        float timer = 0;
        while (timer < delayTime - 0.001f)
        {
            float perc = timer / delayTime;
            Time.timeScale = 1 - perc;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        
            timer += Time.deltaTime;
            yield return null;
        }

        _character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // Time.timeScale = 0.15f;
        // Time.fixedDeltaTime = 0.02F * Time.timeScale;
        // yield return new WaitForSeconds(delayTime);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F;
        StartRewind();
    }
    
    public IEnumerator Disappear(float delayTime) {
        _character.GetComponent<Rigidbody2D>().velocity += 
            Vector2.right * Input.GetAxisRaw("Horizontal") * _character.GetComponent<CharController>().speed;
        
        _character.GetComponent<BoxCollider2D>().enabled = false;
        _character.GetComponent<CharController>().FinishDust();
        AudioManager.Instance.Play("Slow");
        _character.GetComponent<CharController>().isMovementEnabled = false;
        
        SpriteRenderer sr = _character.GetComponent<SpriteRenderer>();
        
        float timer = 0;
        while (timer < delayTime - 0.001f)
        {
            float perc = timer / delayTime;
            Time.timeScale = 1 - perc;
            Color c = sr.color;
            c.a = 1 - perc;
            sr.color = c;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        
            timer += Time.deltaTime;
            yield return null;
        }
        AudioManager.Instance.Play("Level Complete");
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F;
        // Debug.Log("starting animation");
        isCompleting = true;
        // lc.SetActive(true);
        // LevelComplete(0.5f);
    }

    private void StartRewind() {
        AudioManager.Instance.Play("Rewind");
        _rigidbody.velocity = Vector3.zero; //diving fix
        
        _ppm.EnableFilmGrain();
        _collider.enabled = false;
        _rewindEffect.enabled = true;
        _fore.enabled = true;
        
        Time.timeScale = _pointsInTime.Count * Time.fixedDeltaTime / Constants.RewindTime;
        //x second flat time for rewind effect
        isRewinding = true;
        _rigidbody.isKinematic = true;
    }


    private void StopRewind() {
        _character.GetComponent<CharController>().isMovementEnabled = true;
        
        
        _ppm.DisableFilmGrain();
        _collider.enabled = true;
        _rewindEffect.enabled = false;
        _fore.enabled = false;
        
        Time.timeScale = 1;
        isRewinding = false;
        _rigidbody.isKinematic = false;
    }
    
    private void FixedUpdate() {
        if (isRewinding) {
            Rewind();
        }
        else {
            Record();
        }

        if (isCompleting)
        {
            _levelComplete.position = Vector3.SmoothDamp(_levelComplete.position, 
                Vector3.zero, ref _vel, 0.5f);

            if (_levelComplete.position == Vector3.zero)
                isCompleting = false;
        }
    }

    private void Record()
    {
        if (_pointsInTime.Count == 0)
        {
            _pointsInTime.Insert(0, 
                new PointInTime(_charTransform.position, _charTransform.rotation));
            return;
        }
        PointInTime last = _pointsInTime[0];
        if (last.Position != _charTransform.position || last.Rotation != _charTransform.rotation)
        {
            _pointsInTime.Insert(0, 
                new PointInTime(_charTransform.position, _charTransform.rotation));
        }

    }

    private void Rewind() {
        if (_pointsInTime.Count > 0) {
            PointInTime pointInTime = _pointsInTime[0];
            _charTransform.position = pointInTime.Position;
            _charTransform.rotation = pointInTime.Rotation;
            _pointsInTime.RemoveAt(0);
        }
        else {
            StopRewind();
        }
    }

    // private void LevelComplete(float delayTime)
    // {
    //     t.position = 450 * Vector3.down;
    //     
    //     float timer = 0;
    //     while (timer < delayTime)
    //     {
    //         float perc = timer / delayTime;
    //         float p = perc * perc * perc;
    //         t.position = (1 - p) * 450 * Vector3.down;
    //         Debug.Log(t.position);
    //         timer += Time.deltaTime;
    //     }
    // }
    
}

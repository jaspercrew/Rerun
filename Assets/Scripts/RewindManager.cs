using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewindManager : MonoBehaviour {
    private GameObject _character;
    private Transform _charTransform;

    private GameObject _spawn;
    private SpriteRenderer _rewindEffect;
    private SpriteRenderer _fore;
    private PostProcessingManager _ppm;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

    public bool isRewinding = false;
    public bool isCompleting = false;
    //public bool isDelayedRewinding = false;
    
    private List<PointInTime> pointsInTime;
    
    private Vector3 vel = Vector3.zero;
    private RectTransform lct;

    private void Awake() {
        _character = GameObject.FindGameObjectWithTag("Player");
        _charTransform = _character.transform;
        _ppm = GameObject.FindObjectOfType<PostProcessingManager>();
        _spawn = GameObject.FindGameObjectWithTag("Spawn");
        pointsInTime = new List<PointInTime>();
        _rigidbody = _character.GetComponent<Rigidbody2D>();
        _collider = _character.GetComponent<BoxCollider2D>();
        _rewindEffect = GameObject.FindGameObjectWithTag("RewindEffect").GetComponent<SpriteRenderer>();
        _rewindEffect.enabled = false;
        _fore = GameObject.FindGameObjectWithTag("Foreground").GetComponent<SpriteRenderer>();
        _fore.enabled = false;
        
        lct = GameObject.FindGameObjectWithTag("LevelComplete")
            .GetComponent<RectTransform>();
    }
    
    
    public IEnumerator DelayedRewind(float delayTime)
    {
        _character.GetComponent<Rigidbody2D>().velocity += 
            Vector2.right * Input.GetAxisRaw("Horizontal") * _character.GetComponent<CharController>().speed;
        
        FindObjectOfType<AudioManager>().Play("Slow");
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
        FindObjectOfType<AudioManager>().Play("Slow");
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
        FindObjectOfType<AudioManager>().Play("Level Complete");
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F;
        // Debug.Log("starting animation");
        isCompleting = true;
        // lc.SetActive(true);
        // LevelComplete(0.5f);
    }
    
    public void StartRewind() {
        FindObjectOfType<AudioManager>().Play("Rewind");
        _rigidbody.velocity = Vector3.zero; //diving fix
        
        _ppm.EnableFilmGrain();
        _collider.enabled = false;
        _rewindEffect.enabled = true;
        _fore.enabled = true;
        
        Time.timeScale = pointsInTime.Count * Time.fixedDeltaTime / Constants.RewindTime;
        //x second flat time for rewind effect
        isRewinding = true;
        _rigidbody.isKinematic = true;
    }
    

    public void StopRewind() {
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
            lct.position = Vector3.SmoothDamp(lct.position, 
                Vector3.zero, ref vel, 0.5f);

            if (lct.position == Vector3.zero)
                isCompleting = false;
        }
    }
    
    void Record()
    {
        if (pointsInTime.Count == 0)
        {
            pointsInTime.Insert(0, 
                new PointInTime(_charTransform.position, _charTransform.rotation));
            return;
        }
        PointInTime last = pointsInTime[0];
        if (last.position != _charTransform.position || last.rotation != _charTransform.rotation)
        {
            pointsInTime.Insert(0, 
                new PointInTime(_charTransform.position, _charTransform.rotation));
        }

    }

    private void Rewind() {
        if (pointsInTime.Count > 0) {
            PointInTime pointInTime = pointsInTime[0];
            _charTransform.position = pointInTime.position;
            _charTransform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharController: MonoBehaviour {
    
    private ParticleSystem[] _dusts;
    // private GameObject _spawn;
    // private SpawnScript _spawnScript;
    //private PostProcessingManager _ppm;

    public float speed = 2f;
    public float jumpForce = 10f;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

    // public float hangTime = .2f;
    // private float _hangCounter;

    public float jumpBufferLength = .2f;
    private float _jumpBufferCount;

    public bool isMovementEnabled;

    // private RewindManager _rewindManager;
    public float xDir = 2;

    private bool _justJumped;

    private readonly HashSet<Collider2D> _colliding = new HashSet<Collider2D>();

    private void CreateDust() => _dusts[1].Play();

    public void FinishDust() => _dusts[0].Play();

    private void Awake() {
        
        _dusts = GetComponentsInChildren<ParticleSystem>();
        //_ppm = GameObject.FindObjectOfType<PostProcessingManager>();
        // _spawn = GameObject.FindGameObjectWithTag("Spawn");
        // _spawnScript = _spawn.GetComponent<SpawnScript>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        // _rewindManager = GetComponent<RewindManager>();
        isMovementEnabled = true;
        
    }

    private void FixedUpdate() {
        //Vector2 move = new Vector2(Input.GetAxis("Horizontal"), 0f);
        //_rigidbody.MovePosition((Vector2)transform.position + (move * (speed * Time.deltaTime)));
        //Debug.Log(IsGrounded());
        //Debug.Log(_colliding.Count);

        float move = Input.GetAxisRaw("Horizontal");
        if (isMovementEnabled)
        {
            transform.position += new Vector3(move * speed * Time.deltaTime, 0, 0);
        }
        
        if (Math.Abs(xDir - move) > 0.01f && IsGrounded() && move != 0) {
            CreateDust();
        }
        
        xDir = move;

    }

    private bool IsGrounded()
    {
        return _colliding.Count > 0;
    }


    private void Update()
    {
        
        // jump buffer code
        if (Input.GetButtonDown("Jump")) {
            _jumpBufferCount = jumpBufferLength;
        }
        else {
            _jumpBufferCount -= Time.deltaTime;
        }

        // left and right movement
        
        // _rigidbody.AddForce(Vector3.forward * (move * speed * Time.deltaTime * 10000));

        // jump 
        if (_jumpBufferCount >= 0 /* && _hangCounter > 0f */ && isMovementEnabled && IsGrounded())
        {
            CreateDust();
            //ScreenShakeController.instance.StartShake(.2f, .1f);

            if (IsOnGravityPlatform())
            {
                _rigidbody.gravityScale *= -1;
            }
            else
            {
                int dir = _rigidbody.gravityScale < 0 ? -1 : 1;
                _rigidbody.AddForce(new Vector2(0, dir * jumpForce), ForceMode2D.Impulse);
                _justJumped = true;
                _jumpBufferCount = 0;
            }
            // Debug.Log("SETTING HANG TIME TO -1");
            // _hangCounter = -1f;
        }   
        
        // short jump
        if (Input.GetButtonUp("Jump") && !IsGrounded() && isMovementEnabled && _justJumped) {
            // _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * .5f); 
            // same thing but more efficient:
            _rigidbody.velocity = Vector2.Scale(_rigidbody.velocity, new Vector2(1f, 0.5f));
            _justJumped = false;
        }   
    }

    private bool IsOnGravityPlatform()
    {
        return _colliding.Any(c => c.gameObject.CompareTag("GravityPlatform"));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Collider2D col = other.collider;
        float colX = col.transform.position.x;
        float charX = transform.position.x;
        float colW = col.bounds.extents.x;
        float charW = _collider.bounds.extents.x;

        if (!col.isTrigger && Abs(charX - colX) < Abs(colW) + Abs(charW) - 0.01f && !col.CompareTag("No Ground"))
        {
            _colliding.Add(col);
            _justJumped = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _colliding.Remove(other.collider);
    }

    private static float Abs(float x)
    {
        return Mathf.Abs(x);
    }
}
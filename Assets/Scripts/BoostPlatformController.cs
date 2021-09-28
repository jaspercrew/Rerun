using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPlatformController : MonoBehaviour
{
    public int _cardinalDir = 0;
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    private float boostForce = 20f;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        
        Color c = Constants.BoostColor;
        c.a = Constants.EnabledOpacity;
        _renderer.color = c;
        // turn on collision
        _collider.isTrigger = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Player")) {
            switch (_cardinalDir) {
                case 0:
                    other.collider.attachedRigidbody.AddForce(new Vector2(0, boostForce), ForceMode2D.Impulse);
                    break;
                case 1:
                    other.collider.attachedRigidbody.AddForce(new Vector2(boostForce, 0), ForceMode2D.Impulse);
                    break;
                case 2:
                    other.collider.attachedRigidbody.AddForce(new Vector2(0, -boostForce), ForceMode2D.Impulse);
                    break;
                case 3:
                    other.collider.attachedRigidbody.AddForce(new Vector2(-boostForce, 0), ForceMode2D.Impulse);
                    break;
                default:
                    Debug.Log("invalid cardinal direction for boosts");
                    break;

            }
            
            
            Color c = Constants.BoostColor;
            c.a = Constants.DisabledOpacity;
            _renderer.color = c;
            // turn off collision
            _collider.isTrigger = true;
        }
    }
}

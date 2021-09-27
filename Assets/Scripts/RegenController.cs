using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RegenController : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();

        // set color faded
        Color c = Constants.RegenColor;
        c.a = Constants.DisabledOpacity;
        _renderer.color = c;
        // turn off collision
        _collider.isTrigger = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        // set color full
        Color c = _renderer.color;
        c.a = Constants.EnabledOpacity;
        _renderer.color = c;
        // turn on collision
        _collider.isTrigger = false;
        GetComponent<ShadowCaster2D>().castsShadows = true;
    }
    
    
    
    
}

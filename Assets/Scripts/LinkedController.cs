using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LinkedController : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    private ShadowCaster2D _shadow;
    public bool isBreakable;
    
    // TODO: store this in the parent, not every single child
    private LinkedController[] _links;
    
    // Start is called before the first frame update
    private void Start() {
        _links = transform.parent.GetComponentsInChildren<LinkedController>();
        
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _shadow = GetComponent<ShadowCaster2D>();

        if (isBreakable) {
            // set color full
            Color c = _renderer.color;
            c.a = Constants.EnabledOpacity;
            _renderer.color = c;
            // turn on collision
            _collider.isTrigger = false;
            _shadow.castsShadows = true;
        }
        else {
            // set color faded
            Color c = Constants.LinkedColor;
            c.a = Constants.DisabledOpacity;
            _renderer.color = c;
            // turn off collision
            _collider.isTrigger = true;
        }
    }

    private void InvertLinks() {
        foreach (LinkedController l in _links) {
            if (l.isBreakable) {
                Color c = _renderer.color;
                c.a = Constants.DisabledOpacity;
                l._renderer.color = c;
                // turn on collision
                l._collider.isTrigger = true;
                l._shadow.castsShadows = false;
            }
            else {
                // set color full
                Color c = _renderer.color;
                c.a = Constants.EnabledOpacity;
                l._renderer.color = c;
                // turn on collision
                l._collider.isTrigger = false;
                l._shadow.castsShadows = true;
                
            }

            l.isBreakable = !l.isBreakable;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        InvertLinks();
    }
    
    private void OnCollisionExit2D() {
        InvertLinks();
    }
    
    
    
    
}
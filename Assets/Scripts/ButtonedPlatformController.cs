using UnityEngine;

public class ButtonedPlatformController : MonoBehaviour
{
    public bool isInitiallyActive = true;
    private Color _groupColor = Constants.ButtonedColor; // default
    private bool _isActive;
    private SpriteRenderer _sr;
    private BoxCollider2D _collider;

    public void SetColor(Color c)
    {
        _groupColor = c;
        c.a = _isActive ? Constants.EnabledOpacity : Constants.DisabledOpacity;
        if (_sr is null)
            _sr = GetComponent<SpriteRenderer>();
        _sr.color = c;
    } 
    
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        
        _isActive = !isInitiallyActive;
        Toggle();
    }

    public void Toggle()
    {
        _isActive = !_isActive;
        Color c = _groupColor;
        c.a = _isActive ? Constants.EnabledOpacity : Constants.DisabledOpacity;
        _sr.color = c;
        _collider.isTrigger = !_isActive;
    }
}

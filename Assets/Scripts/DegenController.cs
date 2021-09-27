using System.Collections;
using UnityEditor;
using UnityEngine;

public class DegenController : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;

    private Vector3 _originalScale;
    private Vector3 _originalPos;
    
    private enum DegenState
    {
        Fragile, Breaking, Broken, Revived
    }

    private DegenState _state;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        // _square = Resources.Load<Sprite>("Sprites/Square.png");
        // Debug.Log(_square);

        _originalScale = transform.localScale;
        _originalPos = transform.position;

        _state = DegenState.Fragile;
        _renderer.color = Constants.DegenColor;
        Revive();
    }
    
    // private IEnumerator WaitAndBreak(float time) {
    //     yield return new WaitForSeconds(time);
    //
    //     if (_state == DegenState.Tired)
    //     {
    //         _renderer.color = Constants.RegenColor;
    //         Kill();
    //         _state = DegenState.Asleep;
    //     }
    // }
    
    private IEnumerator BreakOverTime(float time) {
        // yield return new WaitForSeconds(time);
        //
        // if (_alive)
        // {
        //     Kill();
        //     _alive = false;
        // }
        _state = DegenState.Breaking;
        
        GameObject tempYellow = new GameObject("tempDegen");
        tempYellow.transform.position = this.transform.position;
        tempYellow.transform.position += Vector3.forward; // push into screen
        tempYellow.transform.localScale = this.transform.localScale;
        SpriteRenderer sr = tempYellow.AddComponent<SpriteRenderer>();
        
        UnityEditorInternal.ComponentUtility.CopyComponent(this.GetComponent<SpriteRenderer>());
        UnityEditorInternal.ComponentUtility.PasteComponentValues(sr);
        
        Color c = Constants.RegenColor;
        c.a = Constants.DisabledOpacity;
        sr.color = c;

        const int updates = 100;

        for (float perc = 0; perc < 1; perc += 1f / updates)
        {
            float newHeight = _originalScale.y * (1 - perc);
            float newY = _originalPos.y + perc * _originalScale.y / 2;
            transform.localScale = new Vector3(_originalScale.x, newHeight, _originalScale.z);
            transform.position = new Vector3(_originalPos.x, newY, _originalPos.z);
            yield return new WaitForSeconds(time / updates);
        }

        Destroy(tempYellow);

        _state = DegenState.Broken;
        transform.localScale = _originalScale;
        transform.position = _originalPos;
        _renderer.color = c;
        _collider.isTrigger = true;
    }
    
    // this triggers when the player hits the block
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.CompareTag("Player")) return;
        if (_state != DegenState.Fragile) return;
        StartCoroutine(BreakOverTime(Constants.BreakingTime));
        _state = DegenState.Breaking;
    }

    // this triggers if the player leaves the block early
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if (other.collider.CompareTag("Player") && _state == DegenState.Tired)
    //     {
    //         _renderer.color = Constants.RegenColor;
    //         Kill();
    //         _state = DegenState.Asleep;
    //     }
    // }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _state == DegenState.Broken)
        {
            Revive();
            _state = DegenState.Revived;
        }
    }

    private void Revive()
    {
        // set color full
        Color c = _renderer.color;
        c.a = Constants.EnabledOpacity;
        _renderer.color = c;
        // turn on collision
        _collider.isTrigger = false;
    }

    // private void Kill()
    // {
    //     // set color faded
    //     Color c = _renderer.color;
    //     c.a = Constants.DisabledOpacity;
    //     _renderer.color = c;
    //     // turn off collision
    //     _collider.isTrigger = true;
    // }
}

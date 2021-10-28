using UnityEngine;

public class BreakableController : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;

    private bool _breaking;
    // private float _originalHeight;
    // private float _originalY;
    
    // Start is called before the first frame update
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();

        // _originalHeight = transform.localScale.y;
        // _originalY = transform.position.y;

        _renderer.color = Constants.BreakableColor;
        
        // set color full
        Color c = _renderer.color;
        c.a = Constants.EnabledOpacity;
        _renderer.color = c;
        // turn on collision
        _collider.isTrigger = false;
    }
    
    
    // private IEnumerator BreakOverTime(float time)
    // {
    //     float timer = 0;
    //
    //     while (timer < time)
    //     {
    //         float perc = timer / time;
    //         float newHeight = _originalHeight * (1 - perc);
    //         float newY = _originalY + perc * _originalHeight / 2;
    //         Vector3 scale = transform.localScale;
    //         transform.localScale = new Vector3(scale.x, newHeight, scale.z);
    //         Vector3 pos = transform.position;
    //         transform.position = new Vector3(pos.x, newY, pos.z);
    //
    //         // Debug.Log(timer);
    //         timer += Time.deltaTime;
    //         yield return null;
    //     }
    //     
    //     transform.localScale = Vector3.zero;
    // }

    // this triggers when the player hits the block, 
    // then it will disappear 1 second later
    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (!other.collider.CompareTag("Player")) return;
    //     if (_breaking) return;
    //     StartCoroutine(BreakOverTime(Constants.BreakingTime));
    //     _breaking = true;
    // }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        print(other.collider);
        if (!other.collider.CompareTag("Player")) return;


        if (other.otherCollider is BoxCollider2D) {
            transform.localScale = Vector3.zero;
        }
    }

}

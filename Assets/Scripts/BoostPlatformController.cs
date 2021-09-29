using UnityEngine;

public class BoostPlatformController : MonoBehaviour
{
    [Header("0 = up, 1 = right, 2 = down, 3 = left")]
    [Range(0, 3)]
    public int cardinalDir;
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    private const float BoostForce = 27f;

    // Start is called before the first frame update
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        
        Color c = Constants.BoostColor;
        c.a = Constants.EnabledOpacity;
        _renderer.color = c;
        // turn on collision
        _collider.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.CompareTag("Player")) return;

        Vector2[] dirs = {Vector2.up, Vector2.right, Vector2.down, Vector2.left};
        Vector2 dir = BoostForce * dirs[cardinalDir];
        other.collider.attachedRigidbody.AddForce(dir, ForceMode2D.Impulse);

        // shorter version of this is above
        // switch (cardinalDir) {
        //     case 0:
        //         other.collider.attachedRigidbody.AddForce(new Vector2(0, BoostForce), ForceMode2D.Impulse);
        //         break;
        //     case 1:
        //         other.collider.attachedRigidbody.AddForce(new Vector2(BoostForce, 0), ForceMode2D.Impulse);
        //         break;
        //     case 2:
        //         other.collider.attachedRigidbody.AddForce(new Vector2(0, -BoostForce), ForceMode2D.Impulse);
        //         break;
        //     case 3:
        //         other.collider.attachedRigidbody.AddForce(new Vector2(-BoostForce, 0), ForceMode2D.Impulse);
        //         break;
        //     default:
        //         Debug.Log("invalid cardinal direction for boosts");
        //         break;
        //
        // }
            
            
        Color c = Constants.BoostColor;
        c.a = Constants.DisabledOpacity;
        _renderer.color = c;
        // turn off collision
        _collider.isTrigger = true;
    }
    
}

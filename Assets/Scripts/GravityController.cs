using UnityEngine;

public class GravityController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color c = Constants.GravityColor;
        c.a = Constants.EnabledOpacity;
        sr.color = c;
    }

    // re-implemented in CharController, when jumping
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if (!other.collider.CompareTag("Player")) return;
    //     other.collider.attachedRigidbody.gravityScale *= -1;
    // }
}

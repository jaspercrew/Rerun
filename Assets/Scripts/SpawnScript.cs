using System.Collections;
using UnityEngine;

// TODO: do we need this
public class SpawnScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _portal;
    private GameObject _character;
    private CharController _charController;

    public PhysicsMaterial2D spawnMat;
    public PhysicsMaterial2D charMat;

    private Rigidbody2D _rigidbody2D;

    private SpriteRenderer _spriteRenderer;

    public bool isSpawning;
    //public GameObject _charPrefab;
    
    void Start() {
        _portal = transform.GetChild(0).gameObject;
        _character = GameObject.FindGameObjectWithTag("Player");
        _charController = _character.GetComponent<CharController>();
        _rigidbody2D = _character.GetComponent<Rigidbody2D>();
        _spriteRenderer = _character.GetComponent<SpriteRenderer>();
    }

    public IEnumerator SpawnCharacter() {
        isSpawning = true;
        _charController.isMovementEnabled = false;
        _rigidbody2D.sharedMaterial = spawnMat;

        //Debug.Log("spawn run");
        //Debug.Log(_portal.name);
        
        _portal.SetActive(true);
        _portal.transform.localScale = new Vector3(1.5f, 3, 0); //requires smoothing (aviv)

        _character.transform.position = transform.localPosition + new Vector3(-2, 2, 0);
        _spriteRenderer.enabled = true;
        _rigidbody2D.gravityScale = 2.5f;
        _rigidbody2D.AddForce(new Vector2(3,10), ForceMode2D.Impulse);

        yield return new WaitForSeconds(3.5f);
        
        _portal.transform.localScale = new Vector3(0, 0, 0); //requires smoothing(aviv) 
        _portal.SetActive(false); 
        _rigidbody2D.sharedMaterial = charMat; 
        _charController.isMovementEnabled = true;
        _rigidbody2D.velocity = Vector3.zero;
        isSpawning = false;

        yield return null;
    }


}

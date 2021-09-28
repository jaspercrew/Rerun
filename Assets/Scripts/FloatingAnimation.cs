using UnityEngine;

// TODO: do we need this
public class FloatingAnimation : MonoBehaviour {
    public float amplitude;          //Set in Inspector 
    public float speed;                  //Set in Inspector 
    private float _startY;
    private Vector3 _startPos;

    private void Start () 
    {
        _startPos = transform.position;
        _startY = _startPos.y;
    }

    private void Update () 
    {        
        _startPos.y = _startY + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = _startPos;
    }
}

using System;
using UnityEngine;

public class PortalPlatformController : MonoBehaviour {
    private const float BoostVelocity = .5f;
    private Transform _otherPortalTransform;
    [Range(0, 3)] public int cardinalDir;

    // Start is called before the first frame update
    private void Start() {
        Transform[] topParentArray = transform.parent.parent.GetComponentsInChildren<Transform>();
        
        // TODO: wtf is this
        _otherPortalTransform = (topParentArray[1] == transform.parent)?
            topParentArray[5].GetChild(0) : topParentArray[1].GetChild(0);
            
        //Debug.Log(_topParentArray.Length);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(otherPortalTransform);
        //Debug.Log(transform.parent.parent);
        //Debug.Log("trigger");
        if (!other.CompareTag("Player") || !other.gameObject.GetComponent<BoxCollider2D>().enabled) return;
        
        //Debug.Log("trigger if");
        Vector2 inVel = other.attachedRigidbody.velocity;
        //int sign = Math.Sign(inVel.y);
        Transform charTransform = other.transform;
        switch (_otherPortalTransform.gameObject.GetComponent<PortalPlatformController>().cardinalDir) {
            case 0: //up - TODO: needs fix when entering from stood up portal
                charTransform.position = _otherPortalTransform.position + new Vector3(0, 1, 0);
                other.attachedRigidbody.velocity = 
                    new Vector2(0,Math.Max(BoostVelocity + Math.Abs(inVel.y), 10f)); 
                break;
            case 1: //right
                charTransform.position = _otherPortalTransform.position + new Vector3(1, 0, 0);
                other.attachedRigidbody.velocity = new Vector2(Math.Abs(inVel.y) + BoostVelocity, 0);
                break;
            case 2: //down - TODO: needs fix when entering from stood up portal
                charTransform.position = _otherPortalTransform.position + new Vector3(0, -1, 0);
                other.attachedRigidbody.velocity = 
                    new Vector2(0,Math.Min(-(BoostVelocity + Math.Abs(inVel.y)), -10f)); 
                break;
            case 3: //left
                charTransform.position = _otherPortalTransform.position + new Vector3(-1, 0, 0);
                other.attachedRigidbody.velocity = new Vector2(-Math.Abs(inVel.y) - BoostVelocity, 0);
                break;
            default:
                Debug.Log("invalid cardinal direction for portal");
                break;
        }
    }
}

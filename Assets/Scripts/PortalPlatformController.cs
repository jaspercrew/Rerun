using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PortalPlatformController : MonoBehaviour {
    private float boostVelocity = .5f;
    private Transform otherPortalTransform;
    public int _cardinalDir = 0;
    
    

    // Start is called before the first frame update
    void Start() {
        Transform[] _topParentArray = transform.parent.parent.GetComponentsInChildren<Transform>();
        
        
            otherPortalTransform = _topParentArray[1] == transform.parent
            ? _topParentArray[5].GetChild(0)
            : _topParentArray[1].GetChild(0);
            
        //Debug.Log(_topParentArray.Length);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(otherPortalTransform);
        //Debug.Log(transform.parent.parent);
        //Debug.Log("trigger");
        if (other.CompareTag("Player") && other.gameObject.GetComponent<BoxCollider2D>().enabled) {
            //Debug.Log("trigger if");
            Vector2 inVel = other.attachedRigidbody.velocity;
            //int sign = Math.Sign(inVel.y);
            Transform charTransform = other.transform;
            switch (otherPortalTransform.gameObject.GetComponent<PortalPlatformController>()._cardinalDir) {
                case 0: //up - needs fix when entering from stood up portal
                    charTransform.position = otherPortalTransform.position + new Vector3(0, 1, 0);
                    other.attachedRigidbody.velocity =  new Vector2(0,Math.Max(boostVelocity + Math.Abs(inVel.y), 10f)); 
                    break;
                case 1: //right
                    charTransform.position = otherPortalTransform.position + new Vector3(1, 0, 0);
                    other.attachedRigidbody.velocity = new Vector2(Math.Abs(inVel.y) + boostVelocity, 0);
                    break;
                case 2: //down needs fix when entering from stood up portal
                    charTransform.position = otherPortalTransform.position + new Vector3(0, -1, 0);
                    other.attachedRigidbody.velocity = new Vector2(0,Math.Min(-(boostVelocity + Math.Abs(inVel.y)), -10f)); 
                    break;
                case 3: //left
                    charTransform.position = otherPortalTransform.position + new Vector3(-1, 0, 0);
                    other.attachedRigidbody.velocity = new Vector2(-Math.Abs(inVel.y) - boostVelocity, 0);
                    break;
                default:
                    Debug.Log("invalid cardinal direction for portal");
                    break;
            }
            

            
            
        }
    }
}

using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private PortalController _pc;
    
    private void Start()
    {
        _pc = FindObjectOfType<PortalController>();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<BoxCollider2D>().enabled)
        {
            _pc.RestartLevel();
        }
    }
}

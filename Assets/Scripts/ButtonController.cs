using UnityEngine;
using UnityEngine.Assertions;

public class ButtonController : MonoBehaviour
{
    public Color groupColor;
    
    private ButtonedPlatformController[] _platforms;
    private bool _used;
    
    // Start is called before the first frame update
    private void Start()
    {
        Transform platformsParent = transform.parent.parent.GetChild(1);
        Assert.AreEqual("Platforms", platformsParent.gameObject.name);

        _platforms = new ButtonedPlatformController[platformsParent.childCount];

        for (int i = 0; i < _platforms.Length; i++)
        {
            _platforms[i] = platformsParent.GetChild(i).GetComponent<ButtonedPlatformController>();
            _platforms[i].SetColor(groupColor);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (_used) return;

        _used = true;
        
        transform.localScale = new Vector3(1, 0.1f, 1);
        foreach (ButtonedPlatformController b in _platforms)
        {
            b.Toggle();
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour {
    private FilmGrain _fg;
    
    // Start is called before the first frame update
    private void Start() {
        Volume vol = GetComponent<Volume>();
        if (!vol.TryGetComponent(out _fg))
            Debug.LogError("CHARLIE BUG - no film grain found; continuing without film grain");
    }

    public void EnableFilmGrain() {
        _fg.active = true;
    }
    
    public void DisableFilmGrain() {
        _fg.active = false;
    }

}

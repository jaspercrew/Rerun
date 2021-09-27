using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour {
    private Volume _volume;
    // Start is called before the first frame update
    void Start() {
        _volume = GetComponent<Volume>();
    }

    public void EnableFilmGrain() {
        FilmGrain _fg;
        _volume.profile.TryGet<FilmGrain>(out _fg);
        _fg.active = true;
    }
    
    public void DisableFilmGrain() {
        FilmGrain _fg;
        _volume.profile.TryGet<FilmGrain>(out _fg);
        _fg.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

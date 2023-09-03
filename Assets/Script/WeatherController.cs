using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [Header("Materials for different weather effects")]
    [SerializeField] Material snowMaterial;
    [SerializeField] Material rainMaterial;

    private new ParticleSystem particleSystem;
    
    void Awake(){
        particleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    #region Start Weather Effects
    public void DeactivateWeather(){
        gameObject.SetActive(false);
    }

    public void StartSnow(){
        gameObject.SetActive(true);
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = snowMaterial;
        var main = particleSystem.main;
        main.startLifetime = 3f;
        main.gravityModifier = 1f;
        particleSystem.Play();
    }

    public void StartRain(){
        gameObject.SetActive(true);
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = rainMaterial;
        var main = particleSystem.main;
        main.startLifetime = 1f;
        main.gravityModifier = 2f;
        particleSystem.Play();
    }

    #endregion

}
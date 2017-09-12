using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerticleExplosion : MonoBehaviour {

    private ParticleSystem particle;

    // Use this for initialization
    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
        particle.Stop();
    }
    public void Explosion(Vector3 pos)
    {
        this.gameObject.transform.position = pos;
        particle.Play();
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    internal LineRenderer line;
    internal ParticleSystem startParticles;
    internal ParticleSystem endParticles;

    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        startParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        endParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    internal void ResetLaset()
    {
        line.positionCount = 1;
        startParticles.gameObject.SetActive(false);
        endParticles.gameObject.SetActive(false);
    }

    internal void SetScale(float scale)
    {
        startParticles.transform.localScale = Vector3.one * scale * 5;
        endParticles.transform.localScale = Vector3.one * scale * 5;
        startParticles.startSize = scale;
        endParticles.startSize = scale;
        line.startWidth = scale;
        line.endWidth = scale;
    }
}

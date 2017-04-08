using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BallExplosionAutoDisable : MonoBehaviour
{

    public bool m_AnimationFinished;

    void Awake()
    {
        m_AnimationFinished = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_AnimationFinished)
        {
            m_AnimationFinished = false;
            gameObject.SetActive(false);
        }

    }
}

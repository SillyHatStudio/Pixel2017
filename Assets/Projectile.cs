using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public ParticleSystem m_ParticleSystem;    
    private Vector3 m_Direction;

    private bool m_Shoot = false;
    private float m_Speed = 0;
    private float m_LivingTime = 1;
    private float m_Distance = 0;
    private bool m_ExplosionDone = false;
    private int m_PlayerId = -1;



	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(m_Shoot)
        {

            transform.position += m_Direction * m_Speed * Time.deltaTime;

            m_LivingTime -= Time.deltaTime;
            if (m_LivingTime<= 0)
            {
                Reset();                
                gameObject.SetActive(false);
            }
           
            if(m_LivingTime < .7f && !m_ExplosionDone)
            {
                GameObject explosion = PoolManager.instance.GetPoolObject("BallExplosion");
                explosion.transform.position = transform.position;
                explosion.GetComponent<ParticleSystem>().startColor = (m_PlayerId == 0) ? Color.white : Color.black;
                m_ExplosionDone = true;
            }


        }	
	}

    public void Reset()
    {
         
        m_Speed = 0;
        m_LivingTime = 1;
       
        m_Distance = 0;
        m_Shoot = false;
        m_ExplosionDone = false;
        m_PlayerId = -1;
    }

    public void Shoot(Vector3 _startPosition, Vector3 _endingPosition, float _travelingTime, int _playerID)
    {
        m_Direction = (_endingPosition - _startPosition).normalized;
        float distance = (_endingPosition - _startPosition).magnitude;
        float speed = distance / _travelingTime;

        m_Distance = distance;
        m_Speed = speed;
        m_PlayerId = _playerID;

        Color color = (_playerID == 0) ? Color.white : Color.black;
        m_ParticleSystem.startColor = color;

        m_Shoot = true;


    }

    



}

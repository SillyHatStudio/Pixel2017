using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMoveTrack : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, .2f);
        bool collideWithMovableObject = false;
        for(int i = 0; i < col.Length; i++)
        {
            if((col[i].GetComponent<MovingCube>()))
            {
                if (col[i].GetComponent<MovingCube>().enabled)
                {
                    collideWithMovableObject = true;
                }
            }
             
        }

        if(!collideWithMovableObject)
        {
            gameObject.layer = LayerMask.NameToLayer("Grey");
        }
    }
}

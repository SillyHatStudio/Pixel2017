using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Collider2D[] col;

    // Use this for initialization
    void Start ()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
        col = Physics2D.OverlapPointAll(gameObject.transform.position);
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].GetComponent<CubeBehaviour>())
            {
                transform.position = col[i].transform.position + new Vector3(0,0,.5f);
            }
        }

	}
}

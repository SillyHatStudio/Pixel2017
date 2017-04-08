using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : TriggerEntryPoint
{
    public override void TriggerAction()
    {
        gameObject.SetActive(false);
        //throw new NotImplementedException();
    }




    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

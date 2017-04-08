using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to activate an exit zone on button trigger
public class ExitZoneTriggerable : TriggerEntryPoint {

    private ExitCube m_ExitCubeComponent;

    void Awake()
    {
        m_ExitCubeComponent = GetComponent<ExitCube>();

        if (m_ExitCubeComponent && m_ExitCubeComponent.enabled == true)
        {
            m_ExitCubeComponent.enabled = false;
        }
    }

    //Action = enable the exit zone
    public override void TriggerAction()
    {
        if (m_ExitCubeComponent && m_ExitCubeComponent.enabled == false)
        {
            m_ExitCubeComponent.enabled = true;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

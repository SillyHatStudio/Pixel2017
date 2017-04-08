using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Test class
public class TestTriggerable : TriggerEntryPoint
{

    public override void TriggerAction()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
}

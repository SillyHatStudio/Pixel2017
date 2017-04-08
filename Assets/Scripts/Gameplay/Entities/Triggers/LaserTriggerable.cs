using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTriggerable : TriggerEntryPoint {

    public override void TriggerAction()
    {
        if(GetComponent<LaserCaster>())
            GetComponent<LaserCaster>().enabled = true;

        if (GetComponent<MovingCube>())
            GetComponent<MovingCube>().enabled = true;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TriggerEntryPoint : MonoBehaviour, ITriggerEntryPoint {

    
    public List<Button> m_LinkedButtons;

    void Awake()
    {
        if(m_LinkedButtons == null)
            m_LinkedButtons = new List<Button>();
    }

    // Use this for initialization
    void Start () {
		
	}

    public virtual void RegisterButton(Button btn)
    {
        m_LinkedButtons.Add(btn);
    }

    public abstract void TriggerAction();

    public virtual bool CheckAllButtonsValid()
    {
        if(m_LinkedButtons.All(btn => btn.m_Validated)){
            Debug.Log("All buttons validated for object : " + gameObject.name);
            return true;
        }

        return false;
    }

    public virtual void LockAllButtons()
    {
        foreach(var btn in m_LinkedButtons)
        {
            btn.m_Locked = true;
        }
    }
}

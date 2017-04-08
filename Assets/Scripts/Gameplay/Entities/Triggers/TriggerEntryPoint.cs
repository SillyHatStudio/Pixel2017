using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TriggerEntryPoint : MonoBehaviour, ITriggerEntryPoint {

    public List<Button> m_LinkedButtons;

    void Awake()
    {
        m_LinkedButtons = new List<Button>();
    }

    // Use this for initialization
    void Start () {
		
	}

    public void RegisterButton(Button btn)
    {
        m_LinkedButtons.Add(btn);
    }

    public virtual void TriggerAction() { }

    public bool CheckAllButtonsValid()
    {
        return m_LinkedButtons.All(btn => btn.m_Validated);
    }

    public void LockAllButtons()
    {
        foreach(var btn in m_LinkedButtons)
        {
            btn.m_Locked = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCube : CubeBehaviour {

    public int m_NumberOfPlayersRequiredInside = 1;
    private int m_CurrentNumberOfPlayersInside;
    public bool validated;

    [HideInInspector]
    public GameObject MapManager;
    
    [HideInInspector]
    public enum PlayerAuthorized
    {
        Any,
        P1,
        P2
    }

    public PlayerAuthorized m_PlayersThatCanGoInside;


    protected override void Awake()
    {
        base.Awake();
        m_CanColor = false;
        m_CurrentNumberOfPlayersInside = 0;
        m_Visual.layer = LayerMask.NameToLayer("ExitZone");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // Use this for initialization
    public void Start () {
		
	}

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            m_CurrentNumberOfPlayersInside++;

            if(m_NumberOfPlayersRequiredInside == m_CurrentNumberOfPlayersInside)
            {

                //If any player inside the zone, ok 
                if(m_PlayersThatCanGoInside == PlayerAuthorized.Any)
                {
                    validated = true;
                }

                //If a specific player is inside the zone
                else
                {
                    if (m_PlayersThatCanGoInside == PlayerAuthorized.P1 && col.gameObject.GetComponent<Player>().GetPlayerNumberEnumValue() == EnumTypes.PlayerEnum.P1
                        || m_PlayersThatCanGoInside == PlayerAuthorized.P2 && col.gameObject.GetComponent<Player>().GetPlayerNumberEnumValue() == EnumTypes.PlayerEnum.P2)
                    {
                        validated = true;
                    }
                }
                
            }
        }
    }

    public void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            m_CurrentNumberOfPlayersInside--;

            if (validated)
                validated = false;
        }
    }


}

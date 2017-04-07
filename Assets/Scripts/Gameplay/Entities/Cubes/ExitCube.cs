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
        m_Visual.layer = LayerMask.NameToLayer("Exit");
       // m_IsFlippable = false;

        //GetComponent<BoxCollider2D>().isTrigger = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // Use this for initialization
    public void Start () {
		
	}

    public void CheckPlayerCollisionIn(GameObject obj)
    {
        if (obj.tag.Equals("Player"))
        {
            m_CurrentNumberOfPlayersInside++;
            Debug.Log("Player num = " + obj.GetComponent<Player>().GetPlayerNumberEnumValue());

            if (m_NumberOfPlayersRequiredInside == m_CurrentNumberOfPlayersInside)
            {

                //If any player inside the zone, ok 
                if (m_PlayersThatCanGoInside == PlayerAuthorized.Any)
                {
                    validated = true;
                    Debug.Log("Validated : all players are in the zone (condition = any)");
                }

                //If a specific player is inside the zone
                else
                {
                    if ((m_PlayersThatCanGoInside == PlayerAuthorized.P1 && obj.GetComponent<Player>().GetPlayerNumberEnumValue() == EnumTypes.PlayerEnum.P1)
                        || (m_PlayersThatCanGoInside == PlayerAuthorized.P2 && obj.GetComponent<Player>().GetPlayerNumberEnumValue() == EnumTypes.PlayerEnum.P2))
                    {
                        validated = true;
                        Debug.Log("Validated : specific player is in the zone (condition = "+ m_PlayersThatCanGoInside+")");
                    }
                }

            }
        }
    }

    public void CheckPlayerCollisionOut(GameObject obj)
    {
        if (obj.tag.Equals("Player"))
        {
            m_CurrentNumberOfPlayersInside--;

            if (validated)
                validated = false;
        }
    }


}

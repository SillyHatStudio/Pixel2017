using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    public bool m_Validated;

    [HideInInspector]
    public enum PlayerAuthorized
    {
        Any,
        P1,
        P2
    }

    public PlayerAuthorized m_PlayerAllowed;

    public List<GameObject> objectsToTrigger;

    public bool m_Locked;

    protected void Awake()
    {
        m_Validated = false;
        m_Locked = false;

        foreach (var triggerableObj in objectsToTrigger)
        {
            if (triggerableObj.GetComponent<ITriggerEntryPoint>() != null)
            {
                triggerableObj.GetComponent<ITriggerEntryPoint>().RegisterButton(this);
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        bool triggerAction = false;

        if(!m_Locked)
        {
            if (col.gameObject.tag.Equals("Player"))
            {
                var player = col.gameObject.GetComponent<Player>();

                Debug.Log("Player num = " + player.GetPlayerNumberEnumValue());

                //If any player is on the button
                if (m_PlayerAllowed == PlayerAuthorized.Any)
                {
                    m_Validated = true;
                    Debug.Log("Button validated (any player on the button)");
                    triggerAction = true;
                }

                //If a specific player is on the button
                else
                {
                    if ((m_PlayerAllowed == PlayerAuthorized.P1 && player.GetPlayerNumberEnumValue() == EnumTypes.PlayerEnum.P1)
                        || (m_PlayerAllowed == PlayerAuthorized.P2 && player.GetPlayerNumberEnumValue() == EnumTypes.PlayerEnum.P2))
                    {
                        m_Validated = true;
                        Debug.Log("Button validated : specific player is in the zone (condition = " + m_PlayerAllowed + ")");
                        triggerAction = true;
                    }
                }
            }

            if (triggerAction)
            {
                foreach (var triggerableObj in objectsToTrigger)
                {
                    if (triggerableObj.GetComponent<ITriggerEntryPoint>() != null)
                    {
                        if (triggerableObj.GetComponent<ITriggerEntryPoint>().CheckAllButtonsValid())
                        {
                            triggerableObj.GetComponent<ITriggerEntryPoint>().TriggerAction();
                            triggerableObj.GetComponent<ITriggerEntryPoint>().LockAllButtons();
                        }
                    }
                }
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D col)
    {
        if(!m_Locked)
        {
            if (col.gameObject.tag.Equals("Player"))
            {
                var player = col.gameObject.GetComponent<Player>(); ;
                if (m_PlayerAllowed == PlayerAuthorized.Any)
                {
                    if (m_Validated)
                        m_Validated = false;
                    Debug.Log("Button unvalidated (any player : left the button)");
                }

                else if ((m_PlayerAllowed == PlayerAuthorized.P1 && player.GetPlayerNumberEnumValue() == EnumTypes.PlayerEnum.P1)
                        || (m_PlayerAllowed == PlayerAuthorized.P2 && player.GetPlayerNumberEnumValue() == EnumTypes.PlayerEnum.P2))
                {
                    if (m_Validated)
                        m_Validated = false;

                    Debug.Log("Button unvalidated " + m_PlayerAllowed + " left the button");
                }
            }
        }
    }

    // Use this for initialization
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {

    }
}

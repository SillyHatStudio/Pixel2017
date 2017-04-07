using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;

//[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : Entity
{
    public InputDevice Device { get; set; }
    private EnumTypes.PlayerEnum m_PlayerNumber;
    private Rigidbody2D m_Rigidbody;
    public float Speed;

    //ref
    public GameObject m_Visual;
    public GameObject m_Target;

    private bool m_RightStickWasPressed = false;
    private float m_BumpingMinimumTime = .2f;
    private float m_BumpingMinimumTimer = 0;



    //private KeyCode m_ControlKeyUp, m_ControlKeyDown, m_ControlKeyLeft, m_ControlKeyRight;

    protected virtual void Awake()
    {
        m_PlayerNumber = EnumTypes.PlayerEnum.Unassigned;
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        base.Start();
    }

    void Update()
    {
        Control();
        BumpingCooldowmUpdate();
        CheckTypeOfFloorObject();
    }

    private void BumpingCooldowmUpdate()
    {
        if (m_BumpingMinimumTimer > 0)
        {
            m_BumpingMinimumTimer -= Time.deltaTime;
            if (m_BumpingMinimumTimer < 0)
            {
                m_BumpingMinimumTimer = 0;
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D col) { }

    private void Control()
    {
        if (Device.LeftStick.Vector != Vector2.zero)
        {
            m_Rigidbody.velocity = Device.LeftStick.Vector * Time.deltaTime * Speed;
        }
        else
        {
            m_Rigidbody.velocity = m_Rigidbody.velocity *= .8f;
        }

        if (Device.RightStick.Vector != Vector2.zero && m_BumpingMinimumTimer == 0)
        {
            m_BumpingMinimumTimer = m_BumpingMinimumTime;

            float angle = Device.RightStick.Angle;
            if (angle >= 315 || angle < 45)
            {
                CheckIfCanMove(m_Target.transform.position + Vector3.up);
            }
            else if (angle < 135)
            {
                CheckIfCanMove(m_Target.transform.position + Vector3.right);
            }
            else if (angle < 225)
            {
                CheckIfCanMove(m_Target.transform.position + Vector3.down);
            }
            else if (angle < 315)
            {
                CheckIfCanMove(m_Target.transform.position + Vector3.left);
            }
        }

        if (Device.RightTrigger.WasPressed)//hit trigger and cast a cross on the game
        {
            CastCross();
        }
    }

    //Do a raycast to a possible position to teleport the object to location
    private void CheckIfCanMove(Vector3 _position)
    {
        Collider2D[] collider = Physics2D.OverlapPointAll(_position);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].GetComponent<CubeBehaviour>())
            {
                m_Target.transform.position = collider[i].transform.position;

            }
            else
            {

            }
        }
    }

    private void CheckTypeOfFloorObject()
    {
        /* TODO */
    }

    private void CastCross()
    {
        CastBox(m_Target.transform.position);
        CastBox(m_Target.transform.position + Vector3.up);
        CastBox(m_Target.transform.position + Vector3.left);
        CastBox(m_Target.transform.position + Vector3.down);
        CastBox(m_Target.transform.position + Vector3.right);
    }

    private void CastBox(Vector2 _position)
    {
        Collider2D[] col = Physics2D.OverlapPointAll(_position);
        Collider2D[] colBelow = Physics2D.OverlapPointAll(gameObject.transform.position);
        GameObject boxBelow = null;

        for (int i = 0; i < colBelow.Length; i++)
        {
            if (colBelow[i].GetComponent<CubeBehaviour>())
            {
                colBelow[i].GetComponent<CubeBehaviour>().m_CanColor = false;
                boxBelow = colBelow[i].gameObject;
            }
        }


        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.layer == 8 || col[i].gameObject.layer == 9 || col[i].gameObject.layer == 10)
            {
                if (col[i].GetComponent<ExitCube>().enabled == false)
                {
                    if (col[i].GetComponent<CubeBehaviour>())
                    {
                        if (col[i].GetComponent<CubeBehaviour>().m_CanColor)
                        {
                            if (m_PlayerNumber == EnumTypes.PlayerEnum.P1)
                            {
                                col[i].GetComponent<CubeBehaviour>().SetMaterialColor(Color.white);
                            }
                            else if (m_PlayerNumber == EnumTypes.PlayerEnum.P2)
                            {
                                col[i].GetComponent<CubeBehaviour>().SetMaterialColor(Color.black);
                            }
                        }
                    }
                }

            }
        }

        if (boxBelow != null)
        {
            boxBelow.GetComponent<CubeBehaviour>().m_CanColor = true;
        }

    }

    //Set number and color
    public void SetPlayerNumber(int number)
    {
        switch (number)
        {
            case 0:
                m_PlayerNumber = EnumTypes.PlayerEnum.P1;
                m_Visual.GetComponent<MeshRenderer>().material.color = Color.black;
                m_Visual.layer = LayerMask.NameToLayer("Black");
                break;

            case 1:
                m_PlayerNumber = EnumTypes.PlayerEnum.P2;
                m_Visual.GetComponent<MeshRenderer>().material.color = Color.white;
                m_Visual.layer = LayerMask.NameToLayer("White");
                break;
        }
    }

    public int GetPlayerNumber()
    {
        return (int)m_PlayerNumber;
    }

    public EnumTypes.PlayerEnum GetPlayerNumberEnumValue()
    {
        return m_PlayerNumber;
    }


}
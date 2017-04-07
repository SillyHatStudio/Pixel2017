using UnityEngine;
using System.Collections;
using InControl;

//[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : Entity
{
    public InputDevice Device { get; set; }
    private EnumTypes.PlayerEnum m_PlayerNumber;
    private Rigidbody2D m_Rigidbody;
    public float Speed;
    public GameObject m_Target;
    private float m_BaseSpeed = 100f;
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

        if (Device.RightStick.Vector != Vector2.zero)
        {
            m_Target.GetComponent<Rigidbody2D>().velocity = Device.RightStick.Vector * Time.deltaTime * 200;
        }
        else
        {
            m_Target.GetComponent<Rigidbody2D>().velocity = m_Target.GetComponent<Rigidbody2D>().velocity *= .8f;
        }
    }

    public void SetPlayerNumber(int number)
    {
        /*switch (number)
        {
            case 0:
                m_PlayerNumber = EnumTypes.PlayerEnum.P1;
                m_ControlKeyUp = KeyCode.W;
                m_ControlKeyDown = KeyCode.S;
                m_ControlKeyLeft = KeyCode.A;
                m_ControlKeyRight = KeyCode.D;
                playerColor = Color.red;
                break;

            case 1:
                m_PlayerNumber = EnumTypes.PlayerEnum.P2;
                m_ControlKeyUp = KeyCode.UpArrow;
                m_ControlKeyDown = KeyCode.DownArrow;
                m_ControlKeyLeft = KeyCode.LeftArrow;
                m_ControlKeyRight = KeyCode.RightArrow;
                playerColor = Color.blue;
                break;

            case 2:
                m_PlayerNumber = EnumTypes.PlayerEnum.P3;
                m_ControlKeyUp = KeyCode.Y;
                m_ControlKeyDown = KeyCode.H;
                m_ControlKeyLeft = KeyCode.G;
                m_ControlKeyRight = KeyCode.J;
                playerColor = Color.white;
                break;

            case 3:
                m_PlayerNumber = EnumTypes.PlayerEnum.P4;
                m_ControlKeyUp = KeyCode.O;
                m_ControlKeyDown = KeyCode.L;
                m_ControlKeyLeft = KeyCode.K;
                m_ControlKeyRight = KeyCode.Semicolon;
                playerColor = Color.green;
                break;
        }*/
    }

    public int GetPlayerNumber()
    {
        return (int)m_PlayerNumber;
    }

}
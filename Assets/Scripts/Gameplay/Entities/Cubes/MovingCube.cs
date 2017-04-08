using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : CubeBehaviour
{
    private Vector3 m_StartPosition, m_EndPosition;
    public float m_MovementSpeed;
    private bool m_isMoving;
    public MovementDirection m_MovementDirection;
    public float m_distanceToMove;
    public bool m_LoopMovement;
    private Vector3 m_Direction;
    private bool m_Going = true;


   //Startposition, movedistance, speed, loop


    [HideInInspector]
    public enum MovementDirection
    {
        Up,
        Down,
        Left,
        Right
    };

    protected override void Awake()
    {
        //base.Awake();

        m_StartPosition = transform.position;

        if (m_MovementDirection == MovementDirection.Left)
        {
            m_EndPosition = new Vector3(m_StartPosition.x - m_distanceToMove, m_StartPosition.y, m_StartPosition.z);
            m_Direction = Vector3.left;
        }

        if (m_MovementDirection == MovementDirection.Right)
        {
            m_EndPosition = new Vector3(m_StartPosition.x + m_distanceToMove, m_StartPosition.y, m_StartPosition.z);
            m_Direction = Vector3.right;
        }

        if (m_MovementDirection == MovementDirection.Up)
        {
            m_EndPosition = new Vector3(m_StartPosition.x, transform.position.y + m_distanceToMove, m_StartPosition.z);
            m_Direction = Vector3.up;
        }

        if (m_MovementDirection == MovementDirection.Down)
        {
            m_EndPosition = new Vector3(m_StartPosition.x, m_StartPosition.y - m_distanceToMove, m_StartPosition.z);
            m_Direction = Vector3.down;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update();


        if (m_LoopMovement)
        {
            if(m_Going)
            {
                transform.position += m_Direction * Time.deltaTime * m_MovementSpeed;
                switch (m_MovementDirection)
                {
                    case MovementDirection.Up:
                        if (transform.position.y >= m_EndPosition.y)
                        {
                            transform.position = m_EndPosition;
                            m_Going = false;
                            m_Direction *= -1;
                        }
                        break;
                    case MovementDirection.Down:
                        if (transform.position.y <= m_EndPosition.y)
                        {
                            transform.position = m_EndPosition;
                            m_Going = false;
                            m_Direction *= -1;
                        }
                        break;
                    case MovementDirection.Left:
                        if(transform.position.x <= m_EndPosition.x)
                        {
                            transform.position = m_EndPosition;
                            m_Going = false;
                            m_Direction *= -1;
                        }
                        break;
                    case MovementDirection.Right:
                        if (transform.position.x >= m_EndPosition.x)
                        {
                            transform.position = m_EndPosition;
                            m_Going = false;
                            m_Direction *= -1;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                transform.position += m_Direction * Time.deltaTime * m_MovementSpeed;
                switch (m_MovementDirection)
                {
                    case MovementDirection.Up:
                        if (transform.position.y <= m_StartPosition.y)
                        {
                            transform.position = m_StartPosition;
                            m_Going = true;
                            m_Direction *= -1;
                        }
                        break;
                    case MovementDirection.Down:
                        if (transform.position.y >= m_StartPosition.y)
                        {
                            transform.position = m_StartPosition;
                            m_Going = true;
                            m_Direction *= -1;
                        }
                        break;
                    case MovementDirection.Left:
                        if (transform.position.x >= m_StartPosition.x)
                        {
                            transform.position = m_StartPosition;
                            m_Going = true;
                            m_Direction *= -1;
                        }
                        break;
                    case MovementDirection.Right:
                        if (transform.position.x <= m_StartPosition.x)
                        {
                            transform.position = m_StartPosition;
                            m_Going = true;
                            m_Direction *= -1;
                        }
                        break;
                    default:
                        break;
                }
            }
            
            //if (m_Going)
            //{
            //    transform.position += m_Direction * m_MovementSpeed * Time.deltaTime;
            //    float distanceTraveled = Vector2.Distance(transform.position, m_StartPosition);
            //    if (distanceTraveled < m_distanceToMove)
            //    {
            //        distanceTraveled = 0;
            //        m_Going = false;
            //        m_Direction *= -1;
            //        transform.position = m_EndPosition;
            //    }
            //}
            ////transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.time * m_MovementSpeed, 1f)));
            //if (m_Going)
            //{
            //    transform.position += m_Direction * m_MovementSpeed * Time.deltaTime;
            //    float distanceTraveled = Vector2.Distance(transform.position, m_StartPosition);
            //    if (distanceTraveled < m_distanceToMove)
            //    {
            //        distanceTraveled = 0;
            //        m_Going = false;
            //        m_Direction *= -1;
            //        transform.position = m_EndPosition;
            //    }
            //}
            //else
            //{
            //    transform.position += m_Direction * m_MovementSpeed * Time.deltaTime;
            //    float distanceTraveled = Vector2.Distance(transform.position, m_EndPosition);
            //    if (distanceTraveled < m_distanceToMove)
            //    {
            //        distanceTraveled = 0;
            //        m_Going = true;
            //        m_Direction *= -1;
            //        transform.position = m_StartPosition;
            //    }
            //}


        }

        else
        {
            transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, Mathf.SmoothStep(0f, 1f, Time.time / m_MovementSpeed));
        }
    }
}

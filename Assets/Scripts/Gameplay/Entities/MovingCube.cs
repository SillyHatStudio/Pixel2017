using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : CubeBehaviour
{
    private Vector2 m_StartPosition, m_EndPosition;
    public float m_MovementSpeed;
    private bool m_isMoving;
    public MovementDirection m_MovementDirection;
    public float m_distanceToMove;

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
        base.Awake();

        m_StartPosition = transform.position;

        if (m_MovementDirection == MovementDirection.Left)
        {
            m_EndPosition = new Vector2(m_StartPosition.x - m_distanceToMove, m_StartPosition.y);
        }

        if (m_MovementDirection == MovementDirection.Right)
        {
            m_EndPosition = new Vector2(m_StartPosition.x + m_distanceToMove, m_StartPosition.y);
        }

        if (m_MovementDirection == MovementDirection.Up)
        {
            m_EndPosition = new Vector2(m_StartPosition.x, transform.position.y + m_distanceToMove);
        }

        if (m_MovementDirection == MovementDirection.Down)
        {
            m_EndPosition = new Vector2(m_StartPosition.x, m_StartPosition.y - m_distanceToMove);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.time / m_MovementSpeed, 1f)));
    }
}

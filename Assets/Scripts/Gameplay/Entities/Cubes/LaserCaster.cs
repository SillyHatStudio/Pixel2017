using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserCaster : MonoBehaviour
{

    private float m_MaxProjectionDistance = 20f;
    private Vector2 m_LaserDirection;
    private GameObject m_LastHitObject;
    private LineRenderer m_LineRenderer;
    private Vector2 m_EndPosition;
    private Vector2 m_CasterToTargetProject;

    [HideInInspector]
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    public GameObject parentWall; //wall to which the caster is linked
    public Direction direction;
    public float rayWidth;
    public Color rayColor;

    void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.startWidth = rayWidth;
        m_LineRenderer.endWidth = rayWidth;
        m_LineRenderer.startColor = rayColor;
        m_LineRenderer.endColor = rayColor;

        if (direction == Direction.Left)
        {
            m_LaserDirection = Vector2.left;
        }

        if (direction == Direction.Right)
        {
            m_LaserDirection = Vector2.right;
        }

        if (direction == Direction.Up)
        {
            m_LaserDirection = Vector2.up;
        }

        if (direction == Direction.Down)
        {
            m_LaserDirection = Vector2.down;
        }

        if (parentWall)
            m_LastHitObject = parentWall;
    }

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPosition = transform.position;

        var beginPoint = currentPosition + m_LaserDirection * (transform.localScale.x / 2f);

        m_LineRenderer.SetPosition(0, beginPoint);


        //Check for any obstacles between the caster and the wall
        RaycastHit2D hit = Physics2D.Raycast(currentPosition + m_LaserDirection * (transform.localScale.x / 2f), m_LaserDirection, m_MaxProjectionDistance);
        if (hit)
        {
            if (hit.collider.gameObject.tag.Equals("Player"))
            {
                //TODO when a player is touched by laser kill him
            }

            else
            {
                if (hit.collider.gameObject != m_LastHitObject)
                {
                    var newTarget = hit.collider.gameObject;
                    Debug.Log("New object hit : " + newTarget.name);

                    m_LastHitObject = newTarget;
                }


                float halfScale = m_LastHitObject.transform.localScale.x / 2f;

                //use y scale to move laser out of the transform if hitting from top or bottom
                if (m_LaserDirection == Vector2.up || m_LaserDirection == Vector2.down)
                {
                    halfScale = m_LastHitObject.transform.localScale.y / 2f;
                }

                Vector2 endPoint = new Vector2(m_LastHitObject.transform.position.x, m_LastHitObject.transform.position.y) - m_LaserDirection * halfScale;

                //Recalculate the laser vector
                Vector2 casterToTarget = endPoint - beginPoint;

                //Project the target<-caster vector onto the direction vector
                m_CasterToTargetProject = Vector3.Project(casterToTarget, m_LaserDirection);


                var offsetTarget = Vector3.Project(endPoint - (Vector2)m_LastHitObject.transform.position, -m_LaserDirection);

                m_EndPosition = beginPoint + m_CasterToTargetProject;

            }
        }

        else
        {
            m_EndPosition = (Vector2)transform.position + m_LaserDirection * m_MaxProjectionDistance;
        }

        m_LineRenderer.SetPosition(1, m_EndPosition);
        Debug.DrawRay((Vector2)transform.position + m_LaserDirection * (transform.localScale.x / 2f), m_CasterToTargetProject, Color.blue);

    }
}

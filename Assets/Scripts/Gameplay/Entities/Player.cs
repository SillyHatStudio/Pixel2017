using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Player : Entity
{
    public InputDevice Device { get; set; }
    public EnumTypes.PlayerEnum m_PlayerNumber;
    private Rigidbody2D m_Rigidbody;
    public float Speed;

    //ref
    public GameObject m_Visual;
    public GameObject m_CrossHair;

    private bool m_RightStickWasPressed = false;
    private float m_BumpingMinimumTime = .2f;
    private float m_BumpingMinimumTimer = 0;

    private GameObject m_LastExitZoneEntered = null;

    private LineRenderer m_SightLineRenderer;
    public Color sightLineColor;
    public float sightLineWidth;
    private float maxSightLineDistance = 20f;
    private Vector2 m_SightLineEnd;
    private Vector2 m_PlayerToCrossHair;

    //ANIMATION
    private bool m_Moving, m_Attacking;
    public Animator m_Animator;

    //Attack Var..
    public float m_AttackTime = .5f;
    public float m_ChargingAttack = 0.1f;



    protected virtual void Awake()
    {
        m_PlayerNumber = EnumTypes.PlayerEnum.Unassigned;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SightLineRenderer = GetComponent<LineRenderer>();
        m_SightLineRenderer.startWidth = sightLineWidth;
        m_SightLineRenderer.endWidth = sightLineWidth;
        m_SightLineRenderer.startColor = sightLineColor;
        m_SightLineRenderer.endColor = sightLineColor;
        m_Animator.SetBool("Moving", m_Moving);
        m_Animator.SetBool("Attacking", m_Attacking);


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
        Animation();
    }

    private void Animation()
    {
        m_Animator.SetBool("Moving", m_Moving);
        m_Animator.SetBool("Attacking", m_Attacking);
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
        if (Device.LeftStick.Vector != Vector2.zero && !m_Attacking)
        {
            m_Rigidbody.velocity = Device.LeftStick.Vector * Time.deltaTime * Speed;          
            transform.eulerAngles = new Vector3(0, 0, -Device.LeftStick.Angle);
            m_Moving = true;
        }
        else
        {
            m_Rigidbody.velocity = m_Rigidbody.velocity *= .8f;
            m_Moving = false;
        }

        if (Device.RightStick.Vector != Vector2.zero && m_BumpingMinimumTimer == 0)
        {
            m_BumpingMinimumTimer = m_BumpingMinimumTime;

            float angle = Device.RightStick.Angle;
            

            CheckLineOfSight();
            if (angle >= 315 || angle < 45)
            {
                CheckIfCanMove(m_CrossHair.transform.position + Vector3.up);
            }
            else if (angle < 135)
            {
                CheckIfCanMove(m_CrossHair.transform.position + Vector3.right);
            }
            else if (angle < 225)
            {
                CheckIfCanMove(m_CrossHair.transform.position + Vector3.down);
            }
            else if (angle < 315)
            {
                CheckIfCanMove(m_CrossHair.transform.position + Vector3.left);
            }
        }

        if (Device.RightTrigger.WasPressed && !m_Attacking)//hit trigger and cast a cross on the game
        {            
            m_Attacking = true;
            StartCoroutine(StartAttack(m_AttackTime, m_CrossHair.transform.position));
            StartCoroutine(ChargeTime(m_ChargingAttack, m_CrossHair.transform.position));
        }
    }




    private IEnumerator ChargeTime(float _chargingTimeBeforeShooting, Vector3 _targetPosition)
    {
        yield return new WaitForSeconds(_chargingTimeBeforeShooting);
        GameObject ball = PoolManager.instance.GetPoolObject("Projectile");
        ball.transform.position = transform.position + Vector3.up;
        int id = (m_PlayerNumber == EnumTypes.PlayerEnum.P1) ? 0 : 1;
        ball.GetComponent<Projectile>().Shoot(ball.transform.position, _targetPosition, m_AttackTime - m_ChargingAttack, id);

    }
    private IEnumerator StartAttack(float _cooldownTime, Vector3 _targetPosition)
    {
        yield return new WaitForSeconds(_cooldownTime);
        CastCross(_targetPosition);
        m_Attacking = false;
    }

    private void CheckLineOfSight()
    {
       /* m_PlayerToCrossHair = m_CrossHair.transform.position - transform.position;

        m_SightLineRenderer.SetPosition(0, (Vector2)transform.position);

        Debug.DrawLine((Vector2)transform.position, m_CrossHair.transform.position, Color.green, 2);

        //Check for any obstacles between the caster and the wall
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, m_PlayerToCrossHair, maxSightLineDistance);
        if (hit)
        {
            Debug.Log("Line of sight hit obstacle");
            var obstacle = hit.collider.gameObject;

            var pointVec = new Vector2(hit.point.x, hit.point.y);
            Debug.DrawRay((Vector2)transform.position, pointVec - (Vector2)transform.position, Color.red, 2);

            m_SightLineRenderer.SetPosition(1, pointVec);
        }
        else
        {
            Debug.DrawRay((Vector2)transform.position, m_PlayerToCrossHair, Color.blue, 2);
        } */
    }

    //Do a raycast to a possible position to teleport the object to location
    private void CheckIfCanMove(Vector3 _position)
    {
        Collider2D[] collider = Physics2D.OverlapPointAll(_position);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].GetComponent<CubeBehaviour>())
            {
                m_CrossHair.transform.position = collider[i].transform.position;

            }
            else
            {

            }
        }
    }

    private void UpdateWatchingDirection(float _angle)
    {
        m_Visual.transform.eulerAngles = new Vector3(m_Visual.transform.eulerAngles.x, m_Visual.transform.eulerAngles.y, _angle);

    }

    private void CheckTypeOfFloorObject()
    {
        Collider2D[] colBelow = Physics2D.OverlapPointAll(gameObject.transform.position);
        GameObject boxBelow = null;

        for (int i = 0; i < colBelow.Length; i++)
        {
            if (colBelow[i].GetComponent<ExitCube>() && colBelow[i].GetComponent<ExitCube>().enabled)
            {
                m_LastExitZoneEntered = colBelow[i].gameObject;

                m_LastExitZoneEntered.GetComponent<CubeBehaviour>().m_CanColor = false;
                boxBelow = m_LastExitZoneEntered;

                m_LastExitZoneEntered.GetComponent<ExitCube>().CheckPlayerCollisionIn(gameObject);
            }

            else
            {
                if (m_LastExitZoneEntered)
                {
                    m_LastExitZoneEntered.GetComponent<ExitCube>().CheckPlayerCollisionOut(gameObject);
                    m_LastExitZoneEntered = null;
                }

            }
        }
    }

    private void CastCross(Vector3 _position)
    {
        CastBox(_position);
        CastBox(_position + Vector3.up);
        CastBox(_position + Vector3.left);
        CastBox(_position + Vector3.down);
        CastBox(_position + Vector3.right);
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
                if (!col[i].GetComponent<ExitCube>() || col[i].GetComponent<ExitCube>().enabled == false)
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
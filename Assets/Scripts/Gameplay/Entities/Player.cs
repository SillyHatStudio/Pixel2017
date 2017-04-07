﻿using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Player : Entity
{
    public InputDevice Device { get; set; }
    private EnumTypes.PlayerEnum m_PlayerNumber;
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

    protected virtual void Awake()
    {
        m_PlayerNumber = EnumTypes.PlayerEnum.Unassigned;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SightLineRenderer = GetComponent<LineRenderer>();
        m_SightLineRenderer.startWidth = sightLineWidth;
        m_SightLineRenderer.endWidth = sightLineWidth;
        m_SightLineRenderer.startColor = sightLineColor;
        m_SightLineRenderer.endColor = sightLineColor;

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

        if (Device.RightTrigger.WasPressed)//hit trigger and cast a cross on the game
        {
            CastCross();
        }
    }

    private void CheckLineOfSight()
    {
        m_PlayerToCrossHair = new Vector2(m_CrossHair.transform.position.x - transform.position.x, m_CrossHair.transform.position.y - transform.position.y);

        m_SightLineRenderer.SetPosition(0, (Vector2)transform.position);

        //Check for any obstacles between the caster and the wall
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + m_PlayerToCrossHair * (transform.localScale.x / 2f), m_PlayerToCrossHair, maxSightLineDistance);
        if (hit)
        {
            Debug.Log("Line of sight hit obstacle");
            var obstacle = hit.collider.gameObject;

            var pointVec = new Vector2(hit.point.x, hit.point.y);
            Debug.DrawRay((Vector2)transform.position, pointVec - (Vector2)transform.position, Color.red);

            m_SightLineRenderer.SetPosition(1, pointVec);
        }
        else
        {
            Debug.DrawRay((Vector2)transform.position, new Vector2(hit.point.x, hit.point.y) - (Vector2)transform.position, Color.blue);
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
                m_CrossHair.transform.position = collider[i].transform.position;

            }
            else
            {

            }
        }
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

    private void CastCross()
    {
        CastBox(m_CrossHair.transform.position);
        CastBox(m_CrossHair.transform.position + Vector3.up);
        CastBox(m_CrossHair.transform.position + Vector3.left);
        CastBox(m_CrossHair.transform.position + Vector3.down);
        CastBox(m_CrossHair.transform.position + Vector3.right);
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
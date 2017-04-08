using Pixel.Game.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(TimerManager))]
public class CubeBehaviour : MonoBehaviour
{

    private EnumTypes.PlayerEnum m_OwnerNumber;
    public GameObject m_Visual;
    public bool m_CanColor = true;

    public bool m_IsFlippable = true;
    public CubeState m_CubeSate = CubeState.Grey;
    private float m_TargetAngle;
    private float m_FlipSpeed = 720f;



    [HideInInspector]
    public GameObject MapManager;

    protected virtual void Awake()
    {
        m_OwnerNumber = EnumTypes.PlayerEnum.Unassigned;      


             
        m_CubeSate = CubeState.Grey;
        gameObject.layer = LayerMask.NameToLayer("Grey");  
        m_IsFlippable = true;

    }

    // Use this for initialization
    void Start()
    {

    }




    // Update is called once per frame
    protected virtual void Update()
    {
        Flip();
    } 

    private void Flip()
    {
        if(transform.eulerAngles.z != m_TargetAngle)
        {           
            if(m_TargetAngle != m_Visual.transform.eulerAngles.z)
            {               
                if(m_TargetAngle > m_Visual.transform.eulerAngles.z)
                {
                    m_Visual.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * m_FlipSpeed);
                    if(m_Visual.transform.eulerAngles.z >= m_TargetAngle)
                    {
                        m_Visual.transform.eulerAngles = new Vector3(m_Visual.transform.eulerAngles.x, m_Visual.transform.eulerAngles.y, m_TargetAngle);
                        m_IsFlippable = true;
                    }
                }
                else if(m_TargetAngle < m_Visual.transform.eulerAngles.z)
                {
                    m_Visual.transform.eulerAngles -= new Vector3(0, 0, Time.deltaTime * m_FlipSpeed);
                    if (m_Visual.transform.eulerAngles.z <= m_TargetAngle)
                    {
                        m_Visual.transform.eulerAngles = new Vector3(m_Visual.transform.eulerAngles.x, m_Visual.transform.eulerAngles.y, m_TargetAngle);
                        m_IsFlippable = true;
                    }
                } 
            }
        }

        if(m_TargetAngle == m_Visual.transform.eulerAngles.z)
        {
            m_IsFlippable = true;
        }
    }

    private void SetFlip()
    {
        if(m_IsFlippable)
        {
            m_IsFlippable = false;
            switch (m_CubeSate)
            {
                case CubeState.Grey:
                    m_TargetAngle = 0;
                    break;
                case CubeState.Black:
                    m_TargetAngle = 240;
                    break;
                case CubeState.White:
                    m_TargetAngle = 120;
                    break;
                case CubeState.None:
                    print("your mom");
                    break;
                default:
                    break;
            }
        }
    }


    void SetOwner(int ownerNumber)
    {
        switch (ownerNumber)
        {
            case 0:
                m_OwnerNumber = EnumTypes.PlayerEnum.P1;              
                SetMaterialColor(EnumTypes.PlayerColors.ColorP1);
                break;

            case 1:
                m_OwnerNumber = EnumTypes.PlayerEnum.P2;             
                SetMaterialColor(EnumTypes.PlayerColors.ColorP2);
                break;

            default:
                m_OwnerNumber = EnumTypes.PlayerEnum.Unassigned;              
                SetMaterialColor(EnumTypes.PlayerColors.ColorNone);
                break;
        }
    }

    public void SetMaterialColor(Color color)
    {
        if (!m_IsFlippable)
            return;

        if (color == Color.black)
        {
            gameObject.layer = LayerMask.NameToLayer("Black");
            m_CubeSate = CubeState.Black;
            SetFlip();
        }
        else if (color == Color.white)
        {
            gameObject.layer = LayerMask.NameToLayer("White");
            m_CubeSate = CubeState.White;
            SetFlip();
        }

        else if (color == Color.gray)
        {
            gameObject.layer = LayerMask.NameToLayer("Grey");
            m_CubeSate = CubeState.Grey;
            SetFlip();
        }
    }

    public void SetMaterialColor(int _playerId)
    {
        if (!m_IsFlippable)
            return;
        Color col = (_playerId == 0) ? Color.black : Color.white;


        //m_Material.color = col;

        if (col == Color.black)
        {
            gameObject.layer = LayerMask.NameToLayer("Black");
            m_CubeSate = CubeState.Black;
            SetFlip();

        }
        else if (col == Color.white)
        {
            gameObject.layer = LayerMask.NameToLayer("White");
            m_CubeSate = CubeState.White;
            SetFlip();
        }
        else if (col == Color.gray)
        {
            gameObject.layer = LayerMask.NameToLayer("Grey");
            m_CubeSate = CubeState.Grey;
            SetFlip();
        }

    }


    public enum CubeState
    {
        Grey,
        Black,
        White,
        None
    }

}

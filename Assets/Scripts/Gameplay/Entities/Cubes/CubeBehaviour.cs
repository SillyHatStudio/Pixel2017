using Pixel.Game.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(TimerManager))]
public class CubeBehaviour : MonoBehaviour
{

    private EnumTypes.PlayerEnum m_OwnerNumber;
    private Color m_CurrentColor;
    private Material m_Material;
    public GameObject m_Visual;
    public bool m_CanColor = true;

    [Range(1, 59)]
    public int timeBetweenFlips;
    public bool m_IsFlippable;
    protected bool m_IsTimingForFlip;
    protected bool m_animationInProgress;
    public TimerManager flipTimer;

    protected virtual void Awake()
    {
        m_OwnerNumber = EnumTypes.PlayerEnum.Unassigned;
        m_CurrentColor = Color.gray;

        m_Material = m_Visual.GetComponent<MeshRenderer>().material;
        m_Material.color = Color.gray;

        if(m_IsFlippable)
        {
            m_IsTimingForFlip = false;

            if (flipTimer)
            {
                flipTimer.m_timerType = EnumTypes.TimerTypeEnum.Countdown;
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(m_IsFlippable)
        {
            if(m_IsTimingForFlip)
            {
                //If the timer is over and we are waiting to flip, do it
                if (flipTimer.remainingSecs == 0)
                {
                    Flip();
                }
            }
        } 
    }

    //Called when the object got painted by a player
    //Cases : no player on it : flip and set timer
    //Has a player above it : flip, set timer + bump the player
    public void GotPaintedCallback()
    {
        if (m_IsFlippable)
        {
            //TODO check if player on it

            Flip();
        }
    }

    public void Flip()
    {
        m_IsTimingForFlip = false;
        m_animationInProgress = true;

        //Todo : rotation over time to show the right face

        m_animationInProgress = false;
        flipTimer.resetTimer();
    }


    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    void SetOwner(int ownerNumber)
    {
        switch (ownerNumber)
        {
            case 0:
                m_OwnerNumber = EnumTypes.PlayerEnum.P1;
                m_CurrentColor = EnumTypes.PlayerColors.ColorP1;
                SetMaterialColor(EnumTypes.PlayerColors.ColorP1);
                break;

            case 1:
                m_OwnerNumber = EnumTypes.PlayerEnum.P2;
                m_CurrentColor = EnumTypes.PlayerColors.ColorP2;
                SetMaterialColor(EnumTypes.PlayerColors.ColorP2);
                break;

            default:
                m_OwnerNumber = EnumTypes.PlayerEnum.Unassigned;
                m_CurrentColor = EnumTypes.PlayerColors.ColorNone;
                SetMaterialColor(EnumTypes.PlayerColors.ColorNone);
                break;
        }
    }

    public void SetMaterialColor(Color color)
    {
        m_Material.color = color;

        if (color == Color.black)
        {
            gameObject.layer = LayerMask.NameToLayer("Black");
        }
        else if (color == Color.white)
        {
            gameObject.layer = LayerMask.NameToLayer("White");
        }

        else if (color == Color.gray)
        {
            gameObject.layer = LayerMask.NameToLayer("Grey");
        }
    }

    public void SetMaterialColor(int _playerId)
    {
        Color col = (_playerId == 0) ? Color.black : Color.white;


        m_Material.color = col;

        if (col == Color.black)
        {
            gameObject.layer = LayerMask.NameToLayer("Black");
        }
        else if (col == Color.white)
        {
            gameObject.layer = LayerMask.NameToLayer("White");
        }
        else if (col == Color.gray)
        {
            gameObject.layer = LayerMask.NameToLayer("Grey");
        }

    }
}

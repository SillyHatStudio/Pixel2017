using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Menu : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    // Members:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
    public FadeOverlay m_FadeOverlay;
    public GameObject m_PressStartLabel;
    public GameObject m_Button0;
    public GameObject m_Button1;
    public GameObject m_ButtonQuit;
    public GameObject m_ButtonContinue;
    public GameObject m_Arrow;
    public GameObject m_P1Ready, m_P2Ready;
    public GameObject m_MenuSelection;
    public GameObject m_MenuGameOver;
    public bool m_IsGameOver = false;

    public bool m_StartMenuOpen = true;

    const int BUTTON_COUNT__ = 2;
    int m_ButtonIndex;
    int m_SelectIndex;

    bool m_PlayerReady;



    void Start()
    {
        m_Arrow.SetActive(false);
        m_PressStartLabel.SetActive(true);
        m_MenuSelection.SetActive(false);
        m_MenuGameOver.SetActive(false);
        m_ButtonQuit.SetActive(false);
        m_ButtonContinue.SetActive(false);

        m_ButtonIndex = 0;
        m_SelectIndex = -1;
        m_PlayerReady = false;
    }

    void Update()
    {
        if (GameManager.GetInstance().GetCurrentPlayerCount() > 0)
        {
            if (m_PlayerReady == false)
            {
                m_PlayerReady = true;
                return;
            }
        }
        else
        {
            return;
        }


        if (GameManager.GetInstance().GetCurrentPlayerCount() > 0)
        {
            m_P1Ready.gameObject.SetActive(true);
            if (GameManager.GetInstance().GetCurrentPlayerCount() > 1)
            {
                m_P2Ready.gameObject.SetActive(true);
                m_MenuSelection.SetActive(true);
                m_Arrow.SetActive(true);
                m_PressStartLabel.SetActive(false);
            }
        }



        if (m_PlayerReady && m_FadeOverlay.m_PerformFadeOut == false)
        {
            Player active = GameManager.GetInstance().GetPlayerAt(0);



            if (active.Device.DPadUp.WasPressed)
            {
                if (m_ButtonIndex != 0)
                {
                    m_ButtonIndex--;
                }
                else
                {
                    m_ButtonIndex = BUTTON_COUNT__ - 1;
                }
            }
            if (active.Device.DPadDown.WasPressed)
            {
                if (m_ButtonIndex < BUTTON_COUNT__ - 1)
                {
                    m_ButtonIndex++;
                }
                else
                {
                    m_ButtonIndex = 0;
                }
            }

            switch (m_ButtonIndex)
            {
                case 0:
                    m_Arrow.transform.position = new Vector3(
                        m_Arrow.transform.position.x,
                        m_Button0.transform.position.y
                    );
                    break;

                case 1:
                    m_Arrow.transform.position = new Vector3(
                        m_Arrow.transform.position.x,
                        m_Button1.transform.position.y
                    );
                    break;

                default:
                    break;
            }

            if (active.Device.Action1.WasPressed)
            {
                if (GameManager.GetInstance().GetCurrentPlayerCount() == 2)
                {
                    m_FadeOverlay.m_PerformFadeOut = true;
                }
                m_SelectIndex = m_ButtonIndex;
            }
        }

        if (m_IsGameOver)
        {
            Player active = GameManager.GetInstance().GetPlayerAt(0);
            m_MenuGameOver.SetActive(true);
            m_ButtonQuit.SetActive(true);
            m_ButtonContinue.SetActive(true);

            if (active.Device.DPadLeft.WasPressed)
            {
                if (m_ButtonIndex != 0)
                {
                    m_ButtonIndex--;
                }
                else
                {
                    m_ButtonIndex = BUTTON_COUNT__ - 1;
                }
            }
            if (active.Device.DPadRight.WasPressed)
            {
                if (m_ButtonIndex < BUTTON_COUNT__ - 1)
                {
                    m_ButtonIndex++;
                }
                else
                {
                    m_ButtonIndex = 0;
                }
            }

            switch (m_ButtonIndex)
            {
                case 0:
                    m_Arrow.transform.position = new Vector3(
                        m_Arrow.transform.position.x,
                        m_ButtonContinue.transform.position.y
                    );
                    break;

                case 1:
                    m_Arrow.transform.position = new Vector3(
                        m_Arrow.transform.position.x,
                        m_ButtonQuit.transform.position.y
                    );
                    break;

                default:
                    break;
            }

            if (active.Device.Action1.WasPressed)
            {
                if (m_ButtonIndex == 0)
                {
                    GameManager.GetInstance().LoadNextMap();
                }

                else if (m_ButtonIndex == 0)
                {
                    GameManager.GetInstance().SetCurrentState(GameManager.GameState.MainMenu);
                }
                m_SelectIndex = m_ButtonIndex;
                ResetGameOverState();
            }

        }
    }

    public void ResetGameOverState()
    {
        m_IsGameOver = false;
        m_MenuGameOver.SetActive(false);
        m_ButtonQuit.SetActive(false);
        m_ButtonContinue.SetActive(false);
    }

    public int GetSelectIndex()
    {
        return m_SelectIndex;
    }

    public bool IsDone()
    {
        return m_FadeOverlay.IsDone();
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CubeBehaviour : MonoBehaviour
{

    private EnumTypes.PlayerEnum m_OwnerNumber;
    private Color m_CurrentColor;
    private Material m_Material;
    public GameObject m_Visual;
    public bool m_CanColor = true;


    protected virtual void Awake()
    {
        m_OwnerNumber = EnumTypes.PlayerEnum.Unassigned;
        m_CurrentColor = Color.gray;

        m_Material = m_Visual.GetComponent<MeshRenderer>().material;
        m_Material.color = Color.gray;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

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
    }

    public void SetMaterialColor(int _playerId)
    {
        Color _myColor = (_playerId == 0) ? Color.black : Color.white;


        m_Material.color = _myColor;

        if (_myColor == Color.black)
        {
            gameObject.layer = LayerMask.NameToLayer("Black");
        }
        else if (_myColor == Color.white)
        {
            gameObject.layer = LayerMask.NameToLayer("White");
        }

    }
}

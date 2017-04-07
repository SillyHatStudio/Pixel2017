using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CubeColoring : MonoBehaviour {

    private EnumTypes.PlayerEnum m_OwnerNumber;
    private Color m_CurrentColor;
    private Material m_Material;

    void Awake()
    {
        m_OwnerNumber = EnumTypes.PlayerEnum.Unassigned;
        m_CurrentColor = Color.gray;

        m_Material = GetComponent<MeshRenderer>().material;

    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

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
    }
}

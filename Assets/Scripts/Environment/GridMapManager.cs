using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridMapManager : MonoBehaviour
{

    public int width;
    public int height;
    private int defaultWidth = 12, defaultHeight = 10;
    public GameObject MapCubePrefab;

    [HideInInspector]
    public GameObject[,] cubegrid;
    private List<GameObject> m_CubesList;

    public PlayerSpawnPoint[] m_PlayerSpawnPoint;
    public WinZone[] m_WinZones;

    private void Awake()
    {
        //if (transform.position.x != 0 && transform.position.y != 0)
        //    transform.position = new Vector2(0, 0);

        if (width <= 0)
            width = defaultWidth;

        if (height <= 0)
            height = defaultHeight;

        cubegrid = new GameObject[width, height];
        m_CubesList = new List<GameObject>();
    }

    // Update is called once per frame
    private void Start()
    {
        //Fill grid
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (PoolManager.instance && MapCubePrefab)
                {
                    //Place a cube on x,y
                    GameObject cube = PoolManager.instance.GetPoolObject(MapCubePrefab.name);
                    cube.transform.position = new Vector2(i+transform.position.x, j+transform.position.y);
                    cubegrid[i, j] = cube;
                    m_CubesList.Add(cube);
                }
            }
        }

        PlacePlayerSpawnPoints();

        PlaceWinZones();
    }

    public void PlacePlayerSpawnPoints()
    {

        if (m_PlayerSpawnPoint.Length > 0)
        {

            Color ownerColor;

            for (int i = 0; i < m_PlayerSpawnPoint.Length; i++)
            {
                ownerColor = (m_PlayerSpawnPoint[i].owner == 0) ? Color.black : Color.white;
                FillSpawnPointColor((int)m_PlayerSpawnPoint[i].position.x, (int)m_PlayerSpawnPoint[i].position.y, i, ownerColor);
            }
        }
    }

    public void PlaceWinZones()
    {
        if (m_WinZones.Length == 1)
        {
            //Change value for any and 2 players validation if set to something else
            m_WinZones[0].playerAllowed = WinZone.AllowedPlayer.Any;
           

            Vector2 coordinates = m_WinZones[0].position;
            var exitCube = cubegrid[(int)coordinates.x, (int)coordinates.y].GetComponent<ExitCube>();
            exitCube.enabled = true;
            exitCube.m_PlayersThatCanGoInside = ExitCube.PlayerAuthorized.Any;
            exitCube.m_NumberOfPlayersRequiredInside = 2;
            exitCube.SetMaterialColor(Color.green);
            exitCube.MapManager = gameObject;
            exitCube.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        else if (m_WinZones.Length > 1)
        {
            for (int i = 0; i < m_WinZones.Length; i++)
            {
                Vector2 coordinates = m_WinZones[i].position;
                var exitCube = cubegrid[(int)coordinates.x, (int)coordinates.y].GetComponent<ExitCube>();
                exitCube.enabled = true;
                exitCube.MapManager = gameObject;
                exitCube.m_NumberOfPlayersRequiredInside = 1;
                exitCube.SetMaterialColor(Color.green); //tmp
                exitCube.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

                if (m_WinZones[i].playerAllowed == WinZone.AllowedPlayer.Any)
                {
                    exitCube.m_PlayersThatCanGoInside = ExitCube.PlayerAuthorized.Any;
                }

                else if (m_WinZones[i].playerAllowed == WinZone.AllowedPlayer.P1)
                {
                    exitCube.m_PlayersThatCanGoInside = ExitCube.PlayerAuthorized.P1;
                }

                else if(m_WinZones[i].playerAllowed == WinZone.AllowedPlayer.P2)
                {
                    exitCube.m_PlayersThatCanGoInside = ExitCube.PlayerAuthorized.P2;
                }
            }
        }
    }

    private void FillSpawnPointColor(int _initX, int _initY, int _playerSpawnPointIndex, Color _color)
    {
        for (int i = 0; i < m_PlayerSpawnPoint[_playerSpawnPointIndex].size.x; i++)
        {
            for (int j = 0; j < m_PlayerSpawnPoint[_playerSpawnPointIndex].size.y; j++)
            {
                cubegrid[_initX + i, _initY + j].GetComponent<CubeBehaviour>().SetMaterialColor(_color);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var anyZoneWithAPlayer = m_CubesList.Any(c => c.GetComponent<ExitCube>() != null && c.GetComponent<ExitCube>().enabled && c.GetComponent<ExitCube>().validated);

        if (anyZoneWithAPlayer)
        {
            Debug.Log("There are player(s) in a zone");
            CheckAllPlayersAreInWinZones();
        }
        
    }

    public void CheckAllPlayersAreInWinZones()
    {
        var winZones = m_CubesList.Where(c => c.GetComponent<ExitCube>() != null && c.GetComponent<ExitCube>().enabled);

        Debug.Log("win zones count = " + winZones.Count());

        if (winZones.Count() == 1)
        {
            if (winZones.First().GetComponent<ExitCube>().validated)
            {
                Debug.Log("victory (the winzone is valid)");
            }
        }

        else if (winZones.Count() > 0)
        {
            var validWinZones = winZones.Where(c => c.GetComponent<ExitCube>().validated);

            if (validWinZones.Count() == winZones.Count()) Debug.Log("All zones are valid");
        }
    }

    [System.Serializable]
    public struct PlayerSpawnPoint
    {
        public Vector2 position;
        public Vector2 size;
        public int owner;
    }

    [System.Serializable]
    public struct WinZone
    {
        public Vector2 position;
        public AllowedPlayer playerAllowed;

        public enum AllowedPlayer
        {
            Any,
            P1,
            P2
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    [System.Serializable]
    public struct CameraSettings
    {
        public Vector2 startPosition;
        public Vector3 startRotation;
        public bool scrolling;
        public float movementSpeed;
        public float distanceToMove;
        public ScrollingCamera.MovementDirection movementDirection;
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

    [System.Serializable]
    public struct MapConfig
    {
        public List<GridMapConfig> mapManagersConfig;
    }

    [System.Serializable]
    public struct GridMapConfig
    {
        [HideInInspector]
        public GameObject managerObject;
        public Vector2 origin;
        public int width, height;
        public List<PlayerSpawnPoint> spawnPoints;
        public List<WinZone> WinZones;
    }

    public CameraSettings cameraSettings;

    public MapConfig stageConfig;

    private List<GridMapManager> m_GridMapManagers;

    public List<Vector2> spawnPoints;


    private void Awake()
    {
        GameManager.GetInstance().StageManager = gameObject;

        //Camera config
        Camera.main.transform.position = cameraSettings.startPosition;
        Camera.main.transform.rotation = Quaternion.Euler(cameraSettings.startRotation);

        if (cameraSettings.scrolling)
        {
            var scrollComponent = Camera.main.GetComponent<ScrollingCamera>();
            scrollComponent.enabled = true;
            scrollComponent.m_MovementSpeed = cameraSettings.movementSpeed;
            scrollComponent.m_MovementDirection = cameraSettings.movementDirection;
            scrollComponent.m_distanceToMove = cameraSettings.distanceToMove;
        }

        //Grid config

        m_GridMapManagers = new List<GridMapManager>();
        for (int i = 0; i < stageConfig.mapManagersConfig.Count; i++)
        {
            var manager = new GameObject();
            manager.name = "GridMapManager_" + i;
            var managerComponent = manager.AddComponent<GridMapManager>();

            manager.transform.position = stageConfig.mapManagersConfig[i].origin;
            managerComponent.width = stageConfig.mapManagersConfig[i].width;
            managerComponent.height = stageConfig.mapManagersConfig[i].height;
            managerComponent.m_WinZones = new GridMapManager.WinZone[stageConfig.mapManagersConfig[i].WinZones.Count];

            for (int j = 0; j < stageConfig.mapManagersConfig[i].WinZones.Count; j++)
            {
                managerComponent.m_WinZones[j] = new GridMapManager.WinZone
                {
                    position = stageConfig.mapManagersConfig[i].WinZones[j].position,
                    playerAllowed = (GridMapManager.WinZone.AllowedPlayer)stageConfig.mapManagersConfig[i].WinZones[j].playerAllowed
                };
            }


            managerComponent.m_PlayerSpawnPoint = new GridMapManager.PlayerSpawnPoint[stageConfig.mapManagersConfig[i].spawnPoints.Count];
            for (int j = 0; j < stageConfig.mapManagersConfig[i].spawnPoints.Count; j++)
            {
                managerComponent.m_PlayerSpawnPoint[j] = new GridMapManager.PlayerSpawnPoint
                {
                    owner = stageConfig.mapManagersConfig[i].spawnPoints[j].owner,
                    position = stageConfig.mapManagersConfig[i].spawnPoints[j].position,
                    size = stageConfig.mapManagersConfig[i].spawnPoints[j].size
                };
            }

            Debug.Log("width = " + stageConfig.mapManagersConfig[i].width + ", height = " + stageConfig.mapManagersConfig[i].height);

            managerComponent.cubegrid = new GameObject[stageConfig.mapManagersConfig[i].width, stageConfig.mapManagersConfig[i].height];
            managerComponent.InitGridObjects();
            managerComponent.PlacePlayerSpawnPoints();
            managerComponent.PlaceWinZones();

            SpawnPlayers();
        }
    }

    public void SpawnPlayers()
    {
        //Players config

        for (int i = 0; i < spawnPoints.Count(); i++)
        {
            var p = GameManager.GetInstance().GetPlayerAt(i);
            p.transform.position = spawnPoints.ElementAt(i);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGridGenerator : MonoBehaviour
{

    public int width;
    public int height;
    private int defaultWidth = 12, defaultHeight = 10;
    private int spawnPointWidth = 2, spawnPointHeight =  1;
    public GameObject MapCubePrefab;
    private int m_OffsetToLeftBorder = 2;

    public Vector2 spawnPointP1, spawnPointP2;

    [HideInInspector]
    public GameObject[,] cubegrid;

    private void Awake()
    {
        if (transform.position.x != 0 && transform.position.y != 0)
            transform.position = new Vector2(0, 0);

        if (width <= 0)
            width = defaultWidth;

        if (height <= 0)
            height = defaultHeight;

        cubegrid = new GameObject[width, height];
    }

    // Update is called once per frame
    private void Start()
    {
        //Fill grid
        for (int i = (int)transform.position.x; i < width; i++)
        {
            for (int j = (int)transform.position.y; j < height; j++)
            {
                if (MapCubePrefab)
                {
                    //Place a cube on x,y
                    GameObject cube = PoolManager.instance.GetPoolObject(MapCubePrefab.name);
                    cube.transform.position = new Vector2(i, j);
                    cubegrid[i, j] = cube;
                }
            }
        }

        PlacePlayerSpawnPoints();
    }

    public void PlacePlayerSpawnPoints()
    {
        /*float quarter = height / 4f;
        float upperSpawnY = Mathf.FloorToInt(height / 2f + quarter);
        float lowerSpawnY = Mathf.FloorToInt(height / 2f - quarter);

        for(int i=0; i< spawnPointWidth; i++)
        {
            for (int j = 0; j < spawnPointWidth; j++)
            {
                cubegrid[m_OffsetToLeftBorder + i, (int)upperSpawnY - j].GetComponent<CubeColoring>().SetMaterialColor(Color.black);
                cubegrid[m_OffsetToLeftBorder + i, (int)lowerSpawnY - j].GetComponent<CubeColoring>().SetMaterialColor(Color.blue);
            }    
        }*/


        //To continue 


        /*if(spawnPointP1.x >= 0 && spawnPointP1.x < width)
        {
            //If the spawnpoint overflows the right limit of the map, offet it to the left
            if(spawnPointP1.x + spawnPointWidth > width)
            {
                var leftOffset = spawnPointWidth - Mathf.Abs(width - spawnPointP1.x);
                spawnPointP1.x = width - leftOffset;
            }

            //If overflow on left  => offsets to the right
            if (spawnPointP1.x + spawnPointWidth > width)
            {
                var rightOffset = spawnPointWidth - Mathf.Abs(0 + spawnPointP1.x);
                spawnPointP1.x = rightOffset;
            }

            //If overflow top  => offsets to bottom
            if (spawnPointP1.y + spawnPointHeight > height)
            {
                var botOffset = height - Mathf.Abs(spawnPointP1.y);
                //spawnPointP1.x = rightOffset;
            }
        } */

        


    }

    // Update is called once per frame
    private void Update()
    {

    }
}

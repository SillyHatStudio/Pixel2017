using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGridGenerator : MonoBehaviour
{

    public int width;
    public int height;
    private int defaultWidth = 12, defaultHeight = 10;
    public GameObject MapCubePrefab;
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
        for(int i= (int)transform.position.x; i < width; i++)
        {
            for (int  j = (int)transform.position.y; j < height; j++)
            {
                if (MapCubePrefab)
                {
                    //Place a cube on x,y
                    GameObject cube = PoolManager.instance.GetPoolObject(MapCubePrefab.name);
                    cube.transform.position = new Vector2(i,j);
                    cubegrid[i, j] = cube;
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }
}

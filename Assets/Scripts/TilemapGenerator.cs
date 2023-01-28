using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Tilemaps;


public class TilemapGenerator : MonoBehaviour
{
    private GameObjectBaseGen baseGen;
    private GameObjectBaseGen.GenCell cellGen;

    public GridLayout gridLayout;

    public GameObject parentLayer;

    public Vector2Int TilemapSize;

    public GameObject tileObj;

    private Vector3Int initVect;
    private Vector3Int initVect2;


    private void Start()
    {
        Debug.Log("Start TilemapGen");
        baseGen = (GameObjectBaseGen)ScriptableObject.CreateInstance(typeof(GameObjectBaseGen));
        
        cellGen = new GameObjectBaseGen.GenCell();
        cellGen.go = tileObj;
        Debug.Log("go to paint: " + cellGen.go.name);

        initVect = new Vector3Int(1, 1, 0);
        initVect2 = new Vector3Int(2, 1, 0);
        //Paint();
        Debug.Log("End TilemapGen");
    }

    /// <summary>
    /// Paint a tile at the given position on the selected layer on the selected grid with the selected gameobject and its properties. 
    /// </summary>

    [ContextMenu("Paint")]
    void Paint()
    {
        Debug.Log("Paint");
        // gridLayout is related to the grid gameobject
        // parentLayer.transform is related to one children of grid (ground1, ground2, ...)
        // initVect is the position of the object to paint
        // cellGen.go is the gameobject to paint
        // cellGen.offset is the height where we paint (relation with parentLayer)
        // cellGen.scale is the scale of the gameobject painted
        // cellGen.orientation is the orientation of the gameobject
        // baseGen.anchor is the center of mass of the gameobject
        baseGen.PaintGo(gridLayout, parentLayer.transform, initVect, cellGen.go, cellGen.offset, cellGen.scale, cellGen.orientation, baseGen.anchor);
    }

    [ContextMenu("GenerateBlockLayer")]
    private void GenerateBlockLayer()
    {
        int x = Mathf.Abs((int)TilemapSize.x);
        int y = Mathf.Abs((int)TilemapSize.y);

        int[,] arrayBlockFull = new int[x, y];
        arrayBlockFull = FullFilling(x, y, arrayBlockFull);

        int[,] arrayBlockRandom = new int[x, y];
        arrayBlockRandom = RandomFilling(x, y, arrayBlockRandom);
    }

    private int[,] FullFilling(int sizeX, int sizeY, int[,] block)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                block[i, j] = 1;
                Debug.Log(block[i, j]);
            }
        }
        return block;
    }

    private int[,] RandomFilling(int sizeX, int sizeY, int[,] block)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                block[i, j] = Random.Range(0,2);
                Debug.Log(block[i, j]);
            }
        }
        return block;
    }
}

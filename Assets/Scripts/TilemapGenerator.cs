using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Tilemaps;


public class TilemapGenerator : MonoBehaviour
{
    private GameObjectBaseGen baseGen;
    private GameObjectBaseGen.GenCell cellGen;
    private Displayer displayer;

    public GridLayout gridLayout;

    // TODO: each parent layer should be create each time the offset increase by 1 
    public GameObject[] parentLayer;

    public Vector2Int TilemapSize;

    public GameObject[] tileObj;
    // Array for index baseGo 
    // Array of gameobject for ground {0: none, 1: baseGo, 2: go2, 3: go3, ...}
    int[] baseGoIndex = new int[] { 1 };
    int[] goIndex = new int[] { 0, 1 };

    private void Start()
    {
        baseGen = (GameObjectBaseGen)ScriptableObject.CreateInstance(typeof(GameObjectBaseGen));
        
        cellGen = new GameObjectBaseGen.GenCell();
        cellGen.go = tileObj;
        Debug.Log("go to paint: " + cellGen.go[1].name);

        displayer = GetComponent<Displayer>();

        // TODO: create a button in inspector as value to call the RenderTilemap context menu.
        //RenderTilemap();
    }

    [ContextMenu("Render")]    
    private void Render()
    {
        // TODO: clear last block size & generate a new block with the same or a new size.
        

    }

    [ContextMenu("PaintTilemap")]
    /// <summary>
    /// Render the tilemap. The context menu can be call directly on play mode.
    /// Call the displayer to set active each tile of the current layer with animations.
    /// </summary>
    private void PaintTilemap()
    {
        // TODO: iterate over layer here to build layer by layer
        int layerIndex = 0;
        foreach (GameObject l in parentLayer)
        {
            Debug.Log("L? " + l);
            int[,] block = GenerateBlockLayer();
            for (int i = 0; i < block.GetLength(0); i++)
            {
                for (int j = 0; j < block.GetLength(1); j++)
                {
                    Vector3Int goPos = new Vector3Int(i, j, 0);
                    Paint(gridLayout, l.transform, goPos, cellGen.go[block[i, j]], new Vector3(0, 0.5f + layerIndex, 0), cellGen.scale, cellGen.orientation, baseGen.anchor);
                }
            }
            layerIndex++;
        }
        // TODO: displayer for each layer
        //Displayer d = new Displayer();
        StartCoroutine(displayer.HelloDisplayer(parentLayer));
        Debug.Log("painttilemap() is done?");
        
    }

    [ContextMenu("TestClearTilemap")]
    private void testClear()
    {
        StartCoroutine(displayer.ByeDisplayer(parentLayer[0]));
    }

    [ContextMenu("ClearTilemap")]
    private void ClearTilemap()
    {
        int[,] block = GenerateBlockLayer();
        for (int i = 0; i < block.GetLength(0); i++)
        {
            for (int j = 0; j < block.GetLength(1); j++)
            {
                Vector3Int goPos = new Vector3Int(i, j, 0);
                Clear(gridLayout, parentLayer[0].transform, goPos, baseGen.anchor);
            }
        }

        Debug.Log("cleartilemap() is done?");
    }


    /// <summary>
    /// Paint a tile at the given position on the selected layer on the selected grid with the selected gameobject and its properties. 
    /// </summary>
    /// <param name="gridLayout">The grid gameobject selected.</param>
    /// <param name="parentLayer">Current selected children of the grid.</param>
    /// <param name="position">The position where to paint the object.</param>
    /// <param name="go">The gameobject to paint.</param>
    /// <param name="offset">The height where the gameobject should be painted (relation with parentLayer).</param>
    /// <param name="scale">The scale of the gameobject.</param>
    /// <param name="orientation">The orientation of the gameobject.</param>
    /// <param name="anchor">The center of mass of the gameobject.</param>
    private void Paint(GridLayout gridLayout, Transform parentLayer, Vector3Int position, GameObject go, Vector3 offset, Vector3 scale, Quaternion orientation, Vector3 anchor)
    {
        baseGen.PaintGo(gridLayout, parentLayer, position, go, offset, scale, orientation, anchor);
    }


    private void Clear(GridLayout gridLayout, Transform parentLayer, Vector3Int position, Vector3 anchor)
    {
        baseGen.ClearGo(gridLayout, parentLayer, position, anchor);
    }

    /// <summary>
    /// Generate a [x].[y] block of tiles on a same layer.
    /// </summary>
    /// <returns>An array of index value for each tile representing the gameobject to paint.</returns>
    private int[,] GenerateBlockLayer()
    {
        int x = Mathf.Abs((int)TilemapSize.x);
        int y = Mathf.Abs((int)TilemapSize.y);

        int[,] arrayBlockFull = new int[x, y];
       
        arrayBlockFull = LayerFilling(arrayBlockFull, baseGoIndex, "baseTile");

        int[,] arrayBlockRandom = new int[x, y];
        arrayBlockRandom = LayerFilling(arrayBlockRandom, goIndex, "randomTile");

        return arrayBlockFull;
    }


    /// <summary>
    /// Fill tiles of a given block with gameobjects.
    /// </summary>
    /// <param name="block"></param>
    /// <param name="index"></param>
    /// <param name="flagGo"></param>
    /// <returns>A block of int values.</returns>
    private int[,] LayerFilling(int[,] block, int[] index, string flagGo)
    {
        for (int i = 0; i < block.GetLength(0); i++)
        {
            for (int j = 0; j < block.GetLength(1); j++)
            {
                switch (flagGo)
                {
                    case "emptyTile":
                        block[i, j] = 0;
                        break;
                    case "baseTile":
                        block[i, j] = 1;
                        break;    
                    default:
                        /// TODO: replace goIndex.Length by the size of gameobject array to randomly choose
                        /// and replace int[] index by gameobject[] listGameObject
                        block[i, j] = UnityEngine.Random.Range(2, goIndex.Length);
                        break;
                }                
            }
        }
        return block;
    }

}

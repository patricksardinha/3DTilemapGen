using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Tilemaps;


public class TilemapGenerator : MonoBehaviour
{
    public GridLayout gridLayout;
    public GameObject tileObj;
    public GameObject parentLayer;

    private GameObjectBrush brush;
    private GameObjectBrush.BrushCell cell;

    private GameObjectBaseGen baseGen;
    private GameObjectBaseGen.GenCell cellGen;

    public Vector3Int initVect;
    public Tilemap tilemap;

    private void Start()
    {
        Debug.Log("Start TilemapGen");
        baseGen = (GameObjectBaseGen)ScriptableObject.CreateInstance(typeof(GameObjectBaseGen));
        
        cellGen = new GameObjectBaseGen.GenCell();
        cellGen.go = tileObj;
        Debug.Log("go to paint: " + cellGen.go.name);

        initVect = new Vector3Int(1, 1, 1);
        //Paint();
        Debug.Log("End TilemapGen");
    }

    [ContextMenu("Paint")]
    void Paint()
    {
        Debug.Log("Paint");
        baseGen.PaintGo(gridLayout, 
            parentLayer.transform,
            initVect, 
            cellGen.go, 
            cellGen.offset, 
            cellGen.scale, 
            cellGen.orientation,
            baseGen.anchor);
    }
    
}

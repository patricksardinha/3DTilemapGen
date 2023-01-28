using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Tilemaps;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


namespace UnityEditor.Tilemaps
{
    public class GameObjectBaseGen : GameObjectBrush
    {
        // Grid Component in Grid gameObject
        [Serializable]
        internal class HiddenGridLayout
        {
            public Vector3 cellSize = Vector3.one;
            public Vector3 cellGap = Vector3.zero;
            public GridLayout.CellLayout cellLayout = GridLayout.CellLayout.Rectangle;
            public GridLayout.CellSwizzle cellSwizzle = GridLayout.CellSwizzle.XYZ;
        }

        [SerializeField]
        [HideInInspector]
        internal HiddenGridLayout hiddenGridLayout = new HiddenGridLayout();

        [HideInInspector]
        public new GameObject hiddenGrid;


        // Tile palette cells property
        // Array of: GameObject to paint, Offset, Scale, Orientation 
        public new GenCell[] cells { get { return m_Cells; } }
        private GenCell[] m_Cells;

        // Tile palette size property
        public new Vector3Int size { get { return m_Size; } set { m_Size = value; } }
        private Vector3Int m_Size = new Vector3Int(1, 1, 1);

        // Tile palette pivot property
        public new Vector3Int pivot { get { return m_Pivot; } set { m_Pivot = value; } }
        private Vector3Int m_Pivot = new Vector3Int(0, 0, 0);

        // Tile palette anchor property
        public Vector3 anchor { get { return m_Anchor; } set { m_Anchor = value; } }
        public new Vector3 m_Anchor = new Vector3(0.5f, 0.5f, 0.5f);

        // Tile palette comp of cells property
        public class GenCell
        {
            // GameObject
            public GameObject go { get { return m_Go; } set { m_Go = value; } }
            private GameObject m_Go;
            
            // Offset
            public Vector3 offset { get { return m_Offset; } set { m_Offset = value; } }
            private Vector3 m_Offset = Vector3.zero;

            // Scale
            public Vector3 scale { get { return m_Scale; } set { m_Scale = value; } }
            private Vector3 m_Scale = Vector3.one;

            // Orientation
            public Quaternion orientation { get { return m_Orientation; } set { m_Orientation = value; } }
            private Quaternion m_Orientation = Quaternion.identity;

        }

        public GameObjectBaseGen()
        {
            Init();
        }

        public void Init()
        {
            Debug.Log("Init GameObjectBaseGen()");
        }

        private void OnEnable()
        {
            hiddenGrid = new GameObject();
            hiddenGrid.name = "(Paint on SceneRoot)";
            hiddenGrid.hideFlags = HideFlags.HideAndDontSave;
            hiddenGrid.transform.position = Vector3.zero;
            var grid = hiddenGrid.AddComponent<Grid>();
            grid.cellSize = hiddenGridLayout.cellSize;
            grid.cellGap = hiddenGridLayout.cellGap;
            grid.cellSwizzle = hiddenGridLayout.cellSwizzle;
            grid.cellLayout = hiddenGridLayout.cellLayout;
        }

        private void OnDisable()
        {
            DestroyImmediate(hiddenGrid);
        }

        public void PaintGo(GridLayout grid, Transform parent, Vector3Int position, GameObject go, Vector3 offset, Vector3 scale, Quaternion orientation, Vector3 anchor)
        {
            if (go == null)
                return;

            GameObject instance;
            if (PrefabUtility.IsPartOfPrefabAsset(go))
            {
                instance = (GameObject)PrefabUtility.InstantiatePrefab(go, parent != null ? parent.root.gameObject.scene : SceneManager.GetActiveScene());
                instance.transform.parent = parent;
            }
            else
            {
                instance = Instantiate(go, parent);
                instance.name = go.name;
                instance.SetActive(true);
                foreach (var renderer in instance.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            }

            Undo.RegisterCreatedObjectUndo(instance, "Paint GameObject");

            var cellSize = grid.cellSize;
            var cellStride = cellSize + grid.cellGap;
            cellStride.x = Mathf.Approximately(0f, cellStride.x) ? 1f : cellStride.x;
            cellStride.y = Mathf.Approximately(0f, cellStride.y) ? 1f : cellStride.y;
            cellStride.z = Mathf.Approximately(0f, cellStride.z) ? 1f : cellStride.z;
            var anchorRatio = new Vector3(
                anchor.x * cellSize.x / cellStride.x,
                anchor.y * cellSize.y / cellStride.y,
                anchor.z * cellSize.z / cellStride.z
            );
            instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position + anchorRatio));
            instance.transform.localRotation = orientation;
            instance.transform.localScale = scale;
            instance.transform.Translate(offset);
        }

    }
}


using UnityEditor;
using UnityEngine;

namespace Stray
{
    public enum CellType
    {
        Air,
        Empty,
        Blocked,
    }

    [System.Serializable]
    public class Cell
    {
        public Cell(Vector3 center)
        {
            this.center = center;
        }

        public Vector3 center;

        public bool isOccupied = false;
        public CellType type = CellType.Air;

        public Cell North;
        public Cell South;
        public Cell East;
        public Cell West;
    }

    public class Grid : MonoBehaviour
    {
        [Header("Dimensions")]
        [SerializeField] private uint width = 16;
        [SerializeField] private uint height = 16;
        [SerializeField] private uint depth = 16;

        [SerializeField] private Vector3 _cellSize = Vector3.one;

        [SerializeField] private Cell[,] _cells;


        [ContextMenu("Update Grid")]
        private void UpdateGrid()
        {
            // create the array
            _cells = new Cell[width, depth];

            // populate the grid with cells
            for (var x = 0; x < width; x++)
            {
                // no need for this just project a 2d grid downwards
                for (var z = 0; z < depth; z++)
                {
                    _cells[x, z] = new Cell(new Vector3(x, 0, z));
                }
            }


            // set the cell heights
            var rayOffset = Vector3.up * 10;

            foreach (var cell in _cells)
            {
                var start = cell.center + rayOffset;
                var ray = new Ray(start, Vector3.down);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    Debug.Log("Hit");
                    cell.center = new Vector3(cell.center.x, hit.point.y + _cellSize.y / 2f, cell.center.z);
                    cell.type = CellType.Empty;
                }
                else
                {
                    cell.type = CellType.Air;
                }

            }

            // set the cells neighbors
            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < depth; z++)
                {
                    var cell = _cells[x, z];

                    if (z + 1 < depth)
                    {
                        SetIfNotNull(ref cell.North, _cells[x, z + 1]);
                    }
                    if (z - 1 > -1)
                    {
                        SetIfNotNull(ref cell.South, _cells[x, z - 1]);
                    }

                    if (x - 1 > -1)
                    {
                        SetIfNotNull(ref cell.East, _cells[x - 1, z]);
                    }

                    if (x + 1 < width)
                    {
                        SetIfNotNull(ref cell.West, _cells[x + 1, z]);
                    }
                }
            }

        }

        private static void SetIfNotNull<T>(ref T target, T value)
        {
            if (value == null) return;
            target = value;
        }

        [SerializeField] private bool DrawGizmos = false;

        private void OnDrawGizmos()
        {
     
            if (!DrawGizmos) return;

            var size = new Vector3(width, height, depth);
            var center = Vector3.zero + (size / 2) - (_cellSize / 2);//transform.position;

            Gizmos.DrawWireCube(center, size);

            var box = Vector3.one * 0.5f;

            var gizmoSize = new Vector3(.5f, 0, .5f);
            var offset = Vector3.up / 2 + Vector3.up * 0.01f;

            /*for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < depth; z++)

                {
                    if (_cells[x, z].type != CellType.Empty) continue;
                    //Handles.DrawWireCube(cell.center, box);
                    Gizmos.DrawCube(_cells[x, z].center - offset, gizmoSize);
                }
            }*/

            /*var target = _cells[5, 5];
            Gizmos.color = Color.black;
            Gizmos.DrawCube(target.center, gizmoSize);

            Gizmos.color = Color.red;
            Gizmos.DrawCube(target.North.center, gizmoSize);

            Gizmos.color = Color.green;
            Gizmos.DrawCube(target.South.center, gizmoSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawCube(target.East.center, gizmoSize);

            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(target.West.center, gizmoSize);*/

        }
    }
}
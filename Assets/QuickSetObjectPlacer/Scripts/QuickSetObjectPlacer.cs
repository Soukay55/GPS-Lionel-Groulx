using UnityEngine;
using System.Collections;

public class QuickSetObjectPlacer : MonoBehaviour
{
    public static string QSName = "_QuickSet";

    [System.Serializable]
    public class QOPobjectGroup
    {
        public QOPgameObject[] GOs;
        public Transform GroupParent;
        public bool IsOpen;
        public string GroupName;
    }

    [System.Serializable]
    public class QOPgameObject
    {
        public bool IsSelected;
        public GameObject GO;
    }

    public bool HasBeenInitialized = false;

    public QOPobjectGroup[] ObjectGroups;

    public Vector2 SelectedObjectId = -1 * Vector2.one;

    public LayerMask RayMask;

    public Vector3 Offset;
    public Vector3 Rotation;
    public Vector3 Scale = Vector3.one;

    // vars snapping
    public bool SnapX = false;
    public bool SnapY = false;
    public bool SnapZ = false;
    public bool AutoSetX = false;
    public bool AutoSetY = false;
    public bool AutoSetZ = false;

    public float AutoXPos = 0;
    public float AutoYPos = 0;
    public float AutoZPos = 0;

    public int GridSize = 10;
    public bool DrawXY = false;
    public bool DrawXZ = false;
    public bool DrawYZ = false;

    private void OnDrawGizmos()
    {
        DrawGrid(GridSize, 1);
    }

    private void DrawGrid(int gridSize, float unitSize)
    {
        var gridByUnit = gridSize * unitSize;

        var gridPos = Vector3.zero;
        var gridUp = Vector3.up;
        var gridRight = Vector3.right;
        var gridForward = Vector3.forward;

        var oldCol = Gizmos.color;
        Gizmos.color = new Color(oldCol.r, oldCol.g, oldCol.b, .2f);

        if (DrawXY)
        {
            var vertPos = gridPos + gridUp * gridByUnit;
            var negVertPos = gridPos + gridUp * -gridByUnit;
            var horzPos = gridPos + gridRight * gridByUnit;
            var negHorzPos = gridPos + gridRight * -gridByUnit;
            // xy plane
            for (var i = 0; i <= gridSize; i++)
                if (i == 0)
                {
                    Gizmos.color = new Color(0, 1, 0, .3f);
                    Gizmos.DrawLine(negVertPos, vertPos);
                    Gizmos.color = new Color(1, 0, 0, .3f);
                    Gizmos.DrawLine(negHorzPos, horzPos);
                    Gizmos.color = new Color(oldCol.r, oldCol.g, oldCol.b, .2f);
                }
                else
                {
                    //xy plane
                    // vert lines
                    Gizmos.DrawLine(negVertPos + gridRight * i * unitSize, vertPos + gridRight * i * unitSize);
                    Gizmos.DrawLine(negVertPos + gridRight * -i * unitSize, vertPos + gridRight * -i * unitSize);
                    // horz lines
                    Gizmos.DrawLine(negHorzPos + gridUp * i * unitSize, horzPos + gridUp * i * unitSize);
                    Gizmos.DrawLine(negHorzPos + gridUp * -i * unitSize, horzPos + gridUp * -i * unitSize);
                }
        }

        if (DrawXZ)
        {
            var forPos = gridPos + gridForward * gridByUnit;
            var negForPos = gridPos + gridForward * -gridByUnit;
            var horzPos = gridPos + gridRight * gridByUnit;
            var negHorzPos = gridPos + gridRight * -gridByUnit;
            for (var i = 0; i <= gridSize; i++)
                if (i == 0)
                {
                    // xz
                    Gizmos.color = new Color(0, 0, 1, .3f);
                    Gizmos.DrawLine(negForPos, forPos);
                    Gizmos.color = new Color(1, 0, 0, .3f);
                    Gizmos.DrawLine(negHorzPos, horzPos);
                    Gizmos.color = new Color(oldCol.r, oldCol.g, oldCol.b, .2f);
                }
                else
                {
                    //xz plane
                    Gizmos.DrawLine(negForPos + gridRight * i * unitSize, forPos + gridRight * i * unitSize);
                    Gizmos.DrawLine(negForPos + gridRight * -i * unitSize, forPos + gridRight * -i * unitSize);

                    Gizmos.DrawLine(negHorzPos + gridForward * i * unitSize, horzPos + gridForward * i * unitSize);
                    Gizmos.DrawLine(negHorzPos + gridForward * -i * unitSize, horzPos + gridForward * -i * unitSize);
                }
        }

        if (DrawYZ)
        {
            var forPos = gridPos + gridForward * gridByUnit;
            var negForPos = gridPos + gridForward * -gridByUnit;
            var vertPos = gridPos + gridUp * gridByUnit;
            var negVertPos = gridPos + gridUp * -gridByUnit;
            // yz plane
            for (var i = 0; i <= gridSize; i++)
                if (i == 0)
                {
                    // yz
                    Gizmos.color = new Color(0, 0, 1, .3f);
                    Gizmos.DrawLine(negForPos, forPos);
                    Gizmos.color = new Color(0, 1, 0, .3f);
                    Gizmos.DrawLine(negVertPos, vertPos);
                    Gizmos.color = new Color(oldCol.r, oldCol.g, oldCol.b, .2f);
                }
                else
                {
                    //xz plane
                    Gizmos.DrawLine(negForPos + gridUp * i * unitSize, forPos + gridUp * i * unitSize);
                    Gizmos.DrawLine(negForPos + gridUp * -i * unitSize, forPos + gridUp * -i * unitSize);

                    Gizmos.DrawLine(negVertPos + gridForward * i * unitSize, vertPos + gridForward * i * unitSize);
                    Gizmos.DrawLine(negVertPos + gridForward * -i * unitSize, vertPos + gridForward * -i * unitSize);
                }
        }


        Gizmos.color = oldCol;
    }
}
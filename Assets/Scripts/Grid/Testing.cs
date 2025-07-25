using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridSystem<GridObject> gridSystem;

    [SerializeField]
    private Transform gridDebugObjectPrefab;

    [SerializeField]
    private Unit unit;

    private void Start()
    {
        // gridSystem = new GridSystem(10, 10, 2f);
        // gridSystem.CreateDebugObject(gridDebugObjectPrefab);
    }

    private void Update()
    {
        // Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetPosition()));



        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPos = LevelGrid.Instance.GetGridPosition(
                MouseWorld.GetPosition()
            );

            GridPosition startGridPosition = new(0, 0);

            var gridPosList = PathFinding.Instance.FindPath(startGridPosition, mouseGridPos);

            for (int i = 0; i < gridPosList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPosList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPosList[i + 1]),
                    Color.white,
                    10f
                );
            }
        }

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     GridSystemVisual.Instance.HideAllGridPosition();
        //
        //     GridSystemVisual.Instance.ShowGridPositionList(
        //         unit.GetMoveAction().GetValidActionGridPostionList()
        //     );
        // }
    }
}

// public class MyClass<T>
// {
//     private T i;
//
//     public MyClass(T i)
//     {
//         this.i = i;
//         Debug.Log(i);
//     }
// }

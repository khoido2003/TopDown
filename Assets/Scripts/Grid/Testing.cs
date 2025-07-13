using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridSystem gridSystem;

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
            unit.GetMoveAction().GetValidActionGridPostionList();
        }
    }
}

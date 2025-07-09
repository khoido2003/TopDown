using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private Camera mainCamera;
    private static MouseWorld Instance;

    [SerializeField]
    private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Testing code
        // transform.position = MouseWorld.GetPosition();
    }

    /// <summary>
    /// Find the mouse point in the game world on specific layer
    /// </summary>
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, float.MaxValue, Instance.mousePlaneLayerMask);

        return raycastHit.point;
    }
}

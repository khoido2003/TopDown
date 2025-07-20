using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;

    public void Show(Material material)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = material;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}

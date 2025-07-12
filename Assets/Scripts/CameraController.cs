using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera cinemachineCamera;
    private CinemachinePositionComposer cinemachinePositionComposer;

    private float rotationSpeed = 50f;
    private float moveSpeed = 5f;

    private const float MIN_OFFSET_Y = 2f;
    private const float MAX_OFFSET_Y = -2f;

    private void Start()
    {
        cinemachinePositionComposer = cinemachineCamera.GetComponent<CinemachinePositionComposer>();
    }

    private void Update()
    {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;

        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        Vector3 currentRotation = cinemachineCamera.transform.eulerAngles;
        Vector3 targetRotationEuler = currentRotation;

        if (Input.GetKey(KeyCode.Q))
        {
            targetRotationEuler.y = currentRotation.y - rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            targetRotationEuler.y = currentRotation.y + rotationSpeed * Time.deltaTime;
        }

        cinemachineCamera.transform.eulerAngles = Vector3.Lerp(
            cinemachineCamera.transform.eulerAngles,
            targetRotationEuler,
            Time.deltaTime * rotationSpeed
        );
    }

    private void ZoomCamera()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        // Only zoom if the scroll input is not approximately zero
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            float newZoomOffsetY = cinemachinePositionComposer.TargetOffset.y;
            float zoomStep = 1f;

            // Invert zoom to match common convention (scroll up = zoom in)
            newZoomOffsetY -= scrollDelta * zoomStep;

            cinemachinePositionComposer.TargetOffset.y = Mathf.Clamp(
                newZoomOffsetY,
                MAX_OFFSET_Y,
                MIN_OFFSET_Y
            );
        }
    }
}

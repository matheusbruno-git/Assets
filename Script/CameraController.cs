using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer framingTransposer;

    private void Start()
    {
        // Get the Framing Transposer from the Virtual Camera
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void AdjustCamera(float newDistance)
    {
        // Dynamically adjust the camera distance
        framingTransposer.m_CameraDistance = newDistance;
    }
}

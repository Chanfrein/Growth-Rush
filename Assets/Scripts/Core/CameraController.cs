using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float XAxisRotationSpeed = 500f;
    [SerializeField] float YAxisRotationSpeed = 100f;


    [SerializeField] GameObject playerCamera;
    CinemachineFreeLook freeLookComponent;

    void Start()
    {
        freeLookComponent = playerCamera.GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            freeLookComponent.m_XAxis.m_MaxSpeed = XAxisRotationSpeed;
        }

        if(Input.GetMouseButtonUp(1))
        {
            freeLookComponent.m_XAxis.m_MaxSpeed = 0;
        }

        if(Input.mouseScrollDelta.y != 0)
        {
            freeLookComponent.m_YAxis.m_MaxSpeed = 100f;
        } 
    }
}

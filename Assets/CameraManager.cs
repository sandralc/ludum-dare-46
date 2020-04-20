using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Camera myCamera = null;
    [SerializeField] private CinemachineVirtualCamera cinemachineCam = null;
    [SerializeField] private GameObject followPointer = null;

    [SerializeField] private GameObject unitGameObject = null;

    Vector3 mousePosition = new Vector3();
    Vector3 followPointerPosition = new Vector3();

    private void Update()
    {
        mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        followPointerPosition = (mousePosition + unitGameObject.transform.position) / 2;
        followPointerPosition = (followPointerPosition + unitGameObject.transform.position) / 2;
        followPointer.transform.position = followPointerPosition;
    }

}

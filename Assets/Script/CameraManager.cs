using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineVirtualCamera idleCam;
    [SerializeField] private CinemachineVirtualCamera followCam;
    private void Awake() {
        SwitchToIdleCam();
    }
    public void SwitchToIdleCam () {
        idleCam.enabled = true;
        followCam.enabled = false;
    }
    public void SwitchToFollowCam (Transform followTransform) {
        followCam.Follow = followTransform;
        idleCam.enabled = false;
        followCam.enabled = true;
    }
}

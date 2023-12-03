using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] playerController player;
    [SerializeField] Vector3 cameraHeight;
    [SerializeField] Camera cam;


    // Update is called once per frame
    void Update()
    {
        cam.transform.position = player.transform.position + cameraHeight;
    }
}

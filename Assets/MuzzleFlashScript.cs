using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashScript : MonoBehaviour
{
    public Transform gunPoint;
    public GameObject muzzleFlash;

    void Update()
    {
        muzzleFlash.transform.position = gunPoint.position;
    }
}

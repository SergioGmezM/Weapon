using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] Transform gunBarrelTransform;
    [SerializeField] Transform satelliteWeaponTransform;

    [SerializeField] GameObject LaserBeam;

    private bool isFiring = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isFiring = true;
            FireLaserBeam();
        }
        else
        {
            isFiring = false;
        }

        if (!isFiring)
        {
            LaserBeam.SetActive(false);
        }
    }

    private void FireLaserBeam()
    {
        RaycastHit hit;

        if (Physics.Raycast(gunBarrelTransform.position, gunBarrelTransform.forward, out hit))
        {
            LineRenderer lr = LaserBeam.GetComponent<LineRenderer>();
            lr.SetPositions(new Vector3[2] { satelliteWeaponTransform.position, hit.point });
            LaserBeam.SetActive(true);
        }
        else
        {
            isFiring = false;
        }
    }
}

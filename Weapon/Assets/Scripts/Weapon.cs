using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] Transform gunBarrelTransform;
    [SerializeField] Transform satelliteWeaponTransform;

    [Header("Laser Beam")]
    [SerializeField] GameObject laserPointer;
    [SerializeField] GameObject laserBeam;
    [SerializeField] AudioClip laserBeamSoundBeginning;
    [SerializeField] AudioClip laserBeamSoundMiddle;
    [SerializeField] AudioClip laserBeamSoundEnd;
    private float laserChargingTimer = 0f;
    private bool chargeTimeReached = false;
    private bool canShootLaserBeam = false;


    private AudioSource playerAudio;
    private bool isFiring = false;

    private void Awake()
    {
        playerAudio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isFiring)
            {
                isFiring = true;
                StartCoroutine(BlinkEffect());
                PlayLaserBeamBeginningSound();
            }

            FireLaserBeam();
        }
        else
        {
            if (isFiring)
            {
                StopLaserBeamMiddleSound();
            }

            isFiring = false;
        }

        if (!isFiring)
        {
            laserPointer.SetActive(true);

            canShootLaserBeam = false;
            laserChargingTimer = 0f;
            chargeTimeReached = false;
            laserBeam.SetActive(false);
        }
    }

    private void FireLaserBeam()
    {
        if (!chargeTimeReached)
        {
            laserChargingTimer += Time.deltaTime;
        }

        if (!chargeTimeReached && laserChargingTimer > 0.9f)
        {
            canShootLaserBeam = true;
            PlayLaserBeamMiddleSound();
        }

        if (canShootLaserBeam)
        {
            StopCoroutine(BlinkEffect());
            chargeTimeReached = true;
            laserPointer.SetActive(true);

            RaycastHit hit;

            if (Physics.Raycast(gunBarrelTransform.position, gunBarrelTransform.forward, out hit))
            {
                LineRenderer lr = laserBeam.GetComponent<LineRenderer>();
                lr.SetPositions(new Vector3[2] { satelliteWeaponTransform.position, hit.point });
                laserBeam.SetActive(true);
            }
            else
            {
                StopLaserBeamMiddleSound();
                isFiring = false;
            }
        }   
    }

    private IEnumerator BlinkEffect()
    {
        while (true)
        {
            laserPointer.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            laserPointer.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void PlayLaserBeamBeginningSound()
    {
        playerAudio.PlayOneShot(laserBeamSoundBeginning, 0.7f);
    }

    private void PlayLaserBeamMiddleSound()
    {
        playerAudio.clip = laserBeamSoundMiddle;
        playerAudio.loop = true;
        playerAudio.Play();
    }

    private void StopLaserBeamMiddleSound()
    {
        playerAudio.Stop();
        PlayLaserBeamEndSound();
    }

    private void PlayLaserBeamEndSound()
    {
        playerAudio.PlayOneShot(laserBeamSoundEnd, 0.7f);
    }

}

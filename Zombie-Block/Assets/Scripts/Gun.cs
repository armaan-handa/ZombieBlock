using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int gunType;

    public float damage = 10f;
    public float range = 100f;
    public int fireRate = 20;
    public float hipDeviation = 0.022f;
    public float aimDeviation = 0.011f;
    public Camera mainCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject bulletHole;
    public Shooting shooting;
    float nextTimeToFire = 0f;

    // Update is called once per frame
    void Update()
    {
        switch(gunType)
        {
            case 0:
                FullAuto();
                break;
            case 1:
                SemiAuto();
                break;
            default:
                break;
        }
    }

    void FullAuto()
    {
        if (Input.GetButton("Fire") && Time.time >=nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + 1f/fireRate;
        }
    }
    void SemiAuto()
    {
        if (Input.GetButtonDown("Fire") && Time.time >=nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + 1f/fireRate;
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        
        RaycastHit hit;
        float deviation = hipDeviation;
        if(shooting.isAiming)
        {
            deviation = aimDeviation;
        }
        Vector3 direction = mainCam.transform.forward;
        direction.x += Random.Range(-deviation, deviation);
        direction.y += Random.Range(-deviation, deviation);
        direction.z += Random.Range(-deviation, deviation);
        if (Physics.Raycast(mainCam.transform.position, direction, out hit))
        {
            Debug.Log(hit.transform.name);
            float BHTime = 60f;

            Health health = hit.transform.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                BHTime = 0f;
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject bulletHoleGO = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
            Destroy(bulletHoleGO, BHTime);
        }
    }
}

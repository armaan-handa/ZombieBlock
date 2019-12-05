using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    /*
     *  0: Full Auto
     *  1: Semi Auto
     */
    public int gunType;

    // weapon stats
    public float damage = 10f;
    public float range = 100f;
    public int fireRate = 20;
    // recoil stats
    public float hipDeviation = 0.022f;
    public float aimDeviation = 0.011f;
    public float recoil;
    public float maxRecoil = 1f;
    public float recoilStrength = 0.1f;
    // ammo stats
    public int magCapacity = 32;
    public float reloadTime = 2f;
    public Text ammoText;
    // scripting objects
    public Camera mainCam;
    // animation
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject bulletHole;
    public GameObject reloadGunGO;
    public Animator animator;
    public Shooting shooting;
    // ammunition
    public int magAmount;
    // private variables
    float BHTime = 60f;
    float nextTimeToFire = 0f;
    float recoilRecoverySpeed = 500f;
    SkinnedMeshRenderer renderer;

    private void Start() {
        // initialize ammp and renderer
        magAmount = magCapacity; 
        renderer = GetComponent<SkinnedMeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        // decide which firing mode to use
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

        ammoText.text = magAmount.ToString(); // update ammo counter on screen
    }

    void FullAuto()
    {
        if (Time.time >= nextTimeToFire)
        {
            // handle reload animations 
            renderer.enabled = true;
            reloadGunGO.SetActive(false);
            animator.SetBool("isReloading", false);
        }
        // reload is pressed and magazine is not full
        if (Input.GetButtonDown("Reload") && magAmount != magCapacity)
        {
            Reload();
        }

        // fire is pressed and wait time has elapsed
        if (Input.GetButton("Fire") && Time.time >=nextTimeToFire)
        {
            Shoot();
            // if magazine is empty, reload
            if (magAmount <= 0)
            {
                Reload();
            }
        }
        // add recoil
        if (!Input.GetButton("Fire"))
        {
            recoil = Mathf.Lerp(maxRecoil, 0, Time.deltaTime * recoilRecoverySpeed);
        }
    }
    void SemiAuto()
    {
        if (Time.time >= nextTimeToFire)
        {
            // handle reload animations 
            renderer.enabled = true;
            reloadGunGO.SetActive(false);
            animator.SetBool("isReloading", false);
        }
        // reload is pressed and magazine is not full
        if (Input.GetButtonDown("Reload") && magAmount != magCapacity)
        {
            Reload();
        }

        // fire is pressed and wait time has elapsed
        if (Input.GetButtonDown("Fire") && Time.time >=nextTimeToFire)
        { 
            Shoot();
            // if magazine is empty, reload
            if (magAmount <= 0)
            {
                Reload();

            }
        }
        // add recoil
        if (!Input.GetButton("Fire"))
        {
            recoil = Mathf.Lerp(maxRecoil, 0, Time.deltaTime * recoilRecoverySpeed);
        }
    }

    void Shoot()
    {
        // set next firing time
        nextTimeToFire = Time.time + 1f/fireRate;

        muzzleFlash.Play();
        
        RaycastHit hit;
        float deviation = hipDeviation;
        
        // set the bullet spread amount depending on wether its hipfire or ADS
        if(shooting.isAiming)
        {
            deviation = aimDeviation;
            recoil += recoilStrength/2;
            recoil = (recoil > maxRecoil ? maxRecoil : recoil);
        }
        else
        {
            recoil += recoilStrength;
            recoil = (recoil > maxRecoil ? maxRecoil : recoil);
        }
        
        // get forward vector + random deviation for bullet spread
        Vector3 direction = mainCam.transform.forward;
        direction.x += Random.Range(-deviation, deviation);
        direction.y += Random.Range(-deviation, deviation);
        direction.z += Random.Range(-deviation, deviation);
        
        //create raycast hit from camera in direction of shot 
        if (Physics.Raycast(mainCam.transform.position, direction, out hit))
        {
            Debug.Log(hit.transform.name);
            BHTime = 60f; // how long bullethole will last

            Health health = hit.transform.GetComponent<Health>();   // get health script of hit object
            if (health != null)
            {
                // take damage and dont create bullethole
                health.TakeDamage(damage);
                BHTime = 0f;
            }

            // Create impact effect and bullet hole
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject bulletHoleGO = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            // Destroy effect after certain time
            Destroy(impactGO, 2f);
            Destroy(bulletHoleGO, BHTime);
        }
        magAmount--;    // decrement ammo
    }

    void Reload()
    {
        magAmount = magCapacity;    // reset ammo
        animator.SetBool("isReloading", true);  // play reload animation
        renderer.enabled = false;   // make gun in hand invisible
        nextTimeToFire = Time.time + reloadTime;    // wait for reload to finish
        reloadGunGO.SetActive(true);  // enable animated gun
    }
}

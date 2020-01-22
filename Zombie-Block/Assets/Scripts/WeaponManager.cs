using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject Primary;
    public GameObject Secondary;
    public GameObject PrimaryStowed;
    public GameObject SecondaryStowed;
    public Animator animator;
    bool currentPrimary = true;
    float switchTime;
    bool switching = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch Weapon"))
        {
            SwitchWeapon();
        }
        if(Time.time >= switchTime && switching)
        {
            if (currentPrimary)
            {
                Primary.SetActive(false);
                PrimaryStowed.SetActive(true);
                Secondary.SetActive(true);
                SecondaryStowed.SetActive(false);
                currentPrimary = false;
            }
            else
            {
                Primary.SetActive(true);
                PrimaryStowed.SetActive(false);
                Secondary.SetActive(false);
                SecondaryStowed.SetActive(true);
                currentPrimary = true;
            }
            switching = false;
        }
    }

    void SwitchWeapon()
    {   
        if(!animator.GetBool("isReloading"))
        {
            if (currentPrimary)
            {
                animator.SetFloat("Primary", 0);
                animator.SetTrigger("GetSecondary");
            }
            else
            {
                animator.SetFloat("Primary", 1);
                animator.SetTrigger("GetPrimary");
            }
            switchTime = Time.time + 0.5f;
            switching = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float camSpeed = 2f;
    public PlayerController movement;
    public Animator animator;
    public Camera cam;
    public GameObject HFCrossHair;
    public GameObject ADSCrossHair;
    public bool isAiming;
    Vector3 HFCamPosition = new Vector3(0f, 2.5f, -13f);
    Vector3 ADSCamPosition = new Vector3 (2f, 2.2f, -7f);

    // Update is called once per frame
    void Update()
    {
        Vector3 camPosition;
        float animShootMode;
        if(Input.GetButton("Aim") && !movement.jumping)
        {
            isAiming = true;
            animShootMode = 1f;
            camPosition = ADSCamPosition;
            HFCrossHair.SetActive(false);
            ADSCrossHair.SetActive(true);
        }
        else 
        {
            isAiming = false;
            animShootMode = 0f;
            camPosition = HFCamPosition;
            HFCrossHair.SetActive(true);
            ADSCrossHair.SetActive(false);
        }

        // animShootMode  = Mathf.Lerp((animShootMode == 0 ? 1 : 0), animShootMode, armSpeed * Time.deltaTime);
        animator.SetFloat("ShootMode", animShootMode);
        
        float curFOV = cam.fieldOfView;
        float desiredFOV = (animShootMode == 1 ? 50 : 80);
        desiredFOV = (movement.running ? 90 : desiredFOV);
        // float curX = cam.transform.position.x;
        // float desiredX = (animShootMode == 1 ? 0 : -1);
        cam.fieldOfView = Mathf.Lerp(curFOV, desiredFOV, camSpeed * Time.deltaTime);
        // cam.transform.position = new Vector3(Mathf.Lerp(curX, desiredX, camSpeed * Time.deltaTime), cam.transform.position.y, cam.transform.position.z);
    }
}

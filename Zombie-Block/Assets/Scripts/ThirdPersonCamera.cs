using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
   
    public float horSens = 10f;
    public float verSens = 10f;
    public Transform target;
    public Transform player;
    public Transform head;
    public Animator animator;
    public PlayerController movement;
    public Gun gun;
    public Vector2 pitchMinMax = new Vector2 (-40, 75);
    
    float mouseY;
    float mouseX;

    // Update is called once per frame
    void LateUpdate()
    {
        mouseY -= Input.GetAxis("Mouse Y") * verSens;
        mouseY = Mathf.Clamp (mouseY, pitchMinMax.x, pitchMinMax.y);
        mouseX += Input.GetAxis("Mouse X") * horSens;
        
        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        head.rotation = Quaternion.Euler(mouseY, mouseX, 0); 
        player.rotation = Quaternion.Euler(0, mouseX, 0);

        transform.LookAt(target);

        animator.SetFloat("ShootAngle", -mouseY - 2);
    }
}

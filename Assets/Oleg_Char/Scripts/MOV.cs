using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOV : MonoBehaviour
{
    private new Rigidbody  rigidbody;

    private Vector3 lookpos;
    
    Animator animator;
    public float speed = 4;
    Transform cam;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;
    float VelocityXAmount;
    float VelocityZAmount;


    void Start()
    {
        rigidbody=GetComponent<Rigidbody>();
        animator=GetComponent<Animator>();
        cam=Camera.main.transform;
    }
    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 50)) {
            lookpos=hit.point;
        }
        Vector3 lookdir = lookpos - transform.position;
        lookdir.y=0;

        transform.LookAt(transform.position + lookdir, Vector3.up);
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(cam!=null)
        {
            camForward= Vector3.Scale(cam.up, new Vector3(1,0,1)).normalized;
            move = vertical*camForward+horizontal*cam.right;
        }
        else
        {
            move= vertical*Vector3.forward+horizontal*Vector3.right;
        }

        if(move.magnitude>1)
        {
            move.Normalize();
        }

        Move(move);

        Vector3 movement= new Vector3(horizontal,0, vertical);

        rigidbody.AddForce(movement*speed/Time.deltaTime);

              
    }

    void Move(Vector3 move)
    {
        if(move.magnitude>1){
            move.Normalize();
        }

        this.moveInput=move;

        ConvertMoveInput();
        UpdateAnimator();
    }
    void ConvertMoveInput(){
        Vector3 localMove=transform.InverseTransformDirection(moveInput);
        VelocityXAmount=localMove.x;
        VelocityZAmount=localMove.z;
    }
    void UpdateAnimator(){
        animator.SetFloat("VelocityX", VelocityXAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityZ", VelocityZAmount, 0.1f, Time.deltaTime);
    }
}
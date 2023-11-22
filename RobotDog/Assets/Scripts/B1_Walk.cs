using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class B1_Walk : MonoBehaviour
{
    private enum MovementType
    {
        Nav,
        Manual
    }

    private MovementType movementType  = MovementType.Manual;

    [SerializeField] private TMPro.TMP_Dropdown movementTypeDropdown;
    [SerializeField] private Transform[] targets;
    [SerializeField] private Transform lookTarget;
    

    [SerializeField] private float aimHeight, heightSetSpeed;
    [SerializeField] private float planeSetSpeed, planeNormalYInvalidateAmt;
    
    [SerializeField] private B1_Inputs inputs;

    void Update()
    {
        switch(movementType)
        {
            case MovementType.Manual:  GetComponent<NavAgent>().enabled = false;
            inputs.ApplyHor(Input.GetAxis("Horizontal"));
        inputs.ApplyVert(Input.GetAxis("Vertical"));
        inputs.ApplyPitch(Input.GetAxis("Pitch"));
        inputs.ApplyYaw(Input.GetAxis("Yaw"));
        inputs.ApplyRoll(Input.GetAxis("Roll"));
        break;

            case MovementType.Nav: GetComponent<NavAgent>().enabled = true;
                                    break;

        }
        StandardAdjustments();
    }
    private void StandardAdjustments()
    {
        HandleBodyRotation(FootPlane());

        float avgY = 0;
        foreach(Transform y in targets)
            avgY += y.position.y;
        avgY /= 4f;

        Debug.DrawLine(transform.position, new Vector3(transform.position.x, avgY, transform.position.z), Color.red);

        if((transform.position.y - avgY) > aimHeight+0.1f)
            transform.position += heightSetSpeed * Time.deltaTime * Vector3.up * -1f;
        else if((transform.position.y - avgY) < aimHeight)
            transform.position += heightSetSpeed * Time.deltaTime * Vector3.up;
    }

    public bool isFacingWorldZ, isFacingWorldX;
    private void HandleBodyRotation(Vector3 n)
    {
        Quaternion oldLocalRot = transform.localRotation;
        
        isFacingWorldZ = Vector3.Dot(transform.forward, Vector3.forward) >= 0;
        isFacingWorldX = Vector3.Dot(transform.forward, Vector3.right) >= 0;
        float localYRot = transform.localRotation.eulerAngles.y;
        transform.up = n.normalized;
        Quaternion newLocalRot = Quaternion.Euler((transform.localRotation.eulerAngles.x*(isFacingWorldZ?1f:-1f) + transform.localRotation.eulerAngles.z*(isFacingWorldX?-1f:1f)), localYRot, 0f); //transform.localRotation.eulerAngles.x * (isFacingWorldX?1f:-1f));
        transform.localRotation = Quaternion.Lerp(oldLocalRot, newLocalRot, planeSetSpeed * Time.deltaTime);       
    }

    Vector3 FootPlane()
    {
        Vector3 v1 = (targets[1].position - targets[0].position).normalized;
        Vector3 v2 = (targets[2].position - targets[0].position).normalized;
        Vector3 v3 = (targets[3].position - targets[0].position).normalized;

        Vector3 n1 = Vector3.Cross(v1, v2).normalized;
        Vector3 n2 = Vector3.Cross(v3, v2).normalized;

        Vector3 final = (n1 + n2).normalized;

        if(final.y < 0f)
            final *= -1f;

        return final + planeNormalYInvalidateAmt*Vector3.up;

    }

    public void SetDropdown()
    {
        switch(movementTypeDropdown.value)
        {
            case 0: movementType = MovementType.Manual; break;
            case 1: movementType = MovementType.Nav; break;

        }
    }

    private void OnDrawGizmos() {
        Debug.DrawRay(transform.position, FootPlane(), Color.white);
    }
}

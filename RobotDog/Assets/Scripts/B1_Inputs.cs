using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class B1_Inputs : MonoBehaviour
{
    
    [SerializeField] private float sidewaysMoveSpeed;
    public float straightMoveSpeed;
    public float yawRotateSpeed;
    [SerializeField] private float otherRotateSpeed;

    [SerializeField] private TMP_Text y, p, r, v, h;
    [SerializeField] private Slider ys, ps, rs, vs, hs;
    
    public void ApplyYaw(float Magnitude)
    {
        transform.eulerAngles += new Vector3(0f, yawRotateSpeed * Magnitude, 0f) * Time.deltaTime;
        ys.value = Magnitude;
        y.text = "" + Magnitude * yawRotateSpeed;
    }
    public void ApplyPitch(float Magnitude)
    {
        transform.eulerAngles += new Vector3(otherRotateSpeed * Magnitude, 0f, 0f) * Time.deltaTime;
        ps.value = Magnitude;
        p.text = "" + Magnitude * otherRotateSpeed;
    }
    public void ApplyRoll(float Magnitude)
    {
        transform.eulerAngles += new Vector3(0f, 0f, otherRotateSpeed * Magnitude) * Time.deltaTime;
        rs.value = Magnitude;
        r.text = "" + Magnitude * otherRotateSpeed;
    }

    public void ApplyVert(float Magnitude)
    {
        transform.position += transform.forward * straightMoveSpeed * Time.deltaTime * Magnitude;
        vs.value = Magnitude;
        v.text = "" + Magnitude * straightMoveSpeed;
        
    }
    public void ApplyHor(float Magnitude)
    {
        transform.position += transform.right * sidewaysMoveSpeed * Time.deltaTime * Magnitude;
        hs.value = Magnitude;
        h.text = "" + Magnitude * sidewaysMoveSpeed;
    }
}

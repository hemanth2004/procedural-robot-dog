using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1FootSolver : MonoBehaviour
{
    public bool isUp = false;
    public Color DebugColor;
    public B1FootSolver syncedFoot;

    [SerializeField] private float raycastSideOffset, raycastLongitudinalOffset, longitudinalStepDistance, lateralStepDistance, lerpDuration, footHeight;
    [SerializeField] private Transform target, body;
    

    private Vector3 newPosition;
    private RaycastHit hit;
    
    private Vector3 horPlane = new Vector3(1f,0f,1f);
    private void Update()
    {   

        Ray ray = new Ray(transform.position + transform.right*raycastSideOffset + transform.forward*raycastLongitudinalOffset, Vector3.down);

        if(Physics.Raycast(ray, out  hit, 15))
        {
            Vector3 dirn = (target.position - hit.point);
            if(
                (Math.Abs(Vector3.Dot(transform.forward, new Vector3(dirn.x, 0f, dirn.z))) > longitudinalStepDistance
                || Math.Abs(Vector3.Dot(transform.right, new Vector3(dirn.x, 0f, dirn.z))) > lateralStepDistance) && !syncedFoot.isUp)

                StartCoroutine(ChangeTarget(hit.point));
        }
    }

    private IEnumerator ChangeTarget(Vector3 newTarget)
    {
        isUp = true;
        Vector3 oldTarget = target.position;
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            
            float t = timeElapsed / lerpDuration;

            Vector3 lerpedPosition = Vector3.Lerp(oldTarget, newTarget, t);

            Vector3 newFootTarget = lerpedPosition - oldTarget;
            lerpedPosition.y += Mathf.Sin(t * Mathf.PI) * footHeight;

            target.position = lerpedPosition;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the target reaches the final position exactly
        target.position = newTarget;
        isUp = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hit.point, 0.1f);
        Debug.DrawLine(hit.point, target.position, Color.red);
    }

    
}

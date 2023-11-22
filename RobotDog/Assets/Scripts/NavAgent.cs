using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour
{

    public B1_Inputs inputs;
    public Transform target;
    public NavMeshAgent agent;
    public NavMeshPath path;
    [SerializeField] private float minTurnThreshold, minStopThreshold;
    [SerializeField] private TMPro.TMP_Text status;

    private Camera cam;

    void Awake()
    {
        path = new NavMeshPath();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        

        bool pathSuccess = NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        if(pathSuccess)
        {
            int size =  path.corners.Length;
            for(int i = 0; i<size-1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i+1], Color.green);
            }
        }
        MoveAlongPath();
    }
    private void MoveAlongPath()
    {
        if(path.corners.Length < 2)
            return;
        
        
        Vector3 nextVec = path.corners[1] - path.corners[0];
        float distTillNext = Vector3.Distance(transform.position, target.position);
        if(distTillNext > minStopThreshold)
        {
            if(Vector3.Angle(XZ(transform.forward) , XZ(nextVec)) <= minTurnThreshold)
            {
                inputs.ApplyVert(1f);
                status.text = ("Moving. Dist till next point: " + distTillNext);
            }
            else
            {
                int dirn = (Vector3.Dot(transform.right, (path.corners[1] - path.corners[0]).normalized) > 0) ? 1 : -1;
                inputs.ApplyYaw((float) dirn);
                status.text = ("Correcting Dirn. Angle left (deg): " + (Vector3.Angle(XZ(transform.forward) , XZ(nextVec)) - minTurnThreshold));
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(target.position, 0.2f);
    }
    
    private Vector3 GetMousePoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition - new Vector3(0,100,0));
        Physics.Raycast(ray, out RaycastHit hit);
        return hit.point;


    }

    public Vector3 XZ(Vector3 vec)
    {
       return new Vector3(vec.x,0,vec.z);
    }
}

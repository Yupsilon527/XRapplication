using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRFingerConnector : MonoBehaviour
{
    public float MoveSpeed = 1;
    public float AngSpeed = 1;

    Rigidbody rBody;
    public GameObject Master;
    public GameObject Slave;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        SnapToPos();
    }
    private void OnValidate()
    {
        SnapToPos();
    }
    void SnapToPos()
    {
        Vector3 deltaPos = (Master.transform.position - Slave.transform.position);
        transform.localScale = new Vector3(10, deltaPos.magnitude * .5f, 10);
        transform.up = deltaPos;
        transform.position = Master.transform.position - deltaPos / 2f;
    }
    void PhysicsPos()
    {
        Vector3 vA = Master.transform.position;
        Vector3 vB = Slave.transform.position;
        Vector3 desiredPos = (vA + vB) * .5f;
        rBody.velocity = (desiredPos-transform.position) * MoveSpeed;


        //Vector3 axis = Vector3.Cross(vB, vA).normalized;
        //float angle = Mathf.Acos(Vector3.Dot(vA,vB)) * Mathf.Rad2Deg;

        Quaternion desiredRot =  Quaternion.LookRotation( transform.forward, vA - vB);
        desiredRot*= Quaternion.Inverse(transform.rotation);
        
        desiredRot.ToAngleAxis(out float angle, out Vector3 axis);
        rBody.angularVelocity = axis * angle * Mathf.Deg2Rad * AngSpeed;
    }
}

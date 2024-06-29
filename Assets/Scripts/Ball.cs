using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class Ball : MonoBehaviour
{

    private Rigidbody rg;
    private GameObject dirObj;
    public bool isForceOK = false;
    bool isDartRotating = false;
    bool isDartReadyToShoot = true;
    ARSessionOrigin aRSession;
    GameObject ARCam;

    public Collider dartFrontCollider;
    // Start is called before the first frame update
    void Start()
    {
        aRSession = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
        ARCam = aRSession.transform.Find("AR Camera").gameObject;
        rg = gameObject.GetComponent<Rigidbody>();
        dirObj = GameObject.Find("DartThrowPoint");
    }

    private void FixedUpdate()
    {
        if (isForceOK)
        {
            dartFrontCollider.enabled = true;
            StartCoroutine(InitDartDestroyVFX());
            GetComponent<Rigidbody>().isKinematic = false;
            isForceOK = false;
            isDartRotating = true;
        }
        //Add Force
        rg.AddForce(dirObj.transform.forward * (12f + 6f) * Time.deltaTime, ForceMode.VelocityChange);
        //Dart ready
        if (isDartReadyToShoot)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 20f);
        }
        //Dart rotating 
        if (isDartRotating)
        {
            isDartReadyToShoot = false;
            transform.Rotate(Vector3.forward * Time.deltaTime
            * 400f);
        }

    }

    IEnumerator InitDartDestroyVFX()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
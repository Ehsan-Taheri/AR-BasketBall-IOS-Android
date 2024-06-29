using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class BallController : MonoBehaviour
{
    public GameObject DartPrefab;

    public Transform DartThrowPoint;

    ARSessionOrigin aRSession;

    GameObject ARCam;

    private GameObject DartTemp;

    private Rigidbody rb;
    void OnEnable()
    {

        PlaceObjectOnPlane.HoopPlaced += DartsInit;
    }

     

    void OnDisable()
    {
        PlaceObjectOnPlane.HoopPlaced -= DartsInit;
    }

     
    // Start is called before the first frame update
    void Start()
    {
        aRSession = GameObject.Find("XR Origin").GetComponent<ARSessionOrigin>();
        ARCam = GameObject.Find("Main Camera").gameObject;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Ball"))
                {
                    //Disable back touch Collider from dart
                    raycastHit.collider.enabled = false;
                    DartTemp.transform.parent = aRSession.transform;
                }
            }

        }

    }


    void DartsInit()
    {
        StartCoroutine(WaitAndSpawnDart());
    }


    public IEnumerator WaitAndSpawnDart()
    {

        yield return new WaitForSeconds(0.1f);
        DartTemp = Instantiate(DartPrefab, DartThrowPoint.position, ARCam.transform.localRotation);
        DartTemp.transform.parent = ARCam.transform;
        rb = DartTemp.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
}


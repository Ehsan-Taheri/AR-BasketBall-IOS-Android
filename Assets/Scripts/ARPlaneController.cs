
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ARPlaneController : MonoBehaviour
{
    
    // Start is called before the first frame update
    ARPlaneManager  m_ARPlaneManager;
    void Awake()
    {
         m_ARPlaneManager= GetComponent<ARPlaneManager>();
    }
    void OnEnable()
    {
        PlaceObjectOnPlane.HoopPlaced+= DisablePlaneDetection;

    }
    void OnDisable()
    {
        PlaceObjectOnPlane.HoopPlaced -= EnablePlaneDetection;
    }

    void EnablePlaneDetection(){
    SetAllPlanesActive(true);
        m_ARPlaneManager.enabled=!m_ARPlaneManager.enabled;
    }
    void DisablePlaneDetection(){
        SetAllPlanesActive(false);
        m_ARPlaneManager.enabled=!m_ARPlaneManager.enabled;
    }
    void SetAllPlanesActive(bool value){
    foreach(var plane in m_ARPlaneManager.trackables){
        plane.gameObject.SetActive(value);
    }
    }
}

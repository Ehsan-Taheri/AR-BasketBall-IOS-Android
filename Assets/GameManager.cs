using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Ball;
    void Awake()
    {
         
        PlaceObjectOnPlane.HoopPlaced += EnableBall;
    }

    private void EnableBall()
    {
        Ball.SetActive(true);
    }

    void OnDisable()
    {
        PlaceObjectOnPlane.HoopPlaced -= DisableBall;
    }

    private void DisableBall()
    {
        Ball.SetActive(false);
    }
     
}

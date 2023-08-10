using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : MonoBehaviour
{
    public GameObject gameControllerObject;
    GameController gameController;
    void Awake()
    {
        gameController = gameControllerObject.GetComponent<GameController>();
    }


    void OnTriggerEnter2D()
    {
        gameController.laps++;
    }

}

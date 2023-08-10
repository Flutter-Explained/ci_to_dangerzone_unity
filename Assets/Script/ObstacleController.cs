using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject game;
    private GameController gameController;

    void Awake(){
        gameController = game.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            gameController.GameOver("Crashed!");
        }
    }
}

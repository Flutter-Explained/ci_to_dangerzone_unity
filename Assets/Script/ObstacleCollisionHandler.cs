using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollisionHandler : MonoBehaviour
{
    public GameObject game;
    private GameController gameController;

    void Awake(){
        gameController = game.GetComponent<GameController>();
    }
    
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Obstacle")){
            gameController.GameOver("Crashed!");
        }
    }
}

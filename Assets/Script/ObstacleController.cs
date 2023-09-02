using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject game;
    private GameController gameController;

    void Awake(){
        gameController = game.GetComponent<GameController>();
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            gameController.GameOver("Crashed!");
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerSP : MonoBehaviour
{
    //how fast the ball moves
    public float ballMoveSpeed;

    public GameObject Player;

    bool ballLaunched;

    Vector3 playerPosition;

    [SerializeField] UIManagerSP uiManager;

    [Header("Audio")]
    public AudioClip hitSFX;
    public AudioSource audioSource;

    //finds out where the ball hit
    float hitFactor(Vector3 ballPosition, Vector3 playerPosition, float playerWidth){
        //returns where the ball hit
        return (ballPosition.x - playerPosition.x) / playerWidth;
    }


    private void Update()
    {
        //checks to see if the ball has been launched
        if (!ballLaunched)
        {
            //if it hasn't then make it the same pos as the player
            this.transform.position = getPlayerPos();
        }
    }


    Vector3 getPlayerPos() {
        //set the ball to the same position as the player
        playerPosition = Player.transform.position;
        playerPosition.y += 1;
        return playerPosition;
    }

    //called when the ball is launched
    public void launchBall()
    {
        //checks to see if the ball has been launched
        if (!ballLaunched)
        {
            //calculate the direction of the ball
            Vector3 direction = Quaternion.AngleAxis(Random.Range(-45.0f, 45.0f), Vector3.forward) * Vector3.up;
            //sets the velocity for the ball
            GetComponent<Rigidbody>().velocity = direction * ballMoveSpeed * Time.deltaTime;
            //makes it so the player cannot spam the launch button
            ballLaunched = true;
        }
    }

    //checks to see what the ball collides with
    private void OnCollisionEnter(Collision collision)
    {
        //play a sound effect when the ball hits something
        audioSource.PlayOneShot(hitSFX);

        //if player
        if (collision.gameObject.tag == "Player") {
            //find where the ball hit
            float x = hitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.x);

            //calculate the direction of the ball
            Vector3 direction = new Vector3(x, 1, 0).normalized;

            //set the velocity of the ball
            GetComponent<Rigidbody>().velocity = direction * ballMoveSpeed * Time.deltaTime;
        }

        //if brick
        if (collision.gameObject.tag == "Brick") {
            //destroy the brick
            Destroy(collision.gameObject);
            //increase the score
            uiManager.increaseScore(100);
        }

        //if killzone
        if (collision.gameObject.tag == "KillZone") {
            //make the position of the ball the same as the player
            this.transform.position = getPlayerPos();
            //make it so the ball can be launched again
            ballLaunched = false;
        }
    }
}

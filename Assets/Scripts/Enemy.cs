using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float enemySpeed = 1f;

    Rigidbody2D myRigidBody;
    SpriteRenderer spriteRender;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        FlipFish();

    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        myRigidBody.velocity = new Vector2(enemySpeed, 0f);
    }

    public void FlipFish()
    {

        if(spriteRender.flipX == true)
        {
            spriteRender.flipX = false;
            enemySpeed = -1;
            Debug.Log("flipX is false");
        }
        else
        {
            spriteRender.flipX = true;
            enemySpeed = 1;
            Debug.Log("flipX is true");
        }
    }

    private void OnCollisionEnter2D(Collision2D otherCollider)
    {
        print("fishy hit something");
    }
}
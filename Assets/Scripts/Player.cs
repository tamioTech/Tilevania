using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float playerSpeed = 5;
    [SerializeField] float jumpSpeed = 10;
    [SerializeField] float climbSpeed = 10;
    [SerializeField] float maxPlayerSpeed = 10f;

    int sceneIndex = 1;

    Vector2 deathKick = new Vector2(10f, 10f);
    Vector2 notRolling = new Vector2(0f, 0f);

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    Rigidbody2D gravityScale;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScale = GetComponent<Rigidbody2D>();

        print(SceneManager.GetActiveScene());
    }

    void Update()
    {
        if (!isAlive)
        {
            if (myRigidBody.velocity == notRolling )
            {
                print("shouldn't be rolling");
                myAnimator.ResetTrigger("Dying");
                myAnimator.SetTrigger("Dead");
            }
            print(myRigidBody.velocity);
            return;
        }
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
        Dying();
        Exit();
    }


    private void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal") ; //value between -1 to 1
        Vector2 playerVelocity = new Vector2(controlThrow * Time.deltaTime * playerSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);

        
    }


    private void ClimbLadder()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("Climbing", false);
            gravityScale.gravityScale = 2f;
            return; }

        gravityScale.gravityScale = 0f;
        float controlThrow = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed * Time.deltaTime);
        myRigidBody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);



    }

    private void Jump()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if(!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if(Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    public void Dying()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))) 
        {

            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void LoadNextLevel()
    {
        print(sceneIndex);
        sceneIndex += 1;
        print(sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
    
    public void Exit()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Interactables"))) { return; }
        

        LoadNextLevel();

    }

}

﻿using UnityEngine;
using UnityEngine.UI;
//JSON
using System.Collections;


public class Player : MonoBehaviour {
    //variables for swipe
    private Touch initialTouchSwipe = new Touch();
    private float distanceSwipe = 0;
    private bool hasSwiped = false;
    private bool isAlive = true;
    //rigidbody for movement with force
    public Rigidbody rb;
    //score + label
    private float score;
    public Text scoreText;
    //Speed + level up
    private float speed = 0.35f;
    private float levelUpTimer = 0; 
    //lane in which player is running, only 3 lanes
    private byte currentLane;
    //height from which player can jump again
    private float playerOnGroundJump = 0.1f;
    //force to move player
    private float forceSide = 7000;
    private float forceUp = 7800;
    private float forceJump = 14000;
    public Texture2D textureHealthRed;
    public Texture2D textureHealthOrange;
    public GameObject healthBar;
    private float health = 10;
    private float maxHealth = 10;

    public Button btnRestart;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        btnRestart.gameObject.SetActive(false);
        score = 0;
        currentLane = 1; //0 links, 1 midden,2 rechts

}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            
            if(health < maxHealth) { 
            
            isAlive = false;
            btnRestart.gameObject.SetActive(true);
            }
            else
            {
                health = 0.1f;
                healthbarChange(health);
            }

        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("swipeWorks");
        }
    }
    void FixedUpdate() //always being called
    {
        
       
        if (isAlive)
        {
            if(health < maxHealth)
            {
                health += 0.01f;
                healthbarChange(health);
            }
            score = Mathf.Round(Time.timeSinceLevelLoad * 100);
            scoreText.text = score.ToString();
            if ((score - levelUpTimer) >= 5000) //if the difference between the current score (= time) and the leveluptimer is bigger then 1000, he levels up 
            {
                levelUpTimer = score;
                speed += 0.05f;
            }



            this.transform.position = this.transform.position + new Vector3(0, 0, speed);
            foreach (Touch t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    initialTouchSwipe = t;
                }
                else if (t.phase == TouchPhase.Moved && !hasSwiped)
                {
                    //distance formula
                    float deltaXSwipe = initialTouchSwipe.position.x - t.position.x;
                    float deltaYSwipe = initialTouchSwipe.position.y - t.position.y;
                    distanceSwipe = Mathf.Sqrt((deltaXSwipe * deltaXSwipe) + (deltaYSwipe * deltaYSwipe));
                    //direction
                    bool swipedSideways = Mathf.Abs(deltaXSwipe) > Mathf.Abs(deltaYSwipe); //swipe up and down or sideways
                    if (distanceSwipe > 100)//100
                    {
                        if (swipedSideways && deltaXSwipe > 0) //swiped left
                        {
                            if(currentLane != 0  && this.transform.position.y <= playerOnGroundJump) { 
                            //this.transform.position = this.transform.position + new Vector3(-15f, 0, 0);
                            Physics.gravity = new Vector3(0, -30F, 0);
                            rb.AddForce(-forceSide, forceUp, 0, ForceMode.Force);
                            currentLane--;
                            }
                        }
                        else if (swipedSideways && deltaXSwipe <= 0) //swiped right
                        {
                            if(currentLane != 2 && this.transform.position.y <= playerOnGroundJump) { 
                            //   this.transform.Rotate(new Vector3(0, 15f, 0));
                            // this.transform.position = this.transform.position + new Vector3(15f, 0, 0);
                            Physics.gravity = new Vector3(0, -30F, 0);
                            rb.AddForce(forceSide, forceUp, 0, ForceMode.Force);
                                currentLane++;
                            }
                        }
                        else if (!swipedSideways && deltaYSwipe > 0) //swiped down
                        {
                           

                        }
                        else if (!swipedSideways && deltaYSwipe <= 0 && this.transform.position.y <= playerOnGroundJump) //swiped up
                        {
                            Physics.gravity = new Vector3(0, -50F, 0);
                            rb.AddForce(0, forceJump, 0, ForceMode.Force); 
                            
                        }
                        hasSwiped = true;
                    }

                }
                else if (t.phase == TouchPhase.Ended)
                {
                    initialTouchSwipe = new Touch(); //reset touch
                    hasSwiped = false;
                }


            }




        }
    }

    void OnGUI()
    {
        //drawing the health bar
        //draw the background (red, only visible if empty)
      //  GUI.BeginGroup(new Rect(100, 10, 1000, 1000));
        //GUI.Box(new Rect(0, 0, 1000, 1000), textureHealthRed);


        //GUI.EndGroup();
    }
    void healthbarChange(float currentHealth)
    {
        currentHealth = currentHealth / 10;
        healthBar.transform.localScale = new Vector3(currentHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
    } 

    
        
   
   
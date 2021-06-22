/*
Copyright 2020 Micah Schuster

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation and/or
other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the Player character.
/// Contains movement, collision, and anaimation.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Player Movement speed.
    /// </summary>
    public float speed;

    /// <summary>
    /// Input value read in using control stick or keyboard.
    /// </summary>
    private float input;

    /// <summary>
    /// Current player health.
    /// </summary>
    public int health;

    /// <summary>
    /// Epsilon, used for control stick input.
    /// </summary>
    private float eps = 0.0001f;

    /// <summary>
    /// Object that contains the death particle effects.
    /// </summary>
    public GameObject deathFX;

    /// <summary>
    /// Rigidbody for player collisions
    /// </summary>
    Rigidbody2D rb;

    /// <summary>
    /// Animator for player player animations.
    /// </summary>
    Animator anim;

    /// <summary>
    /// Gets instances of the Rigidbody and Animator for the player.
    /// Also updates the UI with the current health.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameManager.instance().updateHealthText(health);
        GameManager.instance().deathPanelSwitch(false);

    }

    /// <summary>
    /// Sets animation states depending on input and facing direction.
    /// </summary>
    private void Update()
    {
        if (input > 0 + eps || input < 0 - eps)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (input > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (input < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    /// <summary>
    /// Physics update. Gets the raw input and updates rigidbody velocity.
    /// </summary>
    void FixedUpdate()
    {
        //geting player input
        //input = Input.GetAxis("Horizontal"); //Has smoothing
        input = Input.GetAxisRaw("Horizontal"); //Has no smoothing

        //move player
        rb.velocity = new Vector2(input * speed, rb.velocity.y);

        if (Input.GetKey(KeyCode.Space)) {
            rb.velocity *= 5;
        }
    }

    /// <summary>
    /// Applies damage to the player. When the player dies, the death particle
    /// effect is instantiated, the gameObject is deactivated and the game over
    /// panel is displayed.
    /// </summary>
    /// <param name="value">Amount of damage for player to take.</param>
    public void takeDamage(int value)
    {
        health -= value;
        GameManager.instance().updateHealthText(health);

        if (health <= 0)
        {
            //player dies
            Instantiate(deathFX, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
            this.gameObject.SetActive(false);
            //GameObject.Destroy(gameObject); //can't have this for an easy reset
            GameManager.instance().deathPanelSwitch(true);

        }
    }

    /// <summary>
    /// Resets the game to the initial conditions.
    /// </summary>
    public void reset()
    {
        health = 3;
        Vector3 pos = new Vector3(0.14f, -1.41f, 0.0781f);
        this.transform.position = pos;
        GameManager.instance().updateHealthText(health);
        this.gameObject.SetActive(true);
    }
}

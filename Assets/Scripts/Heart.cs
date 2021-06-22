using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Class.
/// Contains movement and collision detection for the spawned enemy gameobjects.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Heart : MonoBehaviour
{

    //reference to the player script
    //Do we really need the enemy to know about the Player?
    //Depending on the situation, maybe or maybe not.
    /// <summary>
    /// Reference to the gameobject that contains Player
    /// </summary>
    Player p;

    /// <summary>
    /// Amount of damage that the enemy does to the player
    /// </summary>
    public int health;

    /// <summary>
    /// Minimum possible speed for Enemy movement.
    /// </summary>
    public float minSpeed;

    /// <summary>
    /// Maximum possible speed for Enemy movement.
    /// </summary>
    public float maxSpeed;

    /// <summary>
    /// Chosen speed for this object.
    /// </summary>
    private float speed;

    /// <summary>
    /// GameObject (Prefab) that contains the death particle effect.
    /// </summary>
    public GameObject deathEffect;

    /// <summary>
    /// GameObject (Prefab) that contains the death particle effect.
    /// </summary>
    public GameObject hitEffect;

    /// <summary>
    /// Sets a random speed and gets reference to the player gameobject.
    /// </summary>
    void Start()
    {
        //pick random value between min and max speed
        speed = Random.Range(minSpeed, maxSpeed);

        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    /// <summary>
    /// Updates the gameobject position over time.
    /// </summary>
    void Update()
    {
        //translate this object down at speed
        this.transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    /// <summary>
    /// Unity collision check. When the rigidbody eneters a trigger it checks for
    /// if the collider is the player or the ground. Applies appropriate damage
    /// and/or instantiates particle effects.
    /// </summary>
    /// <param name="collision">Collider that was triggered.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //reduce player health
            p.takeHealth(health);
            Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, -0.3f), Quaternion.identity);
            GameObject.Destroy(gameObject);
        }

        //destroy enemy if hits the ground
        if (collision.tag == "Ground")
        {
            Instantiate(deathEffect, new Vector3(transform.position.x, transform.position.y - 0.5f, -0.3f), Quaternion.identity);
            GameObject.Destroy(gameObject);
        }
    }
}

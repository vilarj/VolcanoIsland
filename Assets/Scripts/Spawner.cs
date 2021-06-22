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

/// <summary>
/// Keeps track of all spawned enemies and the spawning rate.
/// <remark>
/// The Update method performs the checks for game state and instantiates the
/// enemy gameobjects.
/// </remark>
/// </summary>
public class Spawner : MonoBehaviour
{

    /// <summary>
    /// Array that contains all of the currently active enemy objects.
    /// </summary>
    public GameObject[] enemies;
    public GameObject[] hearts;

    /// <summary>
    /// Time to wait between spawning enemy objects.
    /// </summary>
    public float timeBetweenSpawns;

    /// <summary>
    /// Minium allowable time between enemy spawn.
    /// </summary>
    public float minSpawnTime;

    /// <summary>
    /// Amount of time to decrease the time between spawning over time.
    /// </summary>
    public float decreaseAmt;

    /// <summary>
    /// Triggers spawn when the value is <=0
    /// </summary>
    private float spawnTimer;

    /// <summary>
    /// sets the spawning timer to 0 so that an enemy will spawn immediately.
    /// </summary>
    void Start()
    {
        spawnTimer = 0f;
    }

    /// <summary>
    /// Spawns the enemy gameobjects. Uses the spawnTimer to determin when to
    /// spawn. If the player is dead, no spawns take place. As the game progresses,
    /// the time between spawns will decrease until a minimum value is reached.
    /// </summary>
    void Update()
    {
        //stop spawning if there is no character
        if (GameObject.FindGameObjectWithTag("Player") == null || GameObject.FindGameObjectWithTag("Player").activeSelf==false) {
            return;
        }
        if (spawnTimer <= 0) {
            //spawn a enemy
            float min = -5f;
            float max = 5f;
            GameObject enemy = enemies[Random.Range(0, enemies.Length)];
            float value = Random.Range(min, max);
            Vector3 position = new Vector3(value, 6.5f, 0f);
            Instantiate(enemy,new Vector3(position.x,position.y,0f),Quaternion.identity);

            //increasing difficulty after every spawn
            timeBetweenSpawns -=decreaseAmt;
            if (timeBetweenSpawns < minSpawnTime) {
                timeBetweenSpawns = minSpawnTime;
            }

            spawnTimer = timeBetweenSpawns;
        } else {
            spawnTimer -= Time.deltaTime;
        }

    }

    /// <summary>
    /// Resets the timeBetweenSpawns back to the initial value.
    /// </summary>
    public void reset(){
        timeBetweenSpawns=1.25f;
    }
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script controls the health of enemy bots
public class EnemyHealth : MonoBehaviour {
    public int startingHealth = 30;
    public GameObject explosion;
    public GameObject bigExplosion;
    public AudioClip hitSoundClip;
    public float pointValue = 20;
    private int totalHealth;
    private GameObject lastBulletCollision;
    public AnimationManager animationManager;
    private bool dead;
    private Queue<GameObject> explosions = new Queue<GameObject>();
    private readonly int smallExplosionLimit = 12;
    private Queue<GameObject> bigExplosions = new Queue<GameObject>();
    private readonly int bigExplosionLimit = 6;
    public FeedbackController feed;

    void Start() {
        totalHealth = startingHealth;

        for (int i = 0; i < smallExplosionLimit; ++i) {
            GameObject splode = Instantiate(explosion, gameObject.transform.position + transform.up, gameObject.transform.rotation);
            splode.transform.parent = transform;
            splode.SetActive(false);
            explosions.Enqueue(splode);
        }

        for (int i = 0; i < bigExplosionLimit; ++i) {
            GameObject splode = Instantiate(bigExplosion, gameObject.transform.position + transform.up, gameObject.transform.rotation);
            splode.transform.parent = transform;
            splode.SetActive(false);
            bigExplosions.Enqueue(splode);
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Bullet" || col.gameObject.tag == "Missile") {
            if (col.gameObject == lastBulletCollision){
                return;
            }
            lastBulletCollision = col.gameObject;

            // Reduce health
            BulletBehavior bullet = col.gameObject.GetComponent(typeof(BulletBehavior)) as BulletBehavior;
            int bulletDamage = bullet.damage;
            totalHealth -= bulletDamage;

            if (totalHealth <= 0 && !dead) {
                PlayerFinance.cashOnHand += pointValue;
                feed.profit(pointValue);
                float deathDuration = animationManager.playDeathAnimation();
                GetComponent<EnemyMovement>().enabled = false;
                GetComponent<EnemyShoot>().enabled = false;
                animationManager.animator.PlayQueued("shrink");
                Destroy(gameObject, deathDuration + 0.5f);
                dead = true;
            }

            // Debug.Log("Damage: " + bullet.damage + " points. Bot has " + totalHealth + " health points remaining");
            // Play sound effect and explosion particle system
            GameObject explode = null;
            if (col.gameObject.tag == "Bullet") {
                explode = explosions.Dequeue();
                explode.SetActive(true);
                AudioSource hitSound = explode.GetComponent<AudioSource>();
                hitSound.PlayOneShot(hitSoundClip, 0.4f * Settings.volume);
                explosions.Enqueue(explode);
            } else if (col.gameObject.tag == "Missile") {
                explode = bigExplosions.Dequeue();
                explode.SetActive(true);
                AudioSource hitSound = explode.GetComponent<AudioSource>();
                hitSound.PlayOneShot(hitSoundClip, 0.65f * Settings.volume);
                bigExplosions.Enqueue(explode);
            }
            explode.transform.position = transform.position + transform.up;
            explode.transform.rotation = transform.rotation;
            explode.transform.parent = gameObject.transform;
            StartCoroutine(explode.GetComponent<Explosion>().timeout());

            // If bot goes under 50% health, make it look damaged
            /*
            if (totalHealth <= startingHealth / 2 && (totalHealth + bulletDamage) > startingHealth / 2) {
                if (gameObject.tag == "Blaster") {
                    for (int i = 0; i < 2; i++) {
                        Vector3 randomDirection = new Vector3(Random.value, Random.value, Random.value).normalized;
                        gameObject.transform.GetChild(0).gameObject.
                        transform.GetChild(0).gameObject.
                        transform.GetChild(2).gameObject.transform.GetChild(i)
                        .gameObject.transform.GetChild(0).gameObject.transform.rotation = Quaternion.LookRotation(randomDirection);
                    }
                }
                if (gameObject.tag == "Lobber") {
                    for (int i = 0; i < 2; i++) {
                        gameObject.transform.GetChild(0).gameObject.
                        transform.GetChild(0).gameObject.
                        transform.GetChild(4).gameObject.transform.GetChild(2 * i)
                        .gameObject.transform.position += new Vector3(0, 0.1f, 0);
                    }
                }
            }
            */
        }
    }
}

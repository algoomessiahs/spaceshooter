using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy-Stats")]
    [SerializeField] int health = 700;
    [SerializeField] int scoreValue = 50;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.1f;
    [SerializeField] float maxTimeBetweenShots = 5f;
    [SerializeField] GameObject EnemyFire;
    [SerializeField] float projectileFiringSpeed = 10f;

    [Header("Particle Effect")]
    [SerializeField] GameObject explodeEffect;
    [SerializeField] float delayParticleDestroy = 1f;

    [Header("Sound")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 1f;


    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter = shotCounter - Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);
        }
    }

    public void Fire()
    {
        GameObject fireShoot = Instantiate(EnemyFire, transform.position, Quaternion.identity) as GameObject;
        fireShoot.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileFiringSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        HitDestroy(damageDealer);
    }

    private void HitDestroy(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameScore>().AddToScore(scoreValue);
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        GameObject particleEffect = Instantiate(explodeEffect, transform.position, transform.rotation);
        Destroy(particleEffect, delayParticleDestroy);
    }
}

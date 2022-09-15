using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] float deathSFXVolume = 1f;
    [SerializeField] GameObject explosion;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] int scoreValue = 150;

    [Header("Projectile")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBeforeShot = 0.2f;
    [SerializeField] float maxTimeBeforeShot = 3f;
    [SerializeField] GameObject enemyLaser;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] float laserSFXVolume = 1f;
    


    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBeforeShot, maxTimeBeforeShot);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShot();
    }

    private void CountDownAndShot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <=0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBeforeShot, maxTimeBeforeShot);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
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
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
        GameObject explosionVFX = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosionVFX, durationOfExplosion);
    }

    private void Fire()
    {
        GameObject laser = Instantiate(enemyLaser, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
    }
}

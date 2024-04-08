using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


public class Player : MonoBehaviour
{

    [Header("Player")]
    // Config params
    [SerializeField] float speedOfPlayerx = 17f;
    [SerializeField] float speedOfPlayery = 14f;
    [SerializeField] float paddingx = 1f;
    [SerializeField] float paddingy = 1f;
    //[SerializeField] int currentHealth = 700;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserYSpeed = 17f;
    [SerializeField] float projectileFiringSpeed = 0.07f;

    [Header("Sound")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;

    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 1f;

    [Header("Separate")]
    public int maxHealth = 100;
    public int currentHealth;
    public Joystick joystick;



    public Health healthBar;

    Coroutine firingCoroutine;

    float minX;
    float maxX;

    float minY;
    float maxY;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        SetUpMoveBoundaries();
    }


    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingx;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddingx;

        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + paddingy;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingy;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        HitDestroy(damageDealer);
    }

    private void HitDestroy(DamageDealer damageDealer)
    {
        currentHealth -= damageDealer.GetDamage();
        healthBar.SetHealth(currentHealth);
        healthBar.SetHealth(currentHealth);
        damageDealer.Hit();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("Game Over");
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    public void Fire()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
        firingCoroutine = StartCoroutine(FireContinuously());
        }

        if (CrossPlatformInputManager.GetButtonUp("Fire1"))
        {
        StopCoroutine(firingCoroutine);
        }

    }

    IEnumerator FireContinuously()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            while (true)
            {
                GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserYSpeed);
               yield return new WaitForSeconds(projectileFiringSpeed);
            }
    }

    void Move()
    {
        var deltaX = joystick.Horizontal * Time.deltaTime * speedOfPlayerx; // Input.GetAxis("Horizontal") * Time.deltaTime * speedOfPlayerx;
        var newXPosition = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);

        var deltay = joystick.Vertical * Time.deltaTime * speedOfPlayery; // Input.GetAxis("Vertical") * Time.deltaTime * speedOfPlayery;
        var newYposition = Mathf.Clamp(transform.position.y + deltay, minY, maxY);

        transform.position = new Vector2(newXPosition, newYposition);
    }

}

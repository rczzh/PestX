using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerMovementSpeed;
    public float playerShotSpeed;
    public float playerFireRate;

    public float lastFire;

    new Rigidbody2D rigidbody;

    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerFireRate = GameController.FireRate;
        playerMovementSpeed = GameController.MoveSpeed;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float shootHorizontal = Input.GetAxisRaw("ShootHorizontal");
        float shootVertical = Input.GetAxisRaw("ShootVertical");

        // player movement
        rigidbody.velocity = new Vector3(horizontal * playerMovementSpeed, vertical * playerMovementSpeed, 0);

        // player shooting
        if ((shootHorizontal != 0 || shootVertical != 0) && Time.time > lastFire + playerFireRate)
        {
            Shoot(shootHorizontal, shootVertical);
            lastFire = Time.time;
        }
    }

    public void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * playerShotSpeed : Mathf.Ceil(x) * playerShotSpeed,
            (x < 0) ? Mathf.Floor(y) * playerShotSpeed : Mathf.Ceil(y) * playerShotSpeed,
            0
        ) ;
    }
}

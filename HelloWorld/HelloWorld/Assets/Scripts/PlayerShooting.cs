using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform laserSpawn;
    [SerializeField] 
    private float laserSpeed = 20f;
    [SerializeField] public float laserForce;
    [SerializeField]
    private GameObject laser;

    private PlayerAmmo ammo;
    private Rigidbody2D body;
    public PlayerMovement move;

    private void Start()
    {
        /*----------------------------------------------------------------------
        //Gets the Players Rigidbody Collider
        ------------------------------------------------------------------------*/
        body = GetComponent<Rigidbody2D>();

        ammo = GetComponent<PlayerAmmo>();
    }

    private void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && ammo.consumeAmmo(1))
        {
            ShootLaser();
            SFXManager.instance.PlaySound("PlayerShootLaser");
        }
    }

    private void ShootLaser()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 mousePosInWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //figuring out movement direction of laser
        Vector2 laserSpawnPos = new Vector2(laserSpawn.position.x, laserSpawn.position.y);
        Vector2 dir = mousePosInWorld - laserSpawnPos;
        dir.Normalize();

        Rigidbody2D spawnedLaser = Instantiate(laser, laserSpawn.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        spawnedLaser.velocity = laserSpeed * dir;

        RotateLaser(mousePos, spawnedLaser.transform);

        if(! move.inPlanetGrav)
        {
            body.AddForce(dir * -1 * laserForce, ForceMode2D.Impulse);
        }
    }

    private void RotateLaser(Vector3 mousePos, Transform laserTransform)
    {
        //figuring out rotation of laser
        Vector3 objectPos = mainCamera.WorldToScreenPoint(laserSpawn.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        laserTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}

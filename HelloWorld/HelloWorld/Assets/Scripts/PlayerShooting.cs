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
    [SerializeField]
    private GameObject laser;

    private PlayerAmmo ammo;

    private void Start()
    {
        ammo = GetComponent<PlayerAmmo>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && ammo.consumeAmmo(1))
        {
            ShootLaser();
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

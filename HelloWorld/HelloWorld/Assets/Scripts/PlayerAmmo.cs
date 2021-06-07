using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{
    [SerializeField] public int intialAmmo;

    [SerializeField] public int pickupAmount;

    [SerializeField] private int currAmmo;

    [SerializeField] public UIManager UI;
    // Start is called before the first frame update
    void Start()
    {
        currAmmo = intialAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        UI.ammoCount = currAmmo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ammo pickup
        if (collision.tag == "Ammo")
        {
            currAmmo += pickupAmount;
            SFXManager.instance.PlaySound("AmmoPickup");
            Destroy(collision.gameObject);
        }
    }

    public bool consumeAmmo(int ammo)
    {
        if (ammo <= currAmmo)
        {
            currAmmo -= ammo;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getAmmo()
    {
        return currAmmo;
    }
}

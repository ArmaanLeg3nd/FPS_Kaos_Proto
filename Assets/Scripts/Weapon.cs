
using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{

    public int weaponType = 0;
    public float damage = 1f;
    public float knockBack = 150f;
    public int bullets = 20;

    void OnTriggerStay(Collider collider)
    {
        if (collider.transform.tag == "player" && !transform.parent && Input.GetKeyDown(KeyCode.Q))
        { //If the player is in range and presses "Q" and has no parent (it can be picked)
            int wepNum = transform.name[3] - '0';
            Destroy(gameObject);
            collider.transform.GetComponent<WeaponInventory>().DropWeapon();
            collider.transform.GetComponent<WeaponInventory>().PickWeapon(wepNum, bullets);
        }
    }
}
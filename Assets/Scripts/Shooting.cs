using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{

    //Note: The bullets (Layer "Bullets") spawned collide only with layer "Level" (Layered Collision Management)

    public Texture2D crosshair;
    public GameObject bulletPrefab;
    public float resetTime;
    float weaponDamage; //The amount of damage the selected weapon is supposed to deal
    float knockBack; // The current amount of knock back the weapon provides
    float timer;
    int scopeState = 0;
    // Update is called once per frame

    void ShootAtPoint()
    {
        RaycastHit rayHit;
        int weaponNumber = GetComponent<WeaponInventory>().currentWeapon;
        GameObject currentGun = GameObject.Find("FirstPersonCharacter").transform.Find("GunCamera").transform.Find("gun" + weaponNumber.ToString()).gameObject;
        Ray currentRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(currentRay, out rayHit) && currentGun.GetComponent<Weapon>().bullets > 0)
        { //Store the result in the output variable rayHit
            currentGun.transform.GetComponent<Weapon>().bullets -= 1;
            gameObject.GetComponent<WeaponInventory>().OnShoot();
            GameObject current = (GameObject)Instantiate(bulletPrefab, transform.position + transform.forward * 0.6f + transform.up * 0.58f + (scopeState == 0 ? transform.right * 0.55f : transform.right * 0.1f), transform.rotation);
            current.GetComponent<Rigidbody>().velocity = transform.up + transform.right * 0.6f;
            if (rayHit.collider.tag == "enemy")
            {
                weaponDamage = GetComponent<WeaponInventory>().currentWeaponDamage;
                rayHit.transform.parent.GetComponent<EnemyAI>().enemyHealth -= weaponDamage; //The body and head are triggered which are children of the actual enemy
            }
            if (rayHit.collider.tag == "woodblock")
            {
                rayHit.collider.GetComponent<WoodBlock>().health -= weaponDamage;
                print(weaponDamage);
            }
            else if (rayHit.collider.GetComponent<Rigidbody>() != null)
            {
                knockBack = GetComponent<WeaponInventory>().currentKnockBack;
                rayHit.transform.GetComponent<Rigidbody>().AddForce(current.transform.forward * knockBack);
            }
        }
    }

    void Start()
    {
        Cursor.visible = false;
        timer = resetTime;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        { //Scope is adjusted for zoom in and zoom out
            scopeState = 1 - scopeState;
            GameObject currentParent = GameObject.Find("FirstPersonCharacter");
            currentParent = GameObject.Find("GunCamera");
            if (scopeState == 0)
            {
                Camera.main.fieldOfView = 60f;
                GetComponent<WeaponInventory>().newWeapon.transform.position = currentParent.transform.position + currentParent.transform.forward * 0.7f + currentParent.transform.right * 0.4f + currentParent.transform.up * -0.15f;
            }
            else
            {
                Camera.main.fieldOfView = 30f;
                GetComponent<WeaponInventory>().newWeapon.transform.position = currentParent.transform.position + currentParent.transform.forward * 0.7f + currentParent.transform.up * -0.15f;
            }
        }
        if (GetComponent<WeaponInventory>().typeOfWeapon == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootAtPoint();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    timer = resetTime;
                    ShootAtPoint();
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 30, 60, 60), crosshair, ScaleMode.ScaleToFit);
    }
}
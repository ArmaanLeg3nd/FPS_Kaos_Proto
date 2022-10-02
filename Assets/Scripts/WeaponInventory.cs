using UnityEngine;
using System.Collections;

public class WeaponInventory : MonoBehaviour
{

    public GameObject[] weapons; //Make sure the guns in these slots are named from gun0, gun1, gun2....gun9
    public string[] weaponNames; //Name each weapon
    public GUISkin bigFont;
    public GUISkin mediumFont;

    int primaryWeapon = 0; //Index of prefab which is our primary weapon
    int secondaryWeapon = 1; //Index of prefab which is our secondary weapon
    public int currentWeapon = 0; //Index of prefab, this is either equal to primaryWeapon or secondaryWeapon
    int activeWeapon = 0; // 0 for primary weapon and 1 for secondary weapon
    int primaryAmmo = 0;
    int secondaryAmmo = 0;

    public GameObject newWeapon; //The current selected weapon
    public float currentWeaponDamage = 0f;
    public int typeOfWeapon = 0; // 0 is manual, 1 is automatic
    public float currentKnockBack = 150f;

    void CreateCurrentWeapon(int wepNumber)
    {
        //GunCamera's depth is greater than firstpersoncharacter camera, so the guns are always visible
        //Each gun is a part of Guns layer mask
        //The GunCamera only shows objects with the Guns layer mask, whereas the firstpersoncharacter camera shows the rest of the world without the guns behind the GunCamera
        GameObject currentParent = GameObject.Find("FirstPersonCharacter").transform.Find("GunCamera").gameObject; //We put guns in the guncamera so that they are always visible (dont go through walls)
        Vector3 reqPosition = currentParent.transform.position + currentParent.transform.forward * 0.7f + currentParent.transform.right * 0.4f + currentParent.transform.up * -0.15f;
        newWeapon = (GameObject)Instantiate(weapons[wepNumber], reqPosition, currentParent.transform.rotation);
        typeOfWeapon = newWeapon.GetComponent<Weapon>().weaponType;
        currentWeaponDamage = newWeapon.GetComponent<Weapon>().damage;
        currentKnockBack = newWeapon.GetComponent<Weapon>().knockBack;
        newWeapon.name = "gun" + wepNumber.ToString();
        newWeapon.transform.parent = currentParent.transform;
        newWeapon.transform.GetComponent<Rigidbody>().detectCollisions = false; //Deactivate the rigidBody
        newWeapon.transform.GetComponent<Rigidbody>().isKinematic = true;
        Camera.main.fieldOfView = 60f;
    }

    void Start()
    {
        CreateCurrentWeapon(primaryWeapon);
        primaryAmmo = 30; //Pistol (starter weapon 1)
        secondaryAmmo = 8; //Shotgun (starter weapon 2)
    }

    public void OnShoot()
    { //Gun Movement Animations
        newWeapon.GetComponent<Animation>().Play();
        if (activeWeapon == 0)
            primaryAmmo--;
        else
            secondaryAmmo--;
        //We reduce the ammo for the weapon that is being used
    }

    public void DropWeapon()
    {
        GameObject prevWeapon = GameObject.Find("FirstPersonCharacter").transform.Find("GunCamera").transform.Find("gun" + currentWeapon.ToString()).gameObject;
        prevWeapon.transform.GetComponent<Rigidbody>().detectCollisions = true; //Activate the rigidBody
        prevWeapon.transform.GetComponent<Rigidbody>().isKinematic = false;
        prevWeapon.GetComponent<Rigidbody>().velocity = prevWeapon.transform.forward * 5f;
        prevWeapon.transform.parent = null;
        currentWeapon = -1;
        if (activeWeapon == 0)
            primaryWeapon = -1;
        else
            secondaryWeapon = -1;
    }

    public void PickWeapon(int weaponNumber, int numBullets)
    { //We change either primary ammo or secondary ammo accordingly
        if (currentWeapon != -1)
            return;
        currentWeapon = weaponNumber;
        if (activeWeapon == 0)
        {
            primaryWeapon = weaponNumber;
            primaryAmmo = numBullets; //Update current primary ammo
        }
        else
        {
            secondaryWeapon = weaponNumber;
            secondaryAmmo = numBullets; //Update current secondary ammo
        }
        CreateCurrentWeapon(weaponNumber); //Here we are instantiating a prefab, so its bullets need to be set to what was on the dropped weapon (numBullets)
        newWeapon.GetComponent<Weapon>().bullets = numBullets; //The number of bullets will be the same as they were when the weapon was dropped earlier
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && primaryWeapon != -1)
        {
            GameObject prevWeapon = GameObject.Find("FirstPersonCharacter").transform.Find("GunCamera").transform.Find("gun" + currentWeapon.ToString()).gameObject;
            Destroy(prevWeapon);
            CreateCurrentWeapon(primaryWeapon);
            newWeapon.GetComponent<Weapon>().bullets = primaryAmmo; //We have swapped to our primary weapon so we set the ammo to our primary ammo
            currentWeapon = primaryWeapon;
            activeWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && secondaryWeapon != -1)
        {
            GameObject prevWeapon = GameObject.Find("FirstPersonCharacter").transform.Find("GunCamera").transform.Find("gun" + currentWeapon.ToString()).gameObject;
            Destroy(prevWeapon);
            CreateCurrentWeapon(secondaryWeapon);
            newWeapon.GetComponent<Weapon>().bullets = secondaryAmmo; //We have swapped to our secondary weapon so we set the ammo to our secondary ammo
            currentWeapon = secondaryWeapon;
            activeWeapon = 1;
        }
    }

    void OnGUI()
    { //Try keep all the GUI related to the player over here
        GUI.skin = bigFont;
        GUI.Label(new Rect(50, 50, 300, 300), weaponNames[currentWeapon]); //Name of selected weapon
        GUI.skin = mediumFont;
        GUI.Label(new Rect(50, 100, 300, 300), weaponNames[currentWeapon == primaryWeapon ? secondaryWeapon : primaryWeapon]); //Name of weapon in hand
        GUI.Label(new Rect(Screen.width - 100, 30, 300, 300), "Ammo");
        GUI.skin = bigFont;
        GUI.Label(new Rect(Screen.width - 100, 80, 300, 300), newWeapon.GetComponent<Weapon>().bullets.ToString());
    }
}
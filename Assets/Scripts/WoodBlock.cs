using UnityEngine;
using System.Collections;

public class WoodBlock : MonoBehaviour
{

    public float health = 20f;

    public GameObject brokenWood;

    // Update is called once per frame
    void Update()
    {
        print(health);
        if (health <= 0)
        {
            health = 0;
            Vector3 position = transform.position;
            Destroy(gameObject);
            Instantiate(brokenWood, position, Quaternion.identity);
        }
    }
}
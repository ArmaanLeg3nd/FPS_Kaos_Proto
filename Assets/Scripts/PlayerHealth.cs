using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public float playerHealth = 50f;

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0f)
        {
            print("You Died");
        }
    }
}
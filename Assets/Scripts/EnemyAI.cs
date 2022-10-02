using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public GameObject player;
    public float enemyHealth = 30f;

    public GameObject flames;
    public GameObject explosion;

    public bool blowUp = false;
    private NavMeshAgent agent;
    private bool isDead = false;
    public float damage = 10f;
    float distance;

    // Use this for initialization
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>(); //This agent uses the nav mesh we created on the level object
    }

    // Update is called once per frame
    void Update()
    {

        if (!isDead)
        {
            if (enemyHealth <= 0f)
            {
                enemyHealth = 0f;
                isDead = true;
                agent.SetDestination(agent.transform.position);
                Instantiate(flames, agent.transform.position, Quaternion.identity);
            }
            else
            {
                distance = Vector3.Distance(player.transform.position, agent.transform.position);
                if (distance < 3)
                {
                    GameObject.Find("Player").GetComponent<PlayerHealth>().playerHealth -= damage * Time.deltaTime;
                    if (blowUp)
                    {
                        isDead = true;
                        Instantiate(explosion, agent.transform.position, Quaternion.identity);
                        GameObject.Destroy(gameObject);
                    }
                    if (!isDead)
                        agent.SetDestination(agent.transform.position);
                }
                else if (distance < 15)
                {
                    agent.SetDestination(player.transform.position);
                }
                else
                {
                    agent.SetDestination(agent.transform.position);
                }
            }
        }
    }
}
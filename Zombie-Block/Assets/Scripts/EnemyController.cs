using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float despawnRadius = 50f;
    public float damageDistance = 0.3f;
    public float attackInterval = 2f;
    public float wanderRadius = 5f;
    public float wanderInterval = 5f;
    bool isRunning;
    public float timeTillNextWander;
    Vector3 destination;
    Health playerHealth;
    Transform target;
    UnityEngine.AI.NavMeshAgent agent;
    float timeTillNextAttack;

    Animation attackAnim;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.stoppingDistance = 0f;
        attackAnim = GetComponent<Animation>();
        playerHealth = PlayerManager.instance.player.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<Health>().isDead)
        {
            agent.ResetPath();
            animator.SetBool("isRunning", false);
            return;
        }
        Random.InitState(System.DateTime.Now.Millisecond);
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance >= despawnRadius)
        {
            Destroy(gameObject);
        }

        if (distance <= lookRadius)
        {
            // go to player
            agent.stoppingDistance = 0.85f;
            agent.SetDestination(target.position);
            animator.SetBool("isRunning", true);
            isRunning = true;

        }
        if (distance <= agent.stoppingDistance && timeTillNextAttack <= 0)
        {   
            // look at player
            Vector3 direction = (target.position - transform.position);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime *5f);
            
            // attack player
            animator.SetBool("isRunning", false);
            animator.SetTrigger("Attack");
            isRunning = false;
            // attackAnim.Play();
            // timeTillNextAttack = attackInterval;
        }
        if (distance <= damageDistance && timeTillNextAttack <= 0)
        {
            // damage player if close enough
            playerHealth.TakeDamage(10f);
            agent.ResetPath();   
            timeTillNextAttack = attackInterval;
        }
        else
        {
            // if time for next wander
            if(!isRunning && timeTillNextWander <= 0)
            {
                // set random destination with distance of wanderRadius
                float t, x, z;
                t = Random.Range(0f, 1f) * 2 * Mathf.PI;
                x = transform.position.x + wanderRadius*Mathf.Cos(t);
                z = transform.position.z + wanderRadius*Mathf.Sin(t);
                destination = new Vector3(x, 0, z);
                // Debug.Log(destination);

                isRunning = true;
                agent.SetDestination(destination);
                animator.SetBool("isRunning", true);
            }
            else if(!isRunning)
            {
                // Debug.Log(timeTillNextWander);
                timeTillNextWander -= Time.deltaTime;
            }
            else if(Vector3.Distance(transform.position, destination) <= 0.5f)
            {
                // destination reached, reset timer.
                isRunning = false;
                animator.SetBool("isRunning", false);
                timeTillNextWander = wanderInterval;
            }
            // Debug.Log(Vector3.Distance(transform.position, destination));
        }
        timeTillNextAttack -= Time.deltaTime;
    }

    // void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, lookRadius);
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.XR.CoreUtils;

public class EnemyController : MonoBehaviour
{
    Transform playerTransform;
    Transform treeTransform;
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float attackRange = 4.0f;
    public int health = 10;
    float timer = 0.0f;
    NavMeshAgent agent;
    public Animator animator;
    public CapsuleCollider mainCollider;
    public GameObject enemyRig;
    EnemySpawner spawner;
    GameObject tree;
    TreeHealthController treeHealthController;

    public float ragdollModeDuration = 3.0f;
    private bool isRagdollModeActive = false;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        tree = GameObject.FindGameObjectWithTag("Tree");
        treeTransform = tree.transform;
        treeHealthController = tree.GetComponent<TreeHealthController>();
        GetRagdollBits();
        RagdollModeOff();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Sword") 
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        health = health - 10;
        if(health > 0) 
            animator.SetTrigger("GetHit"); 

        else if(!isRagdollModeActive)
           RagdollModeOn();
    }

    Collider[] ragDollColliders;
    Rigidbody[] limbsRigidbodies;
    void GetRagdollBits() 
    {
        ragDollColliders = enemyRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = enemyRig.GetComponentsInChildren<Rigidbody>();
    }

    public void SetSpawner(EnemySpawner _spawner) 
    {
        spawner = _spawner;
    }

    void RemoveEnemy()
    {
        if(spawner != null)
        {
            spawner.currentEnemy.Remove(this.gameObject);
        }
        Destroy(gameObject);
    }

    public void RagdollModeOn()
    {
        //Disable script, temporary
        //enabled = false;
        animator.enabled = false;
        
        foreach(Collider col in ragDollColliders)
        {
            col.enabled = true;
        }

        foreach(Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;
        }

        
        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        isRagdollModeActive = true;
        StartCoroutine(RemoveEnemyAfterDelay());
    }

    public void RagdollModeOff()
    {
        foreach(Collider col in ragDollColliders)
        {
            col.enabled = false;
        }

        foreach(Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = true;
        }

        animator.enabled = true;
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        isRagdollModeActive = false;
    }

    private IEnumerator RemoveEnemyAfterDelay()
    {
    yield return new WaitForSeconds(ragdollModeDuration);
    
        // Check if ragdoll mode is still active before removing the enemy
        if (isRagdollModeActive)
        {
            RemoveEnemy();
        }
    }
    
    void Update()
    {
    if (!isRagdollModeActive && tree != null)
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            float sqrDistance = (treeTransform.position - agent.destination).sqrMagnitude;
            if (sqrDistance > maxDistance * maxDistance)
            {
                agent.destination = treeTransform.position;
            }
            timer = maxTime;
        }
        agent.destination = treeTransform.position;
        animator.SetFloat("Speed", agent.velocity.magnitude);

        // Check if the enemy is within attack range
        float distanceToTarget = Vector3.Distance(transform.position, treeTransform.position);
        if (distanceToTarget <= attackRange)
        {
            if (!isAttacking)
            {
                // Stop the enemy
                agent.isStopped = true;
                animator.SetFloat("Speed", 0f);

                if(!treeHealthController.treeRemoved) 
                {
                    // Play the attack animation
                    animator.SetTrigger("Attack");

                    isAttacking = true;

                    // Wait for 2 seconds before allowing another attack
                    StartCoroutine(WaitForNextAttack());

                    // Perform the attack logic here
                    treeHealthController.TakeDamage(10);
                }
                
            }
        }
        else
        {
            // Resume moving towards the tree
            agent.isStopped = false;

            // Rigidbody enemyRigidbody = GetComponent<Rigidbody>();
            // if (enemyRigidbody != null)
            // {
            //     enemyRigidbody.isKinematic = false;
            // }
        }
    }
    else
    {
        agent.destination = transform.position;
        animator.SetFloat("Speed", 0f);
    }
}

private IEnumerator WaitForNextAttack()
{
    yield return new WaitForSeconds(2f);
    isAttacking = false;
}
}
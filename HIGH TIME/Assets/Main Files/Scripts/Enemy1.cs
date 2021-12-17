using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange =15f;
    [SerializeField] int maxHealth=15;
    [SerializeField] Animator anim;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int scene;
    int currentScene;
    public int currentHealth;
    public GameObject blood;
    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    float timer = 2f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        Instantiate(blood, new Vector3(transform.position.x,transform.position.y+1,transform.position.z), transform.rotation);
        Debug.Log(currentHealth);
        
    }

    void Die()
    {
        anim.SetBool("isDead", true);
        StartCoroutine(DestroyEnemy());
    }
    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        if(scene==3)
        {
            currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene + 1);
        }
    }


    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget<=chaseRange)
        {
            Move();
        }
        if(distanceToTarget<=navMeshAgent.stoppingDistance)
        {
            Attack();
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Move()
    {
        anim.SetBool("walk", true);
        anim.SetBool("attack", false);
        StartCoroutine(starNavMesh());
    }
    IEnumerator starNavMesh()
    {
        yield return new WaitForSeconds(2f);
        navMeshAgent.SetDestination(target.position);
    }

    void Attack()
    {
        StartCoroutine(GiveDamage());
    }
    IEnumerator GiveDamage()
    {
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(2f);
        Collider[] hitplayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        Debug.Log(hitplayer.Length);
        hitplayer[0].GetComponent<Maria>().takeDamage(5);
        anim.SetBool("walk", false);
    }    


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

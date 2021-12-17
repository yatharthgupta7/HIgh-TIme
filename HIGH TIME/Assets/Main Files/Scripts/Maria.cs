using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using TMPro;

public class Maria : MonoBehaviour
{

    [SerializeField] Animator anim;
    [SerializeField] float speed = 1.5f;
    [SerializeField] int maxHealth=100;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int reSpawn;
    [SerializeField] Vector3 startPosition;
    [SerializeField] int score = 0;
    [SerializeField] int maxScore;
    [SerializeField] int lastScene;
    [SerializeField] TextMeshProUGUI souls;
    int nextScene;
    int currentScene;
    float currentHealth;
    public float minX, maxX, minZ, maxZ;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attack();
        souls.text = "SOULS : " + score;
        loadNextScene();
    }

    private void loadNextScene()
    {
        if (score >= maxScore)
        {
            StartCoroutine(waitforSeconds());
            SceneManager.LoadScene(currentScene + 1);
        }
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(2f);
    }

    void Movement()
    {
        WMovement(); 
        bool leftclick = Input.GetMouseButton(0);
        Vector3 pos = transform.position;
        bool wPress = anim.GetBool("isWPressed");
        if (wPress && !leftclick&&pos.x>=minX)
        {
            transform.Translate(-2 * Time.deltaTime, 0, 0);
        }
        SMovement();
        bool sPress = anim.GetBool("isSPressed");
        if (sPress && !leftclick && pos.x <= maxX)
        {
            transform.Translate(2 * Time.deltaTime, 0, 0);
        }
        DMovement(); 
        bool dPress = anim.GetBool("isDPressed");
        if (dPress && !leftclick&&pos.z<=maxZ)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        AMovement(); 
        bool aPress = anim.GetBool("isAPressed");
        if (aPress && !leftclick&&pos.z>=minZ)
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        Jump();
    }

    void Jump()
    {
        bool space = Input.GetKeyDown(KeyCode.Space);
        if (space)
        {
            anim.SetBool("isSpacePressed", true);
        }
        if(!space)
        {
            anim.SetBool("isSpacePressed", false);
        }
    }
    void WMovement()
    {
        Vector3 pos = transform.position;
        bool moveW = Input.GetKey("w");
        bool wPress = anim.GetBool("isWPressed");
        bool leftShift = Input.GetKey(KeyCode.LeftShift);
        if (!wPress && moveW)
        {
            if (leftShift) anim.SetBool("isShiftPressed", true);
            anim.SetBool("isWPressed", true); 
        }
        if (wPress && !moveW)
        {
            anim.SetBool("isShiftPressed", false);
            anim.SetBool("isWPressed", false);
        }
    }

    void SMovement()
    {
        Vector3 pos = transform.position;
        bool moveS = Input.GetKey("s");
        bool SPress = anim.GetBool("isSPressed");
        if (!SPress && moveS)
        {
            anim.SetBool("isSPressed", true);
            pos.z -= 2 * Time.deltaTime;
        }
        if (SPress && !moveS)
        {
            anim.SetBool("isSPressed", false);
        }
    }
    void DMovement()
    {
        Vector3 pos = transform.position;
        bool moveD = Input.GetKey("d");
        bool DPress = anim.GetBool("isDPressed");
        if (!DPress && moveD)
        {
            anim.SetBool("isDPressed", true);
            pos.x += speed * Time.deltaTime;
        }
        if (DPress && !moveD)
        {
            anim.SetBool("isDPressed", false);
        }
    }
    void AMovement()
    {
        Vector3 pos = transform.position;
        bool moveA = Input.GetKey("a");
        bool APress = anim.GetBool("isAPressed");
        if (!APress && moveA)
        {
            anim.SetBool("isAPressed", true);
            pos.x -= speed * Time.deltaTime;
            transform.position = pos;
        }
        if (APress && !moveA)
        {
            anim.SetBool("isAPressed", false);
        }
    }

    void Attack()
    {
        bool isLeftClick = Input.GetMouseButtonDown(0);
        if(isLeftClick)
        {
            anim.SetBool("isLeftClickPressed", true);
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            foreach(Collider enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy1>().takeDamage(50);
                score += 10;
            }
        }
        if(!isLeftClick)
        {
            anim.SetBool("isLeftClickPressed", false);
        }
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth<=0)
        {
            Die();
        }
    }
    IEnumerator waitforSeconds()
    {
        yield return new WaitForSeconds(4f);
    }
    void Die()
    {
        anim.SetBool("Dead", true);
        if(reSpawn<3)
        {
            StartCoroutine(waitforSeconds());
            reSpawnPlayer();
        }
        else
        {
            SceneManager.LoadScene(lastScene);
        }
    }

    void reSpawnPlayer()
    {
        reSpawn++;
        transform.position = startPosition;
        anim.SetBool("idle", true);
        currentHealth = maxHealth;
        anim.SetBool("Dead", false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
        }
    }
}

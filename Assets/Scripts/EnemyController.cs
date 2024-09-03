using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState
{
    Idle,
    Wander,
    Tracking,
    Attack,
    Death
}

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currentState = EnemyState.Wander;

    public float enemySpeed;
    public float trackingRange;
    public float attackRange;
    public float attackInterval;

    private bool chooseDirection = false;
    private bool isEnemyDead = false;
    private bool attackCoolDown = false;

    private Vector3 randomDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case (EnemyState.Idle): Idle();
                break;

            case (EnemyState.Wander): Wander();
                break;

            case (EnemyState.Tracking): Follow();
                break;

            case (EnemyState.Attack): Attack();
                break;

            case (EnemyState.Death):
                break;
        }

        if (IsPlayerInRange(trackingRange) && currentState != EnemyState.Death)
        {
            currentState = EnemyState.Tracking;
        }
        else if (!IsPlayerInRange(trackingRange) && currentState != EnemyState.Death)
        {
            //currentState = EnemyState.Wander;
            currentState = EnemyState.Idle;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDirection = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        randomDirection = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDirection = false;
    }

    void Wander()
    {
        if (!chooseDirection)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += enemySpeed * Time.deltaTime * -transform.right;

        if (IsPlayerInRange(trackingRange))
        {
            currentState = EnemyState.Tracking;
        }
    }

    void Idle()
    {
        if (IsPlayerInRange(trackingRange))
        {
            currentState = EnemyState.Tracking;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
    }

    void Attack()
    {
        if (!attackCoolDown)
        {
            GameController.DamagePlayer(1);
            StartCoroutine(CoolDown());
        }
           
    }

    private IEnumerator CoolDown()
    {
        attackCoolDown = true;
        yield return new WaitForSeconds(attackInterval);
        attackCoolDown = false;
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}

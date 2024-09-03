using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,
    Tracking,
    Death
}

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currentState = EnemyState.Wander;

    public float enemySpeed;
    public float enemyRange;

    private bool chooseDirection = false;
    private bool isEnemyDead = false;
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
            case(EnemyState.Wander): Wander();
            break;

            case(EnemyState.Tracking): Follow();
            break;

            case(EnemyState.Death):
            break;
        }

        if (IsPlayerInRange(enemyRange) && currentState != EnemyState.Death)
        {
            currentState = EnemyState.Tracking;
        }
        else if (!IsPlayerInRange(enemyRange) && currentState != EnemyState.Death)
        {
            currentState = EnemyState.Wander;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDirection = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
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

        transform.position += enemySpeed * -transform.right * Time.deltaTime;
        if (IsPlayerInRange(enemyRange))
        {
            currentState = EnemyState.Tracking;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}

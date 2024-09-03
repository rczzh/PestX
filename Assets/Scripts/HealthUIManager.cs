using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    public GameObject heartPrefab;
    //public int health, maxHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private void OnEnable()
    {
        GameController.OnPlayerDamaged += DrawHeart;
    }

    private void OnDisable()
    {
        GameController.OnPlayerDamaged -= DrawHeart;
    }

    private void Start()
    {
        DrawHeart();
    }

    public void DrawHeart()
    {
        ClearHearts();

        // calculate how many hearts to create based on max health
        int maxHealthRemainder = GameController.MaxHealth % 2;
        int heartsToMake = (GameController.MaxHealth / 2 + maxHealthRemainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = Mathf.Clamp(GameController.Health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart> ();
    }
}

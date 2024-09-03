using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;

    private static int health = 6;
    private static int maxHealth = 6;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;

    public List<String> itemsCollected = new List<String>();

    public static int Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }

    public Text healthText;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;
        OnPlayerDamaged?.Invoke();

        if (health <= 0)
        {
            KillPlayer();
        }
    }

    public static void HealPlayer(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
        OnPlayerDamaged?.Invoke();
    }

    public static void MoveSpeedChange(float amount)
    {
        moveSpeed += amount;
    }

    public static void FireRateChange(float amount)
    {
        fireRate -= amount;
    }

    public static void BulletSizeChange(float amount)
    {
        BulletSize += amount;
    }

    public void UpdateItemsCollected(CollectionController item)
    {
        itemsCollected.Add(item.item.name);

        if (itemsCollected.Contains("Monkeypox") && itemsCollected.Contains("Vaccine") && itemsCollected.Contains("Steroids"))
        {
            FireRateChange(0.25f);
        }
    }

    public static void KillPlayer()
    {

    }
}

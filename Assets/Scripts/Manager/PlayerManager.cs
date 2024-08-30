using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, Subject
{
    public static PlayerManager Instance { get; private set; }

    private List<Observer> observers = new List<Observer>();

    private Transform player;

    [Header("Player Stats")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float currentHealth = 3f;
    [SerializeField] private float luck = 1f;
    [SerializeField] private float baseAttackDamage = 3.5f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float attackRange = 1f;

    [Header("Other Stats")]
    [SerializeField] private int gears = 0;

    public float MoveSpeed { get => moveSpeed; set {moveSpeed = value; Notify();} }
    public float MaxHealth { get => maxHealth; set {maxHealth = value; Notify();} }
    public float CurrentHealth { get => currentHealth; set {currentHealth = value; Notify();} }
    public float Luck { get => luck; set {luck = value; Notify();} }
    public float BaseAttackDamage { get => baseAttackDamage; set {baseAttackDamage = value; Notify();} }
    public float AttackSpeed { get => attackSpeed; set {attackSpeed = value; Notify();} }
    public float AttackRange { get => attackRange; set {attackRange = value; Notify();} }

    public int Gears { get => gears; set {gears = value; Notify();} }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("PlayerManager already exists, destroying new one");
            Destroy(gameObject);
        }
    }


    // TODO: Remove this Update method
    public void Update(){
        if(Input.GetKeyDown(KeyCode.F1)){
            MoveSpeed=MoveSpeed+1;
        }
        if(Input.GetKeyDown(KeyCode.F2)){
            MoveSpeed=MoveSpeed-1;
        }
        if(Input.GetKeyDown(KeyCode.F12)){
            SaveSystem.SaveGame(new GameData(this));
        }
    }

    public void LoadPlayerStats(GameData data)
    {
        MoveSpeed = data.moveSpeed;
        MaxHealth = data.maxHealth;
        CurrentHealth = data.currentHealth;
        Luck = data.luck;
        BaseAttackDamage = data.baseAttackDamage;
        AttackSpeed = data.attackSpeed;
        AttackRange = data.attackRange;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            player.GetComponent<PlayerController>().StartCoroutine("Die");
        }
        Debug.Log(CurrentHealth);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    public Transform GetPlayer()
    {
        return player;
    }

    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (Observer observer in observers)
        {
            observer.Notify();
        }
    }
}

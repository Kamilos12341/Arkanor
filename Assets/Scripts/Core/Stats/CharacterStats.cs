using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int attack = 10;
    [SerializeField] private int defense = 5;
    [SerializeField] private float moveSpeed = 5f;



    public CharacterStat MaxHealth { get; private set; }
    public CharacterStat Attack { get; private set; }
    public CharacterStat Defense { get; private set; }
    public CharacterStat MoveSpeed { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        MaxHealth = new CharacterStat(maxHealth);
        Attack = new CharacterStat(attack);
        Defense = new CharacterStat(defense);
        MoveSpeed = new CharacterStat(moveSpeed);
    }
}
using Unity.VisualScripting;
using UnityEngine;
using DamageNumbersPro;

public enum DamageType
{
    Blunt, Slash, Magic
}
public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int power;
    public int xp;

    public float checkForPlayerCooldown;
    public float checkForPlayerCooldownMax;

    public float chaseRadius;
    public float attackCooldown;
    public float attackCooldownMax;
    public bool onAttackRange;
    public DamageType damageType;

    public float hitCooldown;
    public float hitCooldownMax;

    public DamageNumber damageNumber;

    public abstract void Damage(int dam, bool isMagic);
}

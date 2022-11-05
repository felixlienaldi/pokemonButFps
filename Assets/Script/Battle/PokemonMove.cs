using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType {
    Attack,
    Buffer
}

public enum BuffType {
    AttackDebuff,
    DefenseDebuff,
    AttackBuff,
    DefenseBuff
}

[CreateAssetMenu(fileName = "Pokemon Move", menuName = "Pokemon/New Pokemon Move")]
public class PokemonMove : ScriptableObject
{
    public new string name;
    public int limit;
    public int value;
    public MoveType moveType;
    public BuffType buffType;
}
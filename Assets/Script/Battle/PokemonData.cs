using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/New Pokemon")]
public class PokemonData : ScriptableObject
{
    public int health;
    public int attack;
    public int defense;
    public PokemonMove[] moveSet;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public PokemonData pokemon;

    public virtual void DoMove(int index) {
        //TODO: Bikin script AI ngapain Movenya
        Battle.instance.DoMove(pokemon.moveSet[index]);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public Pokemon pokemon;

    public virtual void DoMove(int index) {
        Battle.instance.activeMove = pokemon.data.moveSet[index];
        Battle.instance.DoMove(pokemon.data.moveSet[index]);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrainer : Trainer
{
    public override void DoMove(int index) {
        index = Random.Range(0, pokemon.data.moveSet.Length - 1);
        base.DoMove(index);
    }
}

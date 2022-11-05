using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerDatabase : MonoBehaviour
{
    public static TrainerDatabase instance;

    public List<Trainer> trainers;

    private void Awake() {
        instance = this;
    }
}

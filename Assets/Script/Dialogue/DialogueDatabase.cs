using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueDatabase : MonoBehaviour
{
    public static DialogueDatabase instance;

    [Serializable]
    public class Dialogues {
        public List<Dialogue_Scriptable> dialogues;
    }

    public List<Dialogues> databaseDialogue;

    private void Awake() {
        instance = this;
    }

    public List<Dialogue_Scriptable> SelectDialogue(int p_Index) {
        Debug.Log("Kena panggil sebelum clear");
        Dialogue_Manager.instance.SetDialogue(databaseDialogue[p_Index].dialogues);
        return databaseDialogue[p_Index].dialogues;
    }

}

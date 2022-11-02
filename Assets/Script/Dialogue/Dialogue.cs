using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[Serializable]
public class Dialogue {
    public const string DESIRED_STRING = "{pokemon}";
    [Serializable]
    public class Choice {
        public string choiceName;
        public UnityEvent onPicked;
    }

    public enum DialogueType {
        TextOnly,
        Choice,
    }

    [TextArea(1,3)]
    public string[] sentences;
    public Choice[] choice;
    public bool choiceChoosen;
    public int choosenChoiceIndex;
    public DialogueType dialogueType;
    public UserDialogue userDialogue;
    
    public void Select(int choiceIndex) {
        choosenChoiceIndex = choiceIndex;
        choiceChoosen = true;
        choice[choiceIndex]?.onPicked?.Invoke();
        Dialogue_Manager.instance.ResetChoice();
    }

    public string FilterWords(string sentence) {
        if(sentence.Contains(DESIRED_STRING)) {
            sentence = sentence.Replace(DESIRED_STRING, choice[choosenChoiceIndex].choiceName);
        }

        return sentence;
    }

}

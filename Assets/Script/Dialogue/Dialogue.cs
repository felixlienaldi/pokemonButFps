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
    public const string DESIRED_POKEMON_NAME_STRING = "{pokemonName}";
    public const string DESIRED_POKEMON_BATTLE_MOVE_STRING = "{move}";

    [Serializable]
    public class Choice {
        public string choiceName;
        public UnityEvent<int> onPicked;
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
        choice[choiceIndex]?.onPicked?.Invoke(choiceIndex);
        Dialogue_Manager.instance.ResetChoice();
    }

    public string FilterWords(string sentence) {
        if(sentence.Contains(DESIRED_STRING)) {
            sentence = sentence.Replace(DESIRED_STRING, choice[choosenChoiceIndex].choiceName);
        }
        if (sentence.Contains(DESIRED_POKEMON_NAME_STRING)) {
            sentence = sentence.Replace(DESIRED_POKEMON_NAME_STRING, Battle.instance.ActiveTrainer().pokemon.name);
        }
        if (sentence.Contains(DESIRED_POKEMON_BATTLE_MOVE_STRING)) {
            sentence = sentence.Replace(DESIRED_POKEMON_BATTLE_MOVE_STRING, Battle.instance.activeMove?.name);
        }

        return sentence;
    }

    public Dialogue Sentences(string[] sentences) {
        this.sentences = sentences;
        return this;
    }

    public Dialogue Choices(Choice[] choice) {
        this.choice = choice;
        return this;
    }

    public Dialogue Type(DialogueType dialogueType) {
        this.dialogueType = dialogueType;
        return this;
    }

    public Dialogue UserDialogue(string name, Sprite image = null) {
        this.userDialogue = new UserDialogue() {
            name = name,
            userPotrait = image,
        };

        return this;
    }






}

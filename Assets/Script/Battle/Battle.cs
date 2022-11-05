using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Dialogue;
using static DialogueDatabase;

public enum Turn {
    Player,
    Enemy
}

public class Battle : MonoBehaviour
{
    public static Battle instance;

    public Trainer player;
    public Trainer enemy;
    public Turn turn;
    public PokemonMove activeMove;

    public UnityEvent onBattleEnd;
    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartBattle() {
        turn = Turn.Player;
        InitTrainersData();
        Dialogue dialogue = new Dialogue()
           .Sentences(new string[] { "What Will you Do?" })
           .Type(DialogueType.Choice)
           .Choices(GetChoices())
           .UserDialogue("");
        Dialogue_Manager.instance.isChoice = true; 
        StartCoroutine(ShowUI(dialogue));
    }
    
    public void InitTrainersData() {
        enemy = TrainerDatabase.instance.trainers[Random.Range(0, TrainerDatabase.instance.trainers.Count - 1)];
    }

    public Trainer TargetTrainer() {
        if (turn == Turn.Player) {
            return enemy;
        } else {
            return player;
        }
    }

    public Trainer ActiveTrainer() {
        if (turn == Turn.Player) {
            return player;
        } else {
            return enemy;
        }
    }

    public void DoMove(PokemonMove move) {
        Dialogue dialogue = new Dialogue();
        Dialogue_Manager.instance.dialogues.Clear();
        activeMove = move;
        if (move.moveType == MoveType.Attack) {
            int attack = ((ActiveTrainer().pokemon.attack * move.value) - TargetTrainer().pokemon.defense);
            TargetTrainer().pokemon.health -= attack;
            if(TargetTrainer().pokemon.health <= 0) {
                TargetTrainer().pokemon.health = 0;
            }
            dialogue = new Dialogue()
            .Sentences(new string[] { "{pokemonName} use {move}", "Deal " + attack + " damage" })
            .Type(DialogueType.TextOnly)
            .Choices(GetChoices())
            .UserDialogue("");
        } else if(move.moveType == MoveType.Buffer) {
            if(move.buffType == BuffType.AttackDebuff) {
                TargetTrainer().pokemon.attack -= move.value;
                dialogue = new Dialogue()
                .Sentences(new string[] { "{pokemonName} use {move}", "Enemy attack decreases" })
                .Type(DialogueType.TextOnly)
                .Choices(GetChoices())
                .UserDialogue("");

            } else if(move.buffType == BuffType.DefenseDebuff) {
                TargetTrainer().pokemon.defense -= move.value;
                dialogue = new Dialogue()
            .Sentences(new string[] { "{pokemonName} use {move}", "Enemy defense decreases" })
            .Type(DialogueType.TextOnly)
            .Choices(GetChoices())
            .UserDialogue("");
            } else if (move.buffType == BuffType.DefenseBuff) {
                ActiveTrainer().pokemon.defense += move.value;
                dialogue = new Dialogue()
            .Sentences(new string[] { "{pokemonName} use {move}", "Enemy defense increases" })
            .Type(DialogueType.TextOnly)
            .Choices(GetChoices())
            .UserDialogue("");
            } else if (move.buffType == BuffType.AttackBuff) {
                ActiveTrainer().pokemon.attack += move.value;
                dialogue = new Dialogue()
            .Sentences(new string[] { "{pokemonName} use {move}", "Enemy attack increases" })
            .Type(DialogueType.TextOnly)
            .Choices(GetChoices())
            .UserDialogue("");

            }
        }
        
        Dialogue_Manager.instance.TriggerOneTimeDialogue(dialogue);
    }

    public void NextTurn() {
        Dialogue_Manager.instance.isChoice = true;
        if (turn == Turn.Player) {
            turn = Turn.Enemy;
            Dialogue dialogue = new Dialogue()
          .Sentences(new string[] { "Enemies Turn !" })
          .Type(DialogueType.TextOnly)
          .Choices(GetChoices())
          .UserDialogue("");
            StartCoroutine(ShowEnemyUI(dialogue));
        }else {
            turn = Turn.Player;
            Dialogue dialogue = new Dialogue()
          .Sentences(new string[] { "What Will you Do?" })
          .Type(DialogueType.Choice)
          .Choices(GetChoices())
.UserDialogue("");
            
            StartCoroutine(ShowUI(dialogue));
        }
        
    }

    public void BattleEnd() {
        Debug.Log("Battle End");
        onBattleEnd?.Invoke();
    }

    public void Check() {

        Debug.Log("Sudah dipangil");
        if (player.pokemon.health <= 0 || enemy.pokemon.health <= 0) {
            BattleEnd();
        } else {
            NextTurn();
        }
    }
    public IEnumerator ShowUI(Dialogue dialogue) {
        yield return new WaitForSeconds(1f);
        Dialogue_Manager.instance.onDialogueEnd.AddListener(Check);
        Dialogue_Manager.instance.isEnemy = false;
        Dialogue_Manager.instance.TriggerOneTimeDialogue(dialogue);
    }

    public IEnumerator ShowEnemyUI(Dialogue dialogue) {
        yield return new WaitForSeconds(1f);
        Dialogue_Manager.instance.onDialogueEnd.AddListener(Check);
        Dialogue_Manager.instance.isEnemy = true;
        Dialogue_Manager.instance.TriggerOneTimeDialogue(dialogue);
    }

    public Dialogue.Choice[] GetChoices() {
        Dialogue.Choice[] dialogue = new Dialogue.Choice[ActiveTrainer().pokemon.data.moveSet.Length];
        for (int i = 0; i < dialogue.Length; i++) {
            dialogue[i] = new Dialogue.Choice();
            dialogue[i].choiceName = ActiveTrainer().pokemon.data.moveSet[i].name;
            dialogue[i].onPicked = new UnityEvent<int>();
            dialogue[i].onPicked.AddListener(ActiveTrainer().DoMove);

        }
         

        return dialogue;
    }
}


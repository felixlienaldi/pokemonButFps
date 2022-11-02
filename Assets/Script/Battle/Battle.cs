using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle() {
        turn = Turn.Player;
        //SetBattleUI();
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
        if(move.moveType == MoveType.Attack) {
            TargetTrainer().pokemon.health -= move.value;
        }else if(move.moveType == MoveType.Buffer) {

        }

        if (player.pokemon.health <= 0 || enemy.pokemon.health <= 0) {
            BattleEnd();
        } else {
            NextTurn();
        }
    }

    public void NextTurn() {
        if(turn == Turn.Player) {
            turn = Turn.Enemy;
        }else {
            turn = Turn.Player;
        }

        //TODO: Lanjutkan dengan turn selanjutnya Munculin Choice atau apa.
    }

    public void BattleEnd() {
        //TODO: Masukkan codingan saat battle selesai
    }

}


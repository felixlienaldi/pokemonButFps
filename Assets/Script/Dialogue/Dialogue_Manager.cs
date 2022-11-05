using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.Events;
using static DialogueDatabase;

public class Dialogue_Manager : MonoBehaviour{
    public const string CHOICE_NAME = "{pokemon}";

    public static Dialogue_Manager instance;

    [Header("Config")]
    [SerializeField] private float textSpeed;
    
    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueBoard;
    public List<Dialogue_Scriptable> dialogues = new List<Dialogue_Scriptable>();

    public bool isChoice;
    private Queue<string> sentences = new Queue<string>();

    [Header("Choice")]
    public GameObject choiceBoard;
    public Choice choicePrefab;
    public Transform choiceTransformParent;
    private Choice spawnedChoice;
    private List<Choice> choices = new List<Choice>();
    private int index;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogueNameText;
    public Image imagePotrait;

    [Header("Event")]
    public UnityEvent onDialogueEnd;
 
    //===== PRIVATES =====
    StringBuilder sentence = new StringBuilder();

    int visibleCount;
    int totalVisibleCharacter;
    int counter;
    int currentIndex;
    bool isDialogue;
    [HideInInspector] public bool isEnemy;

    private Coroutine tempCoroutine;
    public List<Action<int>> actions = new List<Action<int>>();
    private Dialogue_Scriptable activeDialogue => dialogues[currentIndex];
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        instance = this;
    }

    void Start() {
        //DialogueDatabase.instance.SelectDialogue(0);
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.S) && dialogues != null && !isChoice) {
            //if (!isDialogue) {
            //    TriggerDialogue();
            //} else {
            Debug.Log(isChoice);
            Next();
            //}
        }
    }


    public void SetDialogue(List<Dialogue_Scriptable> listDialogue) {
        Debug.Log("Ini Dipanggil");
        for (int i = 0; i < listDialogue.Count;i++) {
            Debug.Log(i);
            dialogues.Add(Instantiate(listDialogue[i]));
        }
    }


    public void TriggerDialogue() {
        isEnemy = false;
        NarrationOn(dialogues[currentIndex].dialogue);
        isDialogue = true;
    }

    public void TriggerOneTimeDialogue(Dialogue dialogue) {
        Dialogue_Scriptable newScriptable = ScriptableObject.CreateInstance<Dialogue_Scriptable>();
        newScriptable.dialogue = dialogue;
        dialogues.Add(newScriptable);
        NarrationOn(dialogue);
        isDialogue = true;
    }

    public void Next() {
        if(dialogueBoard.activeSelf && !isChoice) DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue) {
        sentences.Clear();

        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(FilterWords(sentence));
        }

        DisplayNextSentence();
    }

    public string FilterWords(string sentence) {
        return activeDialogue.dialogue.FilterWords(sentence);
    }

    public void DisplayNextSentence() {
        dialogueNameText.text = activeDialogue.dialogue.userDialogue.name;
        if(imagePotrait != null) imagePotrait.sprite = activeDialogue.dialogue.userDialogue.userPotrait;

        if (visibleCount < totalVisibleCharacter) {
            StopCoroutine(tempCoroutine);
            tempCoroutine = StartCoroutine(ShowAllWords());
            return;
        }

        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }        

        sentence.Clear();
        sentence.Append(sentences.Dequeue());

        if(tempCoroutine != null) StopCoroutine(tempCoroutine);
        tempCoroutine = StartCoroutine(DisplayWord(sentence.ToString()));
    }

    public void EndDialogue() {
        if (isChoice) return;
        NarrationOff();
        visibleCount = totalVisibleCharacter;
        dialogueText.maxVisibleCharacters = visibleCount;
        
    }

    public IEnumerator ShowAllWords(List<Action<int>> actions = null) {
        dialogueText.maxVisibleCharacters = totalVisibleCharacter;
        TriggerChoice();
        yield return new WaitForEndOfFrame();
        visibleCount = 0;
        totalVisibleCharacter = 0;
        counter = 0;
        
    }

    public IEnumerator DisplayWord(string p_Sentence) {
        //m_Word.Clear();
        dialogueText.text = p_Sentence;
        dialogueText.maxVisibleCharacters = 0;

        yield return new WaitForEndOfFrame();
        totalVisibleCharacter = dialogueText.textInfo.characterCount;
        counter = 0;
        yield return new WaitForEndOfFrame();
      
        visibleCount = counter % (totalVisibleCharacter + 1);

        while (visibleCount < totalVisibleCharacter) {
            visibleCount = counter % (totalVisibleCharacter + 1);
            dialogueText.maxVisibleCharacters = visibleCount;
            counter++;
            yield return new WaitForSeconds(textSpeed);
        }


        yield return new WaitForSeconds(0.5f);

        TriggerChoice();
    }

    public void NarrationOn(Dialogue p_Dialogue) {
        dialogueBoard.SetActive(true);
        StartDialogue(p_Dialogue);
    }

    public void NarrationOff() {
        dialogueBoard.SetActive(false);
        
        currentIndex++;

        if (currentIndex >= dialogues.Count) {
            currentIndex = 0;
            isDialogue = false;
            dialogues.Clear();
            Reset();
            onDialogueEnd?.Invoke();
            onDialogueEnd.RemoveAllListeners();
        } else {
            TriggerDialogue();
        }
    }

    public void TriggerChoice() {
        Debug.Log("Jalan sebelum disaster");
        if (isEnemy) {
            StartCoroutine(SpawnEnemyChoice());
        } else {
            if (activeDialogue.dialogue.dialogueType == Dialogue.DialogueType.TextOnly || activeDialogue.dialogue.choiceChoosen) return;
            isChoice = true;
            choiceBoard.SetActive(true);
            SpawnChoice();
        }
        Debug.Log(currentIndex);
        Debug.Log(activeDialogue);
    }

    private void SpawnChoice() {
        for(int i = 0; i < activeDialogue.dialogue.choice.Length; i++) {
            index = GetActiveChoice();

            if(index < 0) {
                choices.Add(Instantiate(choicePrefab));
                index = choices.Count - 1;
            }

            spawnedChoice = choices[index];
            spawnedChoice.transform.SetParent(choiceTransformParent);
            spawnedChoice.transform.localScale = Vector3.one;
            spawnedChoice.dialogue = activeDialogue.dialogue;
            //if (actions != null) {
            //    spawnedChoice.dialogue.choice[i].onPicked.AddListener(index => actions[0]?.Invoke(index));
            //}
            spawnedChoice.choiceIndex = i;
            if(spawnedChoice.dialogue.userDialogue.name == "") {
                spawnedChoice.dialogue.userDialogue.name = activeDialogue.dialogue.userDialogue.name;
            }
            spawnedChoice.gameObject.SetActive(true);
        }
    }

    private IEnumerator SpawnEnemyChoice() {
        for (int i = 0; i < activeDialogue.dialogue.choice.Length; i++) {
            index = GetActiveChoice();

            if (index < 0) {
                choices.Add(Instantiate(choicePrefab));
                index = choices.Count - 1;
            }

            spawnedChoice = choices[index];
            spawnedChoice.transform.SetParent(choiceTransformParent);
            spawnedChoice.transform.localScale = Vector3.one;
            spawnedChoice.dialogue = activeDialogue.dialogue;
            //if (actions != null) {
            //    spawnedChoice.dialogue.choice[i].onPicked.AddListener(index => actions[0]?.Invoke(index));
            //}

            spawnedChoice.choiceIndex = i;
            if (spawnedChoice.dialogue.userDialogue.name == "") {
                spawnedChoice.dialogue.userDialogue.name = activeDialogue.dialogue.userDialogue.name;
            }

            spawnedChoice.gameObject.SetActive(false);
        }

        isChoice = true;
        yield return new WaitForSeconds(1f);
        spawnedChoice.dialogue.Select(0);
    }

    private int GetActiveChoice() {
        for(int i = 0; i < choices.Count; i++) {
            if (!choices[i].gameObject.activeSelf) return i;
        }

        return -1;
    }
    
    public void ResetChoice() {
        DoReset();
    }

    public void DoReset() {
        for (int i = 0; i < choices.Count; i++) {
            for (int j = 0; j < choices[i].dialogue.choice.Length; j++) {
                choices[i].dialogue.choice[j].onPicked.RemoveAllListeners();
            }

            if (choices[i].gameObject.activeSelf) choices[i].gameObject.SetActive(false);
        }
        isEnemy = false;
        isChoice = false;
        choiceBoard.SetActive(false);
    }

    public void Reset() {
        for (int i = 0; i < choices.Count; i++) {
            for (int j = 0; j < choices[i].dialogue.choice.Length; j++) {
                choices[i].dialogue.choice[j].onPicked.RemoveAllListeners();
            }
            actions.Clear();
            choices[i].dialogue.choiceChoosen = false;
            if (choices[i].gameObject.activeSelf) choices[i].gameObject.SetActive(false);
        }
    }
}

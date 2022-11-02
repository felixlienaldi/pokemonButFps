using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Choice : MonoBehaviour
{
    public Dialogue dialogue;
    public int choiceIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select() {
        dialogue.Select(choiceIndex);
    }
}

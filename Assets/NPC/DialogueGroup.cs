using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueGroup", menuName = "ScriptableObjects/DialogueGroup")]
public class DialogueGroup : ScriptableObject
{
    public Dialogue[] dialogues;
}

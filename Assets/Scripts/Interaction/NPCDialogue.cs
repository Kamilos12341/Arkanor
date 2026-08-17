using System;
using UnityEngine;

[Serializable]
public class NPCDialogue
{
    [TextArea(2, 5)]
    public string[] lines;
}
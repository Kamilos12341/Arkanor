using System;
using UnityEngine;

namespace Arkanor.NPC
{

    [Serializable]
    public class NPCDialogue
    {
        [TextArea(2, 5)]
        public string[] lines;
    }
}
using UnityEngine;

namespace Arkanor.Dialogue
{
    [CreateAssetMenu(
        fileName = "NewNPCDialogue",
        menuName = "Arkanor/Dialogue/NPC Dialogue"
    )]
    public class NPCDialogue : ScriptableObject
    {
        [TextArea(2, 5)]
        public string[] lines;
    }
}
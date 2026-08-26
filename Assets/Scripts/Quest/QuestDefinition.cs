using UnityEngine;

namespace Arkanor.Quests
{
    [CreateAssetMenu(
        fileName = "NewQuest",
        menuName = "Arkanor/Quest/Quest Definition"
    )]
    public class QuestDefinition : ScriptableObject
    {
        public string ID;
        public string Title;
        [TextArea(2, 5)]
        public string Description;
        public string TargetID;
        public int RequiredAmount;
    }
}
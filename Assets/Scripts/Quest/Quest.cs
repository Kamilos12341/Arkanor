namespace Arkanor.Quests
{
    public class Quest
    {
        public string ID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public QuestState State { get; private set; }

        public int CurrentAmount { get; private set; }
        public int RequiredAmount { get; private set; }

        public string TargetID { get; private set; }

        public Quest(QuestDefinition definition)
        {
            if (definition == null)
                return;

            ID = definition.ID;
            Title = definition.Title;
            Description = definition.Description;

            TargetID = definition.TargetID;
            RequiredAmount = definition.RequiredAmount;

            CurrentAmount = 0;
            State = QuestState.NotStarted;
        }

        public void AddProgress(int amount)
        {
            if (State != QuestState.Active)
                return;

            CurrentAmount += amount;

            if (CurrentAmount >= RequiredAmount)
            {
                CurrentAmount = RequiredAmount;
                State = QuestState.Completed;
            }
        }

        public void Start()
        {
            if (State != QuestState.NotStarted)
                return;

            State = QuestState.Active;
        }

        public bool Complete()
        {
            if (State != QuestState.Completed)
                return false;

            State = QuestState.Rewarded;
            return true;
        }
    }
}
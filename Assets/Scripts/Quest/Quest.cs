using System;

[Serializable]
public class Quest
{
    public string ID;
    public string Title;
    public string Description;

    public QuestState State;

    public int CurrentAmount;
    public int RequiredAmount;

    public Quest(
        string id,
        string title,
        string description,
        int requiredAmount)
    {
        ID = id;
        Title = title;
        Description = description;

        RequiredAmount = requiredAmount;
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
}
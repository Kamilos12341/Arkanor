using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private Dictionary<string, Quest> quests = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CreateQuests();
    }

    private void CreateQuests()
    {
        Quest wolfQuest = new Quest(
            "kill_wolves_01",
            "Wilki na drodze",
            "Pokonaj 3 wilki, które zagrażają podróżnym.",
            3
        );

        AddQuest(wolfQuest);
    }

    public void AddQuest(Quest quest)
    {
        if (quest == null)
            return;

        if (quests.ContainsKey(quest.ID))
            return;

        quests.Add(quest.ID, quest);
    }

    public Quest GetQuest(string id)
    {
        if (quests.TryGetValue(id, out Quest quest))
        {
            return quest;
        }

        return null;
    }

    public void StartQuest(string id)
    {
        Quest quest = GetQuest(id);

        if (quest == null)
            return;

        if (quest.State != QuestState.NotStarted)
            return;

        quest.State = QuestState.Active;

        Debug.Log($"Quest rozpoczęty: {quest.Title}");
    }

    public void AddProgress(string id, int amount)
    {
        Quest quest = GetQuest(id);

        if (quest == null)
            return;

        quest.AddProgress(amount);

        Debug.Log(
            $"Postęp questa {quest.Title}: " +
            $"{quest.CurrentAmount}/{quest.RequiredAmount}"
        );
    }
}
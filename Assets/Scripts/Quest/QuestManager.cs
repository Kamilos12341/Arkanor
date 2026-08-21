using System.Collections.Generic;
using UnityEngine;
using Arkanor.Characters;

namespace Arkanor.Quests
{
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
                "wolf",
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

        public bool CompleteQuest(string id)
        {
            Quest quest = GetQuest(id);

            if (quest == null)
                return false;

            if (quest.State != QuestState.Completed)
                return false;

            quest.State = QuestState.Rewarded;

            Debug.Log($"Quest ukończony i odebrany: {quest.Title}");

            return true;
        }

        private void OnEnable()
        {
            Enemy.OnDied += HandleEnemyDeath;
        }

        private void OnDisable()
        {
            Enemy.OnDied -= HandleEnemyDeath;
        }

        private void HandleEnemyDeath(string enemyId)
        {
            foreach (Quest quest in quests.Values)
            {
                if (quest.State != QuestState.Active)
                    continue;

                if (quest.TargetID != enemyId)
                    continue;

                quest.AddProgress(1);

                Debug.Log(
                    $"Postęp questa {quest.Title}: " +
                    $"{quest.CurrentAmount}/{quest.RequiredAmount}"
                );
            }
        }
    }
}
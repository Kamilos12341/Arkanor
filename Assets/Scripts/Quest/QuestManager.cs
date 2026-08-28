using System.Collections.Generic;
using UnityEngine;
using Arkanor.Enemies;

namespace Arkanor.Quests
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        [SerializeField] private QuestDefinition[] questDefinitions;

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
            foreach (QuestDefinition definition in questDefinitions)
            {
                if (definition == null)
                    continue;

                Quest quest = new Quest(definition);

                AddQuest(quest);
            }
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

            quest.Start();

            Debug.Log($"Quest rozpoczęty: {quest.Title}");
        }

        public bool CompleteQuest(string id)
        {
            Quest quest = GetQuest(id);

            if (quest == null)
                return false;

            if (!quest.Complete())
                return false;

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
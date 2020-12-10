using UnityEngine;
using System.Collections.Generic;

namespace QuestingSystem
{
    [CreateAssetMenu(fileName = "New Dialogue Quest", menuName = "Quests/ Dialogue Quest")]
    public class DialogueQuest : Quest
    {
        [SerializeField] private DialogueQuestData[] _questDialogueName;
        public DialogueQuestData[] QuestDialogueName => _questDialogueName;

        public override void SetDefaultValues()
        {
            base.SetDefaultValues();
            for (int i = 0; i < _questDialogueName.Length; i++)
            {
                _questDialogueName[i].IsDialogueDone = false;
            }
        }
    }
    
    [System.Serializable]
    public struct DialogueQuestData
    {
        public string DialogueFileName;
        public bool IsDialogueDone;
    }
}
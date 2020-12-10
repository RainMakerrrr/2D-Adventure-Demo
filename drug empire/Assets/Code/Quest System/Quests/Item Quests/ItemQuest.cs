using InventorySystem;
using UnityEngine;

namespace QuestingSystem
{
    public class ItemQuest : Quest
    {
        [SerializeField] private ItemObject _itemObject;
       // [SerializeField] private InventoryBackpack _backpack;
        [SerializeField] private int _stuffAmount;
        [SerializeField] private Sprite _questIcon;
        public ItemObject ItemObject => _itemObject;

       // public InventoryBackpack Backpack => _backpack;

        public int StuffAmount
        {
            get => _stuffAmount;
            set => _stuffAmount = value;
        }

        public Sprite QuestIcon => _questIcon;
    }
}
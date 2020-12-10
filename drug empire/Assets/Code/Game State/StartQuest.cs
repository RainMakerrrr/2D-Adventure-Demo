using System;
using System.Collections;
using InventorySystem;
using PlayerController;
using QuestingSystem;
using QuestingSystem.UI;
using TimeSystem;
using UnityEngine;

public class StartQuest : MonoBehaviour
{
    [SerializeField] private ItemObject _item;
    [SerializeField] private ItemQuestReceiver _receiver;
    [SerializeField] private Player _player;
    

    public IEnumerator StartFirstQuest()
    {
        yield return new WaitForEndOfFrame();
        
        var itemQuest = (ItemQuest) _receiver.Quest;
        
        if(!_player.inventory.HasItem(_item, itemQuest.StuffAmount))
            _player.inventory.AddItem(_item, itemQuest.StuffAmount);
        
        _receiver.Quest.DeadlineTime = TimeManager.Instance.GameTime.Hour + 4;
        TimeManager.Instance.GameTime.DeadlineHours = _receiver.Quest.DeadlineTime - TimeManager.Instance.GameTime.Hour;
        TimeManager.Instance.GameTime.DeadlineMinuts = 0;
            
        QuestsCollection.Instance.AddReceiver(_receiver.Quest, _receiver);
        
    }

    private void Update()
    {
        if (_receiver != null)
            _receiver.Quest.OnQuestCompleted.AddListener(CompleteStartQuestHandler);
    }

    private void CompleteStartQuestHandler()
    {
        if (GameStateHandler.Instance.State == GameState.START)
        {
            QuestsUI.Instance.TooltipPanel.gameObject.SetActive(true);
            if(!QuestsUI.Instance.PhoneScrollRect.gameObject.activeSelf)
                QuestsUI.Instance.PhoneScrollRect.gameObject.SetActive(true);
            
            GameStateHandler.Instance.State = GameState.JOB;
        }
    }
}

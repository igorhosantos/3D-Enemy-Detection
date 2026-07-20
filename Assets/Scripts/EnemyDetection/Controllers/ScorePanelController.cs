using System.Collections.Generic;
using UnityEngine;
using Utils.ObjectPooling;

public class ScorePanelController : ControllerMonoBehaviour<HudInteractor,IHudOutput>, IObserver<int> {
    
    [SerializeField] private ObjectPoolingMonoBehaviour pooling;
    [SerializeField] private GameObject content;

    private readonly List<ScrollItemController> scrollItems = new List<ScrollItemController>();
    public void OnNotify(int totalSpawn) {
        for (var i = 0; i < content.transform.childCount; i++) {
            pooling.ReturnObjectInThePool(content.transform.GetChild(i).gameObject);
        }
        
        scrollItems.Clear();
        
        for (var i = 0; i < totalSpawn; i++) {
            var item = pooling.GetObjectInThePool().GetComponent<ScrollItemController>();
            scrollItems.Add(item);
            item.Initiate("Player" + i);
        }
    }

    public void MakeScore(PlayerData playerData) {
        for (int i = 0; i < scrollItems.Count; i++) {
            if (playerData.Id == scrollItems[i].PlayerId) {
                scrollItems[i].UpdatePlayerData(playerData);
            }
        }
    }
}


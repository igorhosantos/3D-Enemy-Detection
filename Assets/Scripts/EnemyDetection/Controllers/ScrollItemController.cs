using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollItemController : MonoBehaviour {
    [SerializeField] private Text playerName;
    [SerializeField] private Text playerHits;

    public string PlayerId => playerName.text;

    public void Initiate(string playerId) {
        name = playerName.text = playerId;
    }
    public void UpdatePlayerData(PlayerData playerData) {
        playerHits.text = "Total Hits Received: " + playerData.Hit;
    }
}


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : ControllerMonoBehaviour<HudInteractor,IHudOutput>, IObserver<int> {
    [SerializeField] private HudInputController inputController;
    [SerializeField] private ScorePanelController scorePanelController;
    [SerializeField] private Text totalAmount;

    public void Initiate(IObserver<int> observer) {
        inputController.Initiate(new List<IObserver<int>>{observer, this, scorePanelController});
    }

    public void OnNotify(int item) {
        totalAmount.text = "Amount Spawned : " + item;
    }
}


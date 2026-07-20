using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class HudInputController : ObservableMonoBehaviour<int> {

    private InputField input;
    private void Awake() {
        input = GetComponent<InputField>();
        input.onValueChanged.AddListener(OnChangeValue);
    }

    public void Initiate(List<IObserver<int>>observers) {
        for (int i = 0; i < observers.Count; i++) {
            Register(observers[i]);
        }
    }
    private void OnChangeValue(string value) {
        if (string.IsNullOrEmpty(value)) {
            return;
        }
        
        var amount = Int32.Parse(value);
        NotifyAllObservers(amount);
    }
}

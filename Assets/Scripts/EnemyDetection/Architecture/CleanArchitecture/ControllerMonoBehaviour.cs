using UnityEngine;

public abstract class ControllerMonoBehaviour<I,O> : MonoBehaviour where I : Interactor<O>, new() {
    
    protected I interactor;
    protected void Awake() {
        interactor = new I();
        interactor.ReceiveOutput(GetComponent<O>());
    }
}

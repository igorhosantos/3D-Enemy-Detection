using UnityEngine;

public class PlayerPresenter : MonoBehaviour, IPlayerOutput {

    public void SetPosition(Vector3 pos) {
        transform.localPosition = pos;
    }

    public void UpdateMovement(Vector3 pos) {
        transform.position = pos;
    }
}

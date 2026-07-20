using UnityEngine;

public interface IPlayerOutput {
    void SetPosition(Vector3 pos);
    void UpdateMovement(Vector3 pos);
}

using UnityEngine;

public class PlayerInteractor :  Interactor<IPlayerOutput> {

    private PlayerMovement playerMovement;
    private Vector3 currentDestiny;
    public void SetInitialPosition(PlayerMovement playerMovement) {
        this.playerMovement = playerMovement;
        output.SetPosition(GenerateRandomPosition());

        currentDestiny = GenerateRandomPosition();
    }

    private Vector3 GenerateRandomPosition() {
        var posX = Random.Range(playerMovement.xPosition.Min, playerMovement.xPosition.Max);
        var posY = Random.Range(playerMovement.yPosition.Min, playerMovement.yPosition.Max);
        var posZ = Random.Range(playerMovement.zPosition.Min, playerMovement.zPosition.Max);
        return new Vector3(posX, posY,posZ);
    }
    

    public void UpdateMovement(float speed, Vector3 position, float distance) {
        output.UpdateMovement(Vector3.Lerp(position,currentDestiny, speed * Time.deltaTime));
        
        // check distance for generate new destiny
        if(Vector3.Distance(position, currentDestiny) < distance){
            currentDestiny = GenerateRandomPosition();
        }
    }
}

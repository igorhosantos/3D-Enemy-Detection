using System;

[Serializable]
public class PlayerData {
    
    public String Id { get; private set; }
     public int Hit { get; private set; }
    
    public PlayerData(string id, int hit) {
        this.Id = id;
        this.Hit = hit;
    }
}



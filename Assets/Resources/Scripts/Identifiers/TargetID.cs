using UnityEngine;

public class TargetID : MonoBehaviour {
    public string type;
    public string produces;
    public string job;
    public bool engaged = false;
    public int targetedBy = 0;
    public bool selected = false;

    public void Harvestify() {
        job = "harvester";
        SetDefaults();
    }

    public void Woodify() {
        type = "wood";
        job = "hauler";
        SetDefaults();
    }

    private void SetDefaults() {
        engaged = false;
        targetedBy = 0;
    }

    public void AbandonTask() {
        SetDefaults();
    }
}

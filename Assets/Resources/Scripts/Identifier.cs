using UnityEngine;

public class Identifier : MonoBehaviour {
    public string type;
    public string produces;
    public string job;
    public bool engaged = false;
    public int targetedBy = 0;
    public bool selected = false;

    public void Chopify() {
        job = "chopper";
        SetDefaults();
    }

    public void Logify() {
        type = "logs";
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

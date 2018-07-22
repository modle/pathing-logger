using UnityEngine;

public class TargetID : MonoBehaviour {
    public string type;
    public string produces;
    public string job;
    public bool engaged;
    public int targetedBy = 0;
    public bool selected;
    public bool destructable;
    public bool workable;

    public void Harvestify() {
        job = "harvester";
        workable = true;
        destructable = false;
        SetDefaults();
    }

    public void Haulify() {
        job = "hauler";
        workable = false;
        destructable = true;
        SetDefaults();
    }

    private void SetDefaults() {
        engaged = false;
        targetedBy = 0;
    }

    public void AbandonTask() {
        SetDefaults();
    }

    public void ChangeSprite() {
        name = produces;
        GetComponent<SpriteRenderer>().sprite =
            TargetManager.manager.harvestedSprites[type].GetComponent<SpriteRenderer>().sprite;
    }
}

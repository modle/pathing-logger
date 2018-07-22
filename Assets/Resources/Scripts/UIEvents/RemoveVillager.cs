using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveVillager : EventTrigger {

    public override void OnPointerClick(PointerEventData eventData) {
        VillagerBucket.bucket.ReassignVillager(tag, "hauler");
    }
}

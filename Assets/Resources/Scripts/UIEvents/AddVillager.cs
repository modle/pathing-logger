using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddVillager : EventTrigger {

    public string job;

    public override void OnPointerClick(PointerEventData eventData) {
        VillagerBucket.bucket.ReassignVillager("hauler", job);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddVillager : EventTrigger {

    public override void OnPointerClick(PointerEventData eventData) {
        VillagerBucket.bucket.ReassignVillager("hauler", tag);
    }
}

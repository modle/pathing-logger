using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayUpdater : MonoBehaviour {

    public Building building;
    public Villager villager;

    public void Update() {
        // TODO: this is ugly. Generics?
        if (building != null) {
            transform.Find("text").GetComponent<Text>().text = building.GetRepr();
        } else if (villager != null) {
            transform.Find("text").GetComponent<Text>().text = villager.GetRepr();
        }
    }
}

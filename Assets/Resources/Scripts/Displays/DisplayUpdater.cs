using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayUpdater : MonoBehaviour {

    public Building building;

    public void Update() {
        transform.Find("text").GetComponent<Text>().text = building.GetRepr();
    }
}

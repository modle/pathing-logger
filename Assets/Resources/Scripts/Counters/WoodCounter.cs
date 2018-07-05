using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodCounter : MonoBehaviour {
    public static WoodCounter counter;
    public int count;
    public Text counterText;

    void Awake() {
        // singleton pattern
        if (counter == null) {
            DontDestroyOnLoad(gameObject);
            counter = this;
        } else if (counter != this) {
            Destroy(gameObject);
        }
    }

	void Update () {
        counterText.text = count.ToString();
	}
}

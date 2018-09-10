using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class MessageLog : MonoBehaviour {

    public static MessageLog log;

    private List<string> messages = new List<string>();
    private GameObject messageContainer;

    void Awake() {
        // singleton pattern
        if (log == null) {
            DontDestroyOnLoad(gameObject);
            log = this;
        } else if (log != this) {
            Destroy(gameObject);
        }
        messageContainer = transform.Find("MessageContainer").gameObject;
    }

    public void Update() {
        string output = "";
        foreach (string message in messages) {
            output += "\n" + message;
        }
        messageContainer.GetComponent<TextMeshProUGUI>().text = output;
    }

    public void Publish(string message) {
        messages.Insert(0, message);
        if (messages.Count > 20) {
            messages.RemoveAt(messages.Count - 1);
        }
    }
}

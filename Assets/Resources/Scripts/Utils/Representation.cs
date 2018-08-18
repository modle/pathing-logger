using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Representation : MonoBehaviour {

    public static Representation repr;

    void Awake() {
        // singleton pattern
        if (repr == null) {
            DontDestroyOnLoad(gameObject);
            repr = this;
        } else if (repr != this) {
            Destroy(gameObject);
        }
    }

    public string CapitalizeFirstLetter(string s) {
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
}

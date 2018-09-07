using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResearchCosting : MonoBehaviour {
    
    [System.Serializable]
    public struct Material {
        public string name;
        public int amount;
    }

    public List<Material> researchMaterials = new List<Material>();
}

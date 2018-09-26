using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * This object is attached to the villager prefab
 */
public class Targets : MonoBehaviour {
    public GameObject target;
    public bool collided;
    public GameObject collisionObject;
    private Vector3 lastPosition = new Vector3(0, 0, 0);
    public float lastCollisionRecheck;
    private float collisionRecheck = 3.0f;

    public void Start() {
        lastCollisionRecheck = Time.time;
    }

    public int CountAvailableTargets() {
        int count = 0;
        foreach (GameObject go in TargetBucket.bucket.targets) {
            if (go == null) {
                continue;
            }
            Properties compareProps = go.GetComponent<Properties>();
            if (compareProps.targeted) {
                continue;
            }
            count++;
        }
        return count;
    }

    public bool HasTarget() {
        if (target == null) {
            GetClosest();
        }
        if (target != null) {
            return true;
        }
        return false;
    }

    private void GetClosest() {
        if (target != null) {
            return;
        }
        GameObject closest = CompareToTargets();
        if (closest != null) {
            target = closest;
            target.GetComponent<Properties>().SetTargeted(GetComponent<Properties>().id);
            if (target.GetComponent<Properties>().type == "building") {
                ProcessCollision(target);
            }
        }
    }

    private GameObject CompareToTargets() {
        Vector3 position = transform.position;
        float distance = Mathf.Infinity;
        GameObject match = null;
        foreach (GameObject go in TargetBucket.bucket.targets) {
            if (go == null) {
                continue;
            }
            Properties compareProps = go.GetComponent<Properties>();
            if (!compareProps.selected || compareProps.targeted || compareProps.job != GetComponent<Properties>().job) {
                continue;
            }
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                match = go;
                distance = curDistance;
            }
        }
        return match;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        ProcessCollision(other.gameObject);
    }

    public void ProcessCollision(GameObject other) {
        if (collided) {
            return;
        }
        if (other == null) {
            return;
        }
        if (target == null || other.GetComponent<Properties>() == null) {
            // nothing to do
            return;
        }
        if (other.gameObject.GetInstanceID() == target.GetInstanceID()) {
            collided = true;
            collisionObject = other;
        }
    }

    public void CheckForRecollision() {
        /*
            if position has not changed after a set time and target is not null, trigger collision;
            prevents villager from getting stuck on objects when target changes
            while villager is inside collision border
        */
        if (Time.time - lastCollisionRecheck < collisionRecheck) {
            return;
        }
        if (transform.position == lastPosition && target != null) {
            ProcessCollision(target);
            lastCollisionRecheck = Time.time;
            return;
        }
        lastPosition = transform.position;
    }

    public void DecomposeTarget() {
        if (target.GetComponent<Properties>().destructable) {
            Destroy(target);
            lastCollisionRecheck = Time.time;
        }
        target = GameObject.Find("Storage");
    }
}

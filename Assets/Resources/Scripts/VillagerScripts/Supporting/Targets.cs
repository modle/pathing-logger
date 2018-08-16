using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Targets : MonoBehaviour {

    public GameObject target;
    public AudioSource audioSource;
    public AudioClip storageClip;
    public bool collided;
    public bool recollide;
    public GameObject collisionObject;

    private Villager villager;
    private Job job;
    private State state;
    private Work work;
 
    public void Start() {
        villager = GetComponent<Villager>();
        job = GetComponent<Job>();
        state = GetComponent<State>();
        work = GetComponent<Work>();
        audioSource = GetComponent<AudioSource>();
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
            target.GetComponent<Properties>().SetTargeted(villager.id);
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
            Properties props = go.GetComponent<Properties>();
            if (!props.selected || props.targeted || props.job != job.GetCurrentJob()) {
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

    private void OnCollisionStay2D(Collision2D other) {
        if (target == null) {
            return;
        }
        if (other.gameObject.GetInstanceID() != target.GetInstanceID()) {
            return;
        }
        // why?
        if (!work.working) {
            ProcessCollision(other.gameObject);
            return;
        }
        // prevents villager from getting stuck when inside storage
        if (target.name == "Storage") {
            ProcessCollision(other.gameObject);
        }
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
        if (recollide && collisionObject != null) {
            return;
        }
        collided = true;
        collisionObject = other;
    }

    public void CollectTarget(Properties props) {
        work.material = target.GetComponent<Properties>().produces;
        if (props.destructable) {
            Destroy(target);
        }
        target = GameObject.Find("Storage");
        work.haveMaterials = true;
    }

    public void PutInStorage() {
        target = null;
        work.haveMaterials = false;
        audioSource.PlayOneShot(storageClip, 0.7F);
        ResourceCounter.counter.counts[work.material]++;
        work.material = "";

        job.TriggerCheckJob();
    }

    public void GetFromStorage(GameObject other) {
        // every instruction accesses villager; move it to Villager?
        if (ResourceCounter.counter.counts[work.material] > 0) {
            target = work.building.transform.gameObject;
            work.haveMaterials = true;
            ResourceCounter.counter.counts[work.material]--;
            recollide = false;
            collisionObject = null;
        } else {
            recollide = true;
            collisionObject = other;
        }
    }
}

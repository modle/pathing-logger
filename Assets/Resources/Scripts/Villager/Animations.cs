using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Animations : MonoBehaviour {

    public Animator anim;
    private SpriteRenderer spriteRenderer;
    bool idleFlipX = false;
    private int speedMod = 20;
    private float horizontal = 0f;
    private float vertical = 0f;
    private float theX = 0f;
    private float theY = 0f;
    public string currentDirection = "";
    private float lastMoved = 0f;
    float maxMoveDist = 1.5f;

    public Dictionary<string, string> directions = new Dictionary<string, string>() {
        {"-1,0", "side"},
        {"1,0", "side"},
        {"0,1", "up"},
        {"0,-1", "down"},
        {"1,1", "side"},
        {"-1,1", "side"},
        {"1,-1", "side"},
        {"-1,-1", "side"},
        {"0,0", "idle"}
    };

    public void Start () {
        SetInitialReferences();
    }

    public void SetAnimation(string direction, bool state) {
        anim.SetBool(direction, state);
        currentDirection = state ? direction : currentDirection;
    }

    void SetInitialReferences() {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(GameObject target) {
        SetDefaultDirections();
        SetDefaultCoordinates();
        GetTargetCoordinates(target);
        SetDirections();
        if (horizontal == 0 && vertical == 0) {
            return;
        }
        MoveSprite();
    }

    public void SetDefaultDirections() {
        SetAnimation("side", false);
        SetAnimation("up", false);
        SetAnimation("down", false);
        SetAnimation("side-attack", false);
    }

    public void SetDefaultCoordinates() {
        theX = 0f;
        theY = 0f;
        horizontal = 0;
        vertical = 0;
        speedMod = 20;
    }

    public void SetDirections() {
        string direction = directions[horizontal + "," + vertical];
        bool flipX = (horizontal != 0 && horizontal == 1) ? false : true;
        spriteRenderer.flipX = flipX;
        idleFlipX = direction == "idle" ? idleFlipX : flipX;
        if (direction == "idle") {
            spriteRenderer.flipX = idleFlipX;
        } else {
            anim.speed = 1;
            SetAnimation(direction, true);
        }
    }

    void GetTargetCoordinates(GameObject target) {
        theX = transform.position.x - target.transform.position.x;
        theY = transform.position.y - target.transform.position.y;
        if (target.GetComponent<Properties>().type == "tree") {
            theX = transform.position.x - (target.transform.position.x + target.gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f);
            theY = transform.position.y - (target.transform.position.y - target.gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 0.3f);
        }
        if (theX < -0.01f) {
            horizontal = 1;
        } else if (theX > 0.01f) {
            horizontal = -1;
        }
        if (theY < -0.01f) {
            vertical = 1;
        } else if (theY > 0.01f) {
            vertical = -1;
        }
    }

    bool MoveSprite() {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontal / speedMod, vertical / speedMod);

        if (horizontal != 0 || vertical != 0) {
            transform.position = end;
            return true;
        }
        return false;
    }

    public void MoveRandomly() {
        // if we get here, it's because we have no target; so drift aimlessly
        // set check that we're within a max distance from storage here
        // only set if it's been x time since last set

        // slow them down
        speedMod = 100;
        if (Time.time - lastMoved > 0.4f) {
            SetDefaultDirections();
            vertical = Random.Range(0, 3) - 1;
            horizontal = Random.Range(0, 3) - 1;
            SetDirections();
            lastMoved = Time.time;
        }

        // boundary check
        if (transform.position.x > maxMoveDist) {
            horizontal = -1;
        }
        if (transform.position.x < -maxMoveDist) {
            horizontal = 1;
        }
        if (transform.position.y > maxMoveDist) {
            vertical = -1;
        }
        if (transform.position.y < -maxMoveDist) {
            vertical = 1;
        }
        MoveSprite();
    }
}

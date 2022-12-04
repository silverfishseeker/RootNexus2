using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {
    
    bool canJump;

    void Start() {
        //gameObject.GetComponent<Transform>().position = new Vector3(4, 0, 10);
        //pos = gameObject.Transform.position;
    }

    // Update is called once per frame
    void Update() {
        //ManageJump();
        /*gameObject.transform.position = new Vector3(
            gameObject.transform.position.x-0.1f*Time.deltaTime,
            gameObject.transform.position.y,
            gameObject.transform.position.z
            );
        gameObject.transform.Translate(0,0.2f*Time.deltaTime,0);*/
        // float step = 4f * Time.deltaTime;
        // float xstep = 0;
        // if (Input.GetKey("right")) xstep += step;
        // if (Input.GetKey("left")) xstep -= step;

        // gameObject.transform.Translate(xstep, 0, 0);

        if (gameObject.transform.position.y < -10) {
            gameObject.transform.position = new Vector3(0,0,0);
            gameObject.transform.rotation  = new Quaternion(0,0,0,0);
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;//new Vector3(0,0,0);
        }
        if (Input.GetKey("right")) {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000f*Time.deltaTime, 0));
            gameObject.GetComponent<Animator>().SetBool("moving", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetKey("left")) {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000f*Time.deltaTime, 0));
            gameObject.GetComponent<Animator>().SetBool("moving", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (!(Input.GetKey("left") || Input.GetKey("right"))) {
            gameObject.GetComponent<Animator>().SetBool("moving", false);
        }

        if (Input.GetKeyDown("up") && canJump) {
            canJump = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000f));
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "Ground")
            canJump = true;
    }

    void ManageJump(){
        if (gameObject.transform.position.y < 0)
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                0,
                gameObject.transform.position.z);

        if (gameObject.transform.position.y == 0)
            canJump = true;
        
        if (Input.GetKey("up") && canJump && transform.position.y <= 10)
            gameObject.transform.Translate(0, 10f * Time.deltaTime, 0);
        else {
            canJump = false;
            if (gameObject.transform.position.y > 0)
                gameObject.transform.Translate(0, -20f * Time.deltaTime, 0);
        }
    }
}

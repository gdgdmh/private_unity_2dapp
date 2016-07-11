using UnityEngine;
using System.Collections;

public class Rigidbody2dVelocityData : MonoBehaviour {

    public Rigidbody2dVelocityData(Rigidbody2D rigidbody2d) {
        velocity_ = rigidbody2d.velocity;
        angular_velocity_ = rigidbody2d.angularVelocity;
    }
    public Vector2 velocity_;
    public float angular_velocity_;
}


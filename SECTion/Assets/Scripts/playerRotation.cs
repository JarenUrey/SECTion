using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRotation : MonoBehaviour{
    
    void Update(){
        //tracks mouse position in the world
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //rotates the player relative to mouse position
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x,
         mousePosition.y - transform.position.y);

        transform.up = direction;
    }
}

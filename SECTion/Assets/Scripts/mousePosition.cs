using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class mousePosition : MonoBehaviour{

    [SerializeField] private Camera mainCamera;
    [SerializeField] public Sprite currentSprite;
    [SerializeField] public Sprite cursorAiming;
    [SerializeField] public Sprite cursorDefault;


    public bool aiming;

    private void Start()
    {
        aiming = false;
    }
    private void Update() {
        
        //set mouse position to z = 0
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
        GetComponent<SpriteRenderer>().sprite = currentSprite;

        if (aiming == true)
        {
            currentSprite = cursorAiming;
        }
        else
        {
            currentSprite = cursorDefault;
        }
    }

}

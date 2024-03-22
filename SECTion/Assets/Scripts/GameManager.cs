using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("---- Player Stuff ----")]
    public GameObject player;
    public CharacterController2D characterController2D;

    [Header("---- UI ----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] public GameObject revolverAmmoCount;

    [SerializeField] public GameObject currRevolverAmmo;
    [SerializeField] public GameObject revolverFiveShot;
    [SerializeField] public GameObject revolverFourShot1;
    [SerializeField] public GameObject revolverFourShot2;
    [SerializeField] public GameObject revolverThreeShot;
    [SerializeField] public GameObject revolverTwoShot;
    [SerializeField] public GameObject revolverOneShot;
    [SerializeField] public GameObject revolverZeroShot;

    public bool isPaused;
    float timeScaleOrig;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            statePause();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;

        menuActive.SetActive(false);
        menuActive = null;
    }
}

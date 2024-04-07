using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("---- Player Stuff ----")]
    public GameObject player;
    public CharacterController2D controller;
    public GameObject PlayerSpawnPos;

    [Header("---- UI ----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuLose;

    [SerializeField] public GameObject healthActive;
    [SerializeField] public Image playerHealthBar;

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

    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        PlayerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        healthActive.SetActive(true);
    }

    void Update()
    {
        //when the player presses ESC
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
        controller.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        controller.enabled = true;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}

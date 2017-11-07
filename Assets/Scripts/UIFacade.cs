﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFacade : MonoBehaviour {

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject readme;

    [Space(10f)]

    public Singleplayer singleplayer;
    public LocalMultiplayer localMultiplayer;

    //Singleton!
    public static UIFacade Singleton
    {
        get; private set;
    }

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;
    }

    private void Start()
    {
        Observer.Singleton.onReadme += DoneReadme;
    }

    public void SetActiveMainMenu(bool state)
    {
        mainMenu.SetActive(state);
    }

    public void Done()
    {
        SceneManager.LoadScene(0);
    }

    public void DoneReadme()
    {
        if (readme.activeInHierarchy)
            readme.SetActive(false);
        else
            readme.SetActive(true);
    }
}

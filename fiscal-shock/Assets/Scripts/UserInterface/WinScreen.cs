﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {
    private GameObject loadingScreen;
    private LoadingScreen loadScript;

    public void Start() {
        Settings.forceUnlockCursorState();
        loadingScreen = GameObject.FindGameObjectWithTag("Loading Screen");
        loadScript = loadingScreen.GetComponent<LoadingScreen>();
    }

    public void QuitClick() {
        Debug.Log("Quit by win game.");
        StateManager.playerWon = false;
        Settings.quitToDesktop();
    }

    public void GoBack() {
        GameObject.FindGameObjectWithTag("Spawn Point").GetComponent<SpawnPoint>().resetToHubDefaults();
        StateManager.playerWon = false;
        loadScript.startLoadingScreen("Hub");
    }
}

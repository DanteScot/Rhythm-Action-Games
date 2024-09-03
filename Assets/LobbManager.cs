using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LobbManager : MonoBehaviour
{
    public static LobbManager Instance { get; private set; }

    private bool isFirstTime;

    [SerializeField] private GameObject[] itemToDestroy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null) isFirstTime = false;
        else isFirstTime = true;

        if (!isFirstTime){
            NotFirstTimeLobby();
            PlayerManager.Instance.LoadPlayerStats(data);
        }

        SaveSystem.SaveGame(new GameData(PlayerManager.Instance));
    }

    void NotFirstTimeLobby(){
        foreach (var item in itemToDestroy)
        {
            Destroy(item);
        }

        foreach (var item in FindObjectsOfType<Light2D>())
        {
            item.enabled = false;
        }

        foreach (var item in FindObjectsOfType<Interactable>())
        {
            item.enabled = true;
        }
    }
}
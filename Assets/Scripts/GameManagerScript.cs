using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public int score;
    private List<BlockScript> blockList;

    public static GameManagerScript instance = null;
    [HideInInspector] public PlayerScript PS;
    [HideInInspector] public GameMenuHandler GMH;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        PS = GameObject.Find("Player").GetComponent<PlayerScript>();
        GMH = GameObject.Find("Canvas/GameMenuHandler").GetComponent<GameMenuHandler>();
        Transform gridT = GameObject.Find("Environment/Grid").transform;
        BlockScript[] blockArray = gridT.GetComponentsInChildren<BlockScript>();
        blockList = new List<BlockScript>(blockArray);
        GMH.ScoreText(score);
    }

    public void UpdateProgress(BlockScript bs)
    {
        score++;
        GMH.ScoreText(score);
        if (bs.health == 0) blockList.Remove(bs);
        if (blockList.Count == 0) StartCoroutine(GMH.LevelCompletedCoroutine());
    }
}

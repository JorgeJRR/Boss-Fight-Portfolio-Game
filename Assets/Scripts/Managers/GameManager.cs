using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public DoorScript door;

    public Image transitionCurtain;

    private void Awake()
    {
        Instance = this;
    }

    public void EnterBossScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        door.OpenDoor();
        transitionCurtain.DOFade(1, 1).OnComplete(() => SceneManager.LoadScene(nextScene));
    }
}

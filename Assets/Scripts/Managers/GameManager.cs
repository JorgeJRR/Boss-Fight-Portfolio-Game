using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public DoorScript door;

    public Image transitionCurtain;

    public CanvasGroup winCurtain;

    private void Awake()
    {
        Instance = this;
        winCurtain.alpha = 0;
    }

    public void OpenedDoorCurtain ()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        door.OpenDoor();

        transitionCurtain.DOFade(1, 1).OnComplete(() => PrepareToChangeScene(nextScene));
    }

    public void PrepareToChangeScene(int nextScene)
    {
        PlayerController.Instance.gameObject.transform.position = new Vector3(-6.5f, -1.6f, 0);
        SceneManager.LoadScene(nextScene);
    }

    public void PlayerWinPanel()
    {
        PlayerController.Instance.gameObject.transform.DOMove(new Vector3(0, -1, 0), 1).OnComplete(() => PlayerController.Instance.PlayerWin());
        winCurtain.DOFade(1, 1);
    }
}

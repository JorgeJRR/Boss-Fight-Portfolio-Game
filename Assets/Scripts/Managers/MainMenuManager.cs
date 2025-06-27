using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup buttonsPanel;
    [SerializeField]
    private RectTransform tutorialPanel;

    public float buttonsPanelFadeTime;

    [SerializeField]
    private TextMeshProUGUI blinkText;
    private InputAction anyKeyAction;
    private Tween blinkTween;

    private void Awake()
    {
        anyKeyAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/anyKey");
        anyKeyAction.performed += ctx => StopBlinkingText();

        StartBlinkingText();
    }

    private void OnEnable()
    {
        anyKeyAction.Enable();
    }

    private void OnDisable()
    {
        anyKeyAction.Disable();
    }

    public void StartBlinkingText()
    {
        blinkTween = blinkText.DOFade(0f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void StopBlinkingText()
    {
        if (blinkTween != null && blinkTween.IsActive())
        {
            blinkTween.Kill();
            Color c = blinkText.color;
            c.a = 0f;
            blinkText.color = c;
        }
        anyKeyAction.Disable();
        ShowButtonsPanel();
    }

    private void ShowButtonsPanel()
    {
        buttonsPanel.DOFade(1, buttonsPanelFadeTime).SetDelay(0.5f).SetEase(Ease.InOutQuad)
                  .OnStart(() => {
                      buttonsPanel.interactable = true;
                      buttonsPanel.blocksRaycasts = true;
                  });
    }

    public void StartGame()
    {
        buttonsPanel.DOFade(0, buttonsPanelFadeTime)
            .SetDelay(0.5f)
            .SetEase(Ease.InOutQuad)
            .OnStart(() => {
                buttonsPanel.interactable = false;
                buttonsPanel.blocksRaycasts = false;
            })
            .OnComplete(() => { SceneManager.LoadScene(1); });
    }

    public void ShowTutorialPanel()
    {
        tutorialPanel.DOOffsetMin(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
        tutorialPanel.DOOffsetMax(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
    }

    public void HideTutorialPanel()
    {
        tutorialPanel.DOOffsetMin(Vector2.up * 1080, 0.5f).SetEase(Ease.OutCubic);
        tutorialPanel.DOOffsetMax(Vector2.up * 1080, 0.5f).SetEase(Ease.OutCubic);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;

    [Header("Credits")]
    [SerializeField] private GameObject creditsImage;

    [Header("Fade Panel")]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration = 2f;

    private bool creditsVisible = false;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        creditsButton.onClick.AddListener(ShowCredits);
        backButton.onClick.AddListener(HideCredits);
        exitButton.onClick.AddListener(ExitGame);

        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            fadePanel.color = new Color(c.r, c.g, c.b, 1f);
            StartCoroutine(FadeIn());
        }

        if (creditsImage != null)
            creditsImage.SetActive(false);

        if (backButton != null)
            backButton.gameObject.SetActive(false);
    }

    void PlayGame()
    {
        SceneManager.LoadScene("DialoguePrototype");
    }

    void ShowCredits()
    {
        if (creditsImage != null)
            creditsImage.SetActive(true);

        if (backButton != null)
            backButton.gameObject.SetActive(true);
    }

    void HideCredits()
    {
        if (creditsImage != null)
            creditsImage.SetActive(false);

        if (backButton != null)
            backButton.gameObject.SetActive(false);
    }

    void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color originalColor = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUISound : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(PlaySound);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlayButtonClick();

    }
}
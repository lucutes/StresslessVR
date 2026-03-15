using TMPro;
using UnityEngine;

public class PromptUIController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField promptInputField;
    public PromptFeedbackUI promptFeedbackUI;

    [Header("System References")]
    public PromptInterpreter promptInterpreter;
    public EnvironmentManager environmentManager;

    public void OnGenerateButtonClicked()
    {
        if (promptInputField == null)
        {
            Debug.LogError("Prompt Input Field is not assigned.");
            return;
        }

        string prompt = promptInputField.text.Trim();

        if (string.IsNullOrEmpty(prompt))
        {
            Debug.LogWarning("Prompt is empty.");

            if (promptFeedbackUI != null)
                promptFeedbackUI.ClearFeedback();

            return;
        }

        if (promptInterpreter == null)
        {
            Debug.LogError("PromptInterpreter is not assigned.");
            return;
        }

        if (environmentManager == null)
        {
            Debug.LogError("EnvironmentManager is not assigned.");
            return;
        }

        SceneSettings settings = promptInterpreter.InterpretPrompt(prompt);

        environmentManager.GenerateEnvironment(settings);

        if (promptFeedbackUI != null)
        {
            promptFeedbackUI.UpdateFeedback(settings);
        }
    }
}
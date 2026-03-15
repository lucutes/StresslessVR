using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptFeedbackUI : MonoBehaviour
{
    public TMP_Text feedbackText;
    public RectTransform panelRect;

    public void UpdateFeedback(SceneSettings settings)
    {
        if (feedbackText == null)
        {
            Debug.LogError("Feedback Text is not assigned.");
            return;
        }

        feedbackText.text =
        $"Aplinka: {GetEnvironmentName(settings.environmentType)}\n" +
        $"Paros laikas: {GetTimeName(settings.timeOfDay)}\n" +
        $"Orai: {GetWeatherName(settings.weatherType)}\n" +
        $"Nuotaika: {GetMoodName(settings.moodType)}";
        // Force layout rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
    }

    public void ClearFeedback()
    {
        if (feedbackText == null)
            return;

        feedbackText.text =
            "Environment: -\n" +
            "Time: -\n" +
            "Weather: -\n" +
            "Mood: -";
    }
    string GetEnvironmentName(EnvironmentType type)
{
    switch (type)
    {
        case EnvironmentType.Forest: return "Miškas";
        case EnvironmentType.City: return "Miestas";
        case EnvironmentType.Ocean: return "Jūra";
        default: return "-";
    }
}

string GetTimeName(TimeOfDay time)
{
    switch (time)
    {
        case TimeOfDay.Day: return "Diena";
        case TimeOfDay.Night: return "Naktis";
        case TimeOfDay.Sunset: return "Vakaras";
        case TimeOfDay.Dawn: return "Aušra";
        default: return "-";
    }
}

string GetWeatherName(WeatherType weather)
{
    switch (weather)
    {
        case WeatherType.Foggy: return "Rūkas";
        case WeatherType.Clear: return "Giedra";
        default: return "-";
    }
}

string GetMoodName(MoodType mood)
{
    switch (mood)
    {
        case MoodType.Calm: return "Rami";
        case MoodType.Neutral: return "Normali";
        default: return "-";
    }
}
}

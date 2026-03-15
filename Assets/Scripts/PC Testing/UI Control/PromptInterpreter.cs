using UnityEngine;

public class PromptInterpreter : MonoBehaviour
{
    public SceneSettings InterpretPrompt(string prompt)
    {
        SceneSettings settings = new SceneSettings();

        if (string.IsNullOrWhiteSpace(prompt))
            return settings;

        string p = prompt.ToLower();

        // Environment type

        if (p.Contains("mišk") || p.Contains("medž") || p.Contains("gamt"))
        {
            settings.environmentType = EnvironmentType.Forest;
        }
        else if (p.Contains("miest") || p.Contains("daugiabut") || p.Contains("pastat"))
        {
            settings.environmentType = EnvironmentType.City;
        }
        else if (p.Contains("jūr") || p.Contains("vandenyn") || p.Contains("koral") || p.Contains("po vandeniu"))
        {
            settings.environmentType = EnvironmentType.Ocean;
        }

        // Time of day

        if (p.Contains("nakt"))
        {
            settings.timeOfDay = TimeOfDay.Night;
        }
        else if (p.Contains("saulėlyd") || p.Contains("vakar"))
        {
            settings.timeOfDay = TimeOfDay.Sunset;
        }
        else if (p.Contains("aušr") || p.Contains("rytas"))
        {
            settings.timeOfDay = TimeOfDay.Dawn;
        }
        else
        {
            settings.timeOfDay = TimeOfDay.Day;
        }

        // Weather

        if (p.Contains("rūk"))
        {
            settings.weatherType = WeatherType.Foggy;
        }

        // Mood

        if (p.Contains("ramu") || p.Contains("rami"))
        {
            settings.moodType = MoodType.Calm;
        }

        return settings;
    }
}
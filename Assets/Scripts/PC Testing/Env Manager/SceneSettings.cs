using UnityEngine;

[System.Serializable]
public class SceneSettings
{
    public EnvironmentType environmentType = EnvironmentType.None;
    public TimeOfDay timeOfDay = TimeOfDay.Day;
    public WeatherType weatherType = WeatherType.Clear;
    public MoodType moodType = MoodType.Neutral;
}

public enum EnvironmentType
{
    None,
    Forest,
    City,
    Ocean
}

public enum TimeOfDay
{
    Day,
    Night,
    Sunset,
    Dawn
}

public enum WeatherType
{
    Clear,
    Foggy
}

public enum MoodType
{
    Neutral,
    Calm
}
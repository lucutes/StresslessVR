using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Skyboxes")]
public Material daySkybox;
public Material nightSkybox;
public Material sunsetSkybox;
public Material dawnSkybox;
    [Header("Scene References")]
    public Light directionalLight;
    public Transform spawnParent;
    public GameObject ground;

    [Header("Prefabs")]
    public GameObject treePrefab;
    public GameObject apartmentPrefab;
    public GameObject coralPrefab;

    [Header("Spawn Settings")]
    public int treeCount = 25;
    public int apartmentCount = 8;
    public int coralCount = 20;
    public float spawnRange = 25f;

    private readonly List<GameObject> spawnedObjects = new();

    public void GenerateEnvironment(SceneSettings settings)
    {
        ClearEnvironment();
        ApplyDefaultSettings();

        switch (settings.environmentType)
        {
            case EnvironmentType.Forest:
                ApplyForestSettings(settings);
                SpawnObjects(treePrefab, treeCount, new Vector2(0.8f, 1.4f), true, 0f);
                break;

            case EnvironmentType.City:
                ApplyCitySettings(settings);
                SpawnObjects(apartmentPrefab, apartmentCount, new Vector2(1f, 1.2f), true, 2f);
                break;

            case EnvironmentType.Ocean:
                ApplyOceanSettings(settings);
                SpawnObjects(coralPrefab, coralCount, new Vector2(0.7f, 1.3f), true, 1.5f);
                break;

            default:
                break;
        }

        ApplyTimeOfDay(settings.timeOfDay);
        ApplyWeather(settings.weatherType);
        ApplyMood(settings.moodType);

        Debug.Log($"Environment: {settings.environmentType}, Time: {settings.timeOfDay}, Weather: {settings.weatherType}, Mood: {settings.moodType}");
    }

    private void ClearEnvironment()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        spawnedObjects.Clear();
    }

    private void SpawnObjects(GameObject prefab, int count, Vector2 scaleRange, bool randomYRotation, float yOffset = 0f)
    {
        if (prefab == null || spawnParent == null)
            return;

        for (int i = 0; i < count; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                yOffset,
                Random.Range(-spawnRange, spawnRange)
            );

            Quaternion rotation = randomYRotation
                ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)
                : Quaternion.identity;

            GameObject spawned = Instantiate(prefab, position, rotation, spawnParent);

            float scale = Random.Range(scaleRange.x, scaleRange.y);
            spawned.transform.localScale *= scale;

            spawnedObjects.Add(spawned);
        }
    }

    private void ApplyDefaultSettings()
    {
        RenderSettings.fog = false;

        if (directionalLight != null)
        {
            directionalLight.color = Color.white;
            directionalLight.intensity = 1f;
            directionalLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        if (ground != null)
        {
            Renderer renderer = ground.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    private void ApplyForestSettings(SceneSettings settings)
    {
        if (ground != null)
        {
            Renderer renderer = ground.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = new Color(0.25f, 0.55f, 0.25f);
        }

        if (directionalLight != null)
        {
            directionalLight.color = new Color(1f, 0.95f, 0.85f);
            directionalLight.intensity = 1.1f;
        }
    }

    private void ApplyCitySettings(SceneSettings settings)
    {
        if (ground != null)
        {
            Renderer renderer = ground.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = new Color(0.4f, 0.4f, 0.42f);
        }

        if (directionalLight != null)
        {
            directionalLight.color = new Color(1f, 0.98f, 0.95f);
            directionalLight.intensity = 1.2f;
        }
    }

    private void ApplyOceanSettings(SceneSettings settings)
    {
        if (ground != null)
        {
            Renderer renderer = ground.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = new Color(0.2f, 0.55f, 0.65f);
        }

        if (directionalLight != null)
        {
            directionalLight.color = new Color(0.8f, 0.95f, 1f);
            directionalLight.intensity = 1.15f;
        }
    }

   private void ApplyTimeOfDay(TimeOfDay timeOfDay)
{
    if (directionalLight == null)
        return;

    switch (timeOfDay)
    {
        case TimeOfDay.Night:

            RenderSettings.skybox = nightSkybox;

            directionalLight.color = new Color(0.35f, 0.4f, 0.7f);
            directionalLight.intensity = 0.2f;
            directionalLight.transform.rotation = Quaternion.Euler(15f, -30f, 0f);

            break;

        case TimeOfDay.Sunset:

            RenderSettings.skybox = sunsetSkybox;

            directionalLight.color = new Color(1f, 0.7f, 0.45f);
            directionalLight.intensity = 0.8f;
            directionalLight.transform.rotation = Quaternion.Euler(20f, -20f, 0f);

            break;

        case TimeOfDay.Dawn:

            RenderSettings.skybox = dawnSkybox;

            directionalLight.color = new Color(1f, 0.85f, 0.7f);
            directionalLight.intensity = 0.9f;
            directionalLight.transform.rotation = Quaternion.Euler(25f, -35f, 0f);

            break;

        case TimeOfDay.Day:
        default:

            RenderSettings.skybox = daySkybox;

            directionalLight.color = Color.white;
            directionalLight.intensity = 1f;

            break;
    }

    DynamicGI.UpdateEnvironment();
}

    private void ApplyWeather(WeatherType weatherType)
    {
        switch (weatherType)
        {
            case WeatherType.Foggy:
                RenderSettings.fog = true;
                RenderSettings.fogColor = new Color(0.7f, 0.75f, 0.8f);
                RenderSettings.fogDensity = 0.02f;
                break;

            case WeatherType.Clear:
            default:
                RenderSettings.fog = false;
                break;
        }
    }

    private void ApplyMood(MoodType moodType)
    {
        if (moodType == MoodType.Calm)
        {
            if (directionalLight != null)
            {
                directionalLight.intensity *= 0.9f;
            }

            if (RenderSettings.fog)
            {
                RenderSettings.fogDensity *= 0.8f;
            }
        }
    }
}
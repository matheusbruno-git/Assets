using UnityEngine;

[ExecuteAlways]
public class LightningManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightningPreset Preset;
    
    [SerializeField, Range(0, 2400)] public float TimeOfDay;

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime * 10f; // mudar o time speed se necessario
            TimeOfDay %= 2400;
        }

        UpdateLighting(TimeOfDay / 2400f);
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170, 0));
            
            if (TimeOfDay < 600 || TimeOfDay > 1800)
            {
                DirectionalLight.gameObject.SetActive(false);
            }
            else
            {
                DirectionalLight.gameObject.SetActive(true);
            }
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}

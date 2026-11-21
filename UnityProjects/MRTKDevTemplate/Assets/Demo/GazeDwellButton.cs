using PupilLabs;
using System;
using TMPro;
using UnityEngine;

public class GazeDwellButton : MonoBehaviour
{
    public float dwellTime = 1.5f;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;

    public Action onSelected;

    private Renderer rend;
    private float timer = 0f;
    private bool selected = false;
    private bool wasLooking = false;
    private float lastHitTime = -10f;

    public void SetLabel(string text)
    {
        TMP_Text label = GetComponentInChildren<TMP_Text>(true);
        if (label != null) label.text = text;
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        SetColor(normalColor);

        GazeDataVisualizer visualizer = FindObjectOfType<GazeDataVisualizer>(true);
        if (visualizer == null)
        {
            Debug.LogError("No GazeDataVisualizer found :(");
            return;
        }
        visualizer.onHit.AddListener(OnGazeHit);
    }

    void OnGazeHit(Transform hitTransform, Vector3 hitPoint, Vector2 uv)
    {
        if (hitTransform == transform)
        {
            lastHitTime = Time.time;
        }
    }

    void SetColor(Color c)
    {
        rend.material.SetColor("_Color", c);
        rend.material.SetColor("_BaseColor", c);
    }

    void Update()
    {
        bool looking = (Time.time - lastHitTime) < 0.2f;

        if (looking != wasLooking)
        {
            Debug.Log("Gaze hover: " + looking);
            wasLooking = looking;
        }

        if (looking)
        {
            timer += Time.deltaTime;
            if (!selected && timer >= dwellTime)
            {
                selected = true;
                Debug.Log("Button selected!");
                if (onSelected != null) onSelected();
            }

            if (selected)
            {
                SetColor(selectedColor);
            }
            else
            {
                SetColor(hoverColor);
            }
        }
        else
        {
            timer = 0f;
            selected = false;
            SetColor(normalColor);
        }
    }
}

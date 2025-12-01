using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [Header("Movement")]
    public float moveUpSpeed = 40f;
    public float randomSpread = 20f;

    [Header("Pop Animation")]
    public float popScale = 1.5f;
    public float popSpeed = 8f;

    [Header("Fade")]
    public float fadeSpeed = 2f;

    private TextMeshProUGUI text;
    private Color textColor;
    private Vector3 baseScale;
    private Vector3 floatDirection;
    private bool popped = false;

    private RectTransform rect;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();

        if (text == null)
        {
            Debug.LogError("FloatingDamageText: TextMeshProUGUI component not found!");
            return;
        }

        textColor = text.color;
        baseScale = transform.localScale;

        // Randomized movement direction for UI (x + y movement only)
        floatDirection = new Vector3(
            Random.Range(-randomSpread, randomSpread),
            Random.Range(15f, 35f), // always move up
            0f
        ).normalized;
    }

    public void SetText(string value)
    {
        if (UpgradeSystem.instance != null && UpgradeSystem.instance.IsMenuOpen) return;
        if (text != null)
            text.text = value;
    }

    void Update()
    {
        if (UpgradeSystem.instance != null && UpgradeSystem.instance.IsMenuOpen) return;
        if (text == null) return;

        // Pop animation
        if (!popped)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, baseScale * popScale, popSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, baseScale * popScale) < 0.05f)
                popped = true;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, baseScale, popSpeed * Time.deltaTime);
        }

        // Floating movement (UI uses anchoredPosition)
        rect.anchoredPosition += (Vector2)(floatDirection * moveUpSpeed * Time.deltaTime);

        // Fade out
        textColor.a -= fadeSpeed * Time.deltaTime;
        text.color = textColor;

        if (textColor.a <= 0)
            Destroy(gameObject);
    }
}




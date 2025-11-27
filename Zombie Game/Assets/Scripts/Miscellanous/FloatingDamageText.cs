using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [Header("Movement")]
    public float moveUpSpeed = 1f;
    public float randomSpread = 0.3f;

    [Header("Pop Animation")]
    public float popScale = 1.5f;
    public float popSpeed = 8f;

    [Header("Fade")]
    public float fadeSpeed = 2f;

    private TextMeshPro text;
    private Color textColor;
    private Vector3 baseScale;
    private Vector3 floatDirection;
    private bool popped = false;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        if (text == null)
        {
            Debug.LogError("FloatingDamageText: TextMeshPro component not found!");
            return;
        }

        textColor = text.color;
        baseScale = transform.localScale;

        // Random floating direction
        floatDirection = Vector3.up + new Vector3(
            Random.Range(-randomSpread, randomSpread),
            0f,
            Random.Range(-randomSpread, randomSpread)
        );
        floatDirection.Normalize();
    }

    public void SetText(string value)
    {
        if (text != null)
            text.text = value;
    }

    void Update()
    {
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

        // Floating movement
        transform.position += floatDirection * moveUpSpeed * Time.deltaTime;

        // Face camera
        if (Camera.main != null)
        {
            Vector3 lookDir = transform.position - Camera.main.transform.position;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // Fade out
        textColor.a -= fadeSpeed * Time.deltaTime;
        text.color = textColor;

        if (textColor.a <= 0)
            Destroy(gameObject);
    }
}



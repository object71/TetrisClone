using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{
    public float startAlpha = 1f;
    public float targetAlpha = 0f;

    public float delay = 0f;
    public float timeToFade = 1f;

    private float incrementBy;
    private float currentAlpha;

    private MaskableGraphic graphic;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        graphic = GetComponent<MaskableGraphic>();
        originalColor = graphic.color;

        currentAlpha = startAlpha;
        Color startColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
        graphic.color = startColor;

        incrementBy = ((targetAlpha - startAlpha) / timeToFade) * Time.deltaTime;

        StartCoroutine("FadeRoutine");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(delay);

        while (Mathf.Abs(targetAlpha - currentAlpha) > 0.01f)
        {
            yield return new WaitForEndOfFrame();

            currentAlpha = currentAlpha + incrementBy;
            Color currentColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
            graphic.color = currentColor;
        }
    }
}

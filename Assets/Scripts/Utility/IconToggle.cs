using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{
    public Sprite iconTrue;
    public Sprite iconFalse;

    public bool defaultIconState = true;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = defaultIconState ? iconTrue : iconFalse;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleIcon(bool state)
    {
        if (!image || !iconTrue || !iconFalse)
        {
            return;
        }

        image.sprite = state ? iconTrue : iconFalse;
    }
}

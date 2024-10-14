using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class ImageNumberTools : MonoBehaviour
{
    public Sprite[] sprites;
    public Image mainImage;
    // Start is called before the first frame update
    void Start()
    {
        //mainImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNumber(int i)
    {
        if (mainImage != null)
        {
            mainImage.sprite = sprites[i];
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ChangePlayerSprite : MonoBehaviour
{
    public List<Sprite> spritesToChange;
    public List<Sprite> spriteTemplates;

    public GameObject sliderHGameObj;
    public GameObject sliderSGameObj;
    public GameObject sliderVGameObj;

    private Slider sliderH;
    private Slider sliderS;
    private Slider sliderV;

    public Sprite playerSprite;
    public Sprite playerTemplateSprite;
    private Image playerImage;
    private Texture2D playerTexture;
    private Texture2D playerTemplateTexture;
    private Texture2D tempTexture;
    private Color[] pixels;
    private Color[] templatePixels;
    private Color[] pixelsAfterChange;
    // private float HSave;
    private float originalH = 0.06372548f;
    // private float originalV = 0.2588235f;

    private float globalH;
    private float globalS;
    private float globalV;

    // private void Awake()
    // {
    //     PlayerControls = new PlayerInput();
    // }
    // 
    // private void OnEnable()
    // {
    //     shoot = PlayerControls.Player.Attack;
    //     shoot.Enable();
    //     shoot.performed += ChangeColour;
    // }
    // 
    // private void OnDisable()
    // {
    //     shoot.Disable();
    // }

    private void Start()
    {
        sliderH = sliderHGameObj.GetComponent<Slider>();
        sliderS = sliderSGameObj.GetComponent<Slider>();
        sliderV = sliderVGameObj.GetComponent<Slider>();
        playerImage = gameObject.transform.Find("Image").GetComponent<Image>();
        playerSprite = playerImage.sprite;
        playerTexture = playerSprite.texture;
        tempTexture = new Texture2D(playerTexture.width, playerTexture.height);
        tempTexture.filterMode = FilterMode.Point;
        playerTemplateTexture = playerTemplateSprite.texture;
        templatePixels = playerTemplateTexture.GetPixels();
        pixels = playerTexture.GetPixels();
        pixelsAfterChange = playerTexture.GetPixels();

        globalH = sliderH.value;
        globalV = sliderV.value;

        ChangeColor();
    }

    public void SetH(float H)
    {
        globalH = H;
    }

    public void SetS(float S)
    {
        globalS = S;
    }

    public void SetV(float V)
    {
        globalV = V;
    }

    public void ChangeColor()
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a != 0)
            {
                Color.RGBToHSV(pixels[i], out float H, out float S, out float V);
                if (V != 0 && S != 0)
                {
                    // H = (H + HDiff) % 1.0f;
                    Color.RGBToHSV(templatePixels[i], out float templateH, out float templateS, out float templateV);
                    float newS = templateS + globalS;
                    float newV = templateV + globalV;
                    pixelsAfterChange[i] = Color.HSVToRGB(globalH, newS, newV);
                }
            }
        }

        tempTexture.SetPixels(pixelsAfterChange);
        tempTexture.Apply();
        Sprite newSprite = Sprite.Create(tempTexture, playerSprite.rect, playerSprite.pivot, playerSprite.pixelsPerUnit);
        playerImage.sprite = newSprite;
    }

    public void ChangeH(float H)
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a != 0)
            {
                Color.RGBToHSV(pixels[i], out float tempH, out float S, out float V);
                if (V != 0 && S != 0)
                {
                    // H = (H + HDiff) % 1.0f;
                    pixelsAfterChange[i] = Color.HSVToRGB(H, S, V);
                }
            }
        }

        tempTexture.SetPixels(pixelsAfterChange);
        tempTexture.Apply();
        Sprite newSprite = Sprite.Create(tempTexture, playerSprite.rect, playerSprite.pivot, playerSprite.pixelsPerUnit);
        playerImage.sprite = newSprite;
    }

    public void ChangeV(float VChange)
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a != 0)
            {
                Color.RGBToHSV(pixels[i], out float H, out float S, out float V);
                if (V != 0 && S != 0)
                {
                    V += VChange;
                    H = sliderH.value;
                    // H = (H + HDiff) % 1.0f;
                    pixelsAfterChange[i] = Color.HSVToRGB(H, S, V);
                }
            }
        }

        tempTexture.SetPixels(pixelsAfterChange);
        tempTexture.Apply();
        Sprite newSprite = Sprite.Create(tempTexture, playerSprite.rect, playerSprite.pivot, playerSprite.pixelsPerUnit);
        playerImage.sprite = newSprite;
    }

    public void SetColour()
    {
        Color[] tempPixels;
        Color[] temptemplatePixels;
        Texture2D t;
        for (int index = 0; index < spritesToChange.Count; index++)
        {
            t = spritesToChange[index].texture;
            tempPixels = t.GetPixels();
            temptemplatePixels = spriteTemplates[index].texture.GetPixels();
            for (int i = 0; i < tempPixels.Length; i++)
            {
                if (tempPixels[i].a != 0)
                {
                    Color.RGBToHSV(tempPixels[i], out float H, out float S, out float V);
                    if (V != 0 && S != 0)
                    {
                        Color.RGBToHSV(temptemplatePixels[i], out float templateH, out float templateS, out float templateV);
                        float newS = templateS + globalS;
                        float newV = templateV + globalV;
                        tempPixels[i] = Color.HSVToRGB(sliderH.value, newS, newV);
                    }
                    // tempPixels[i] = Color.HSVToRGB((originalH + HDiffSave) % 1.0f, S, V);
                }
            }
            t.SetPixels(tempPixels);
            t.Apply();
        }
    }

    public void RevertBack()
    {
        sliderH.value = originalH;
        sliderV.value = 0;
        sliderS.value = 0;
        globalH = originalH;
        globalV = 0;
        globalS = 0;
        ChangeColor();
        // ChangeV();
    }
}
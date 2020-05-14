using UnityEngine;
using UnityEngine.UI;

public class PlacePanel : BasePanel
{
    private Image placeImage;
    private string placeType;
    public Sprite[] sprites;

    public void SetPlaceType(string p)
    {
        placeType = p;
        switch (placeType)
        {
            case PlaceType.Park:
                placeImage.sprite = sprites[0];
                break;

            case PlaceType.Garden:
                placeImage.sprite = sprites[1];
                break;
        }
    }

    public override void Init()
    {
        base.Init();
        placeImage = transform.Find("PlaceImage").GetComponent<Image>();
    }
}
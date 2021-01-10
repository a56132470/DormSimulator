using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class PlacePanel : BasePanel
    {
        private Image m_PlaceImage;
        private string m_PlaceType;
        public Sprite[] sprites;

        public void SetPlaceType(string p)
        {
            m_PlaceType = p;
            switch (m_PlaceType)
            {
                case PlaceType.Classroom:
                    m_PlaceImage.sprite = sprites[0];
                    break;

                case PlaceType.Dorm:
                    m_PlaceImage.sprite = sprites[1];
                    break;
                case PlaceType.Playground:
                    m_PlaceImage.sprite = sprites[2];
                    break;
            }
        }

        public override void Init()
        {
            base.Init();
            m_PlaceImage = transform.Find("PlaceImage").GetComponent<Image>();
        }
    }
}
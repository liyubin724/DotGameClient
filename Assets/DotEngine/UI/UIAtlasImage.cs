using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [AddComponentMenu("DotEngine/UI/Atlas Image", 10)]
    public class UIAtlasImage : Image
    {
        [SerializeField]
        private SpriteAtlas m_atlas;
        public SpriteAtlas Atlas
        {
            get
            {
                return m_atlas;
            }
            set
            {
                if (value != null && value != m_atlas)
                {
                    m_atlas = value;
                    SpriteName = "";
                }
            }
        }

        [SerializeField]
        private string m_SpriteName = "";
        public string SpriteName
        {
            get
            {
                return m_SpriteName;
            }
            set
            {
                if (m_atlas != null)
                {
                    if (m_SpriteName != value)
                    {
                        m_SpriteName = value;
                        if (string.IsNullOrEmpty(m_SpriteName))
                        {
                            sprite = null;
                        }
                        else
                        {
                            sprite = m_atlas.GetSprite(m_SpriteName);
                        }
                    }
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (m_atlas != null)
            {
                if (string.IsNullOrEmpty(m_SpriteName))
                {
                    sprite = null;
                }
                else
                {
                    sprite = m_atlas.GetSprite(m_SpriteName);
                }
            }
        }
    }

}


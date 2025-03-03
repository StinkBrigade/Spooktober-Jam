﻿using UnityEngine;
namespace Spooktober.Character.People
{
    public class PersonPart : MonoBehaviour
    {
        private PersonMaker m_personMaker;

        private SpriteRenderer m_spriteRenderer;
        
        [SerializeField] private BodyPart m_bodyPart;
        
        private void Awake()
        {
            Generate();
        }

        void Generate()
        {
            m_personMaker = FindObjectOfType<PersonMaker>();

            m_spriteRenderer = GetComponent<SpriteRenderer>();

            m_spriteRenderer.sprite = m_personMaker.GetBodySprite(m_bodyPart);
        }
    }
}

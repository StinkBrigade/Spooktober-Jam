﻿namespace Spooktober.Dialogue.Types
{
    public class Dialogue
    {
        private string m_type;
        
        private string m_text;

        public string Type => m_type;
        
        public string Text => m_text;

        public Dialogue(string _type, string _text)
        {
            m_type = _type;
            m_text = _text;
        }
    }
}

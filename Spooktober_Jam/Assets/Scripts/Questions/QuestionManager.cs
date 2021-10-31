using System;
using System.Collections;
using System.Collections.Generic;
using Spooktober.Character;
using Spooktober.Dialogue;
using Spooktober.Dialogue.Types;
using Spooktober.UI;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Spooktober
{
    public class QuestionManager : MonoBehaviour
    {
        private class QuestionHolder
        {
            public Dialogue.Types.Dialogue m_dialogue;
            public Stat m_stat;

            public QuestionHolder(Stat _stat, Dialogue.Types.Dialogue _dialogue)
            {
                m_stat = _stat;
                m_dialogue = _dialogue;
            }
        }
        
        private DialogueManager m_dialogueManager;
        private AnswerManager m_answerManager;
        
        [SerializeField] private int m_maxQuestions;
        [SerializeField] private int m_offerCount;
        
        [SerializeField] private float m_offeredStatMultiplier;
        [SerializeField] private float m_chosenStatMultiplier;

        [SerializeField] private int m_maxRerolls;

        private QuestionHolder[] m_currentQuestions;
        
        private float[] m_statWeights;
        
        private int m_count;

        public int RemainingQuestions => m_maxQuestions - m_count;

        private void Awake()
        {
            m_dialogueManager = FindObjectOfType<DialogueManager>();
            m_answerManager = FindObjectOfType<AnswerManager>();
            
            m_statWeights = new float[Enum.GetNames(typeof(Stat)).Length];
            for (var i = 0; i < m_statWeights.Length; i++)
            {
                m_statWeights[i] = 1.0f;
            }
        }

        private void Start()
        {
            OfferQuestions();
        }

        public void OfferQuestions()
        {
            if (m_count >= m_maxQuestions) return;
            
            var stats = new List<Stat>();
            for (var i = 0; i < Enum.GetNames(typeof(Stat)).Length; i++)
            {
                stats.Add((Stat) i);
            }
            
            for (var i = 0; i < m_offerCount; i++)
            {
                RollStat(stats);
            }
            AdjustOfferedWeights(stats);

            m_currentQuestions = new QuestionHolder[m_offerCount];
            for (var i = 0; i < m_offerCount; i++)
            {
                m_currentQuestions[i] = new QuestionHolder(stats[i], m_dialogueManager.GetRandomQuestion(stats[i]));
            }
        }

        public UnityEvent m_clickQuestionEvent;
        public UnityEvent m_countFullEvent;

        public void ClickQuestions()
        {
            if (m_count >= m_maxQuestions) return;
            m_clickQuestionEvent?.Invoke();
        }

        public void PickQuestion(int _id)
        {
            m_count++;
            if (m_count >= m_maxQuestions)
            {
                m_countFullEvent?.Invoke();
            }
            
            m_answerManager.ShowAnswers(m_currentQuestions[_id].m_stat, m_currentQuestions[_id].m_dialogue);
        }

        public Dialogue.Types.Dialogue GetQuestion(int _index)
            => m_currentQuestions[_index].m_dialogue;

        private void RollStat(IList<Stat> _stats)
        {
            for (var i = 0; i < m_maxRerolls; i++)
            {
                var statIndex = Random.Range(0, _stats.Count);
                
                var statWeight = m_statWeights[(int)_stats[statIndex]];
                if (i < m_maxRerolls - 1 && (statWeight >= 1.0f || Random.Range(0.0f, 1.0f) > statWeight)) continue;
                
                _stats.RemoveAt(statIndex);
                break;
            }
        }

        private void AdjustOfferedWeights(IList<Stat> _stats)
        {
            foreach (var stat in _stats)
            {
                m_statWeights[(int)stat] *= m_offeredStatMultiplier;
            }
        }
    }
}

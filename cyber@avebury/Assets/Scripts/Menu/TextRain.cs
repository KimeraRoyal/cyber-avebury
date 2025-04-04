using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextRain : MonoBehaviour
    {
        [Serializable]
        private class RainColumn
        {
            private TextRain m_rain;

            [SerializeField] private int m_minColumn;
            [SerializeField] private int m_maxColumn;
            [SerializeField] private int m_currentRow;
            [SerializeField] private int m_currentColumn;
            
            private float m_movementInterval;
            private float m_movementTimer;

            public int MinColumn
            {
                get => m_minColumn;
                set => m_minColumn = value;
            }

            public int MaxColumn
            {
                get => m_maxColumn;
                set => m_maxColumn = value;
            }

            public int CurrentRow => m_currentRow;
            public int CurrentColumn => m_currentColumn;

            public RainColumn(TextRain _rain)
            {
                m_rain = _rain;
                
                Reset();
                m_currentRow = Random.Range(0, m_rain.m_rowCount);
            }
            
            public void Move()
            {
                m_movementTimer += Time.deltaTime;
                if(m_movementTimer < m_movementInterval) { return; }
                m_movementTimer -= m_movementInterval;

                m_currentRow++;
                if (m_currentRow >= m_rain.m_characters.Length)
                {
                    Reset();
                }
                m_rain.m_dirty = true;
            }

            public int CalculateDistance(int _row)
                => m_currentRow - _row;

            public void Reset()
            {
                m_currentColumn = Random.Range(m_minColumn, m_maxColumn + 1);
                m_currentRow = 0;
            
                m_movementInterval = Random.Range(m_rain.m_minMovementInterval, m_rain.m_maxMovementInterval);
            }
        }
        
        private TMP_Text m_text;

        [SerializeField] private RainColumn[] m_columns;

        [SerializeField] private int m_columnCount = 123;
        [SerializeField] private int m_rowCount = 54;

        [SerializeField] private int m_rainCount = 8;
        
        [SerializeField] private byte m_lifetime = 5;
        
        [SerializeField] private float m_minMovementInterval = 0.1f;
        [SerializeField] private float m_maxMovementInterval = 0.1f;
        
        [SerializeField] private float m_randomizationInterval = 1.0f;
        private float m_randomizationTimer;

        private string m_display;
        private char[] m_characters;
        private string[] m_fadeHex;
        private bool m_dirty;

        private void Awake()
        {
            m_text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            GenerateCharacters();
            GenerateFades();
            CreateColumns();
            DrawText();
        }

        private void Update()
        {
            //Randomize();
            ScrollText();
            DrawText();
        }

        private void DrawText()
        {
            if(!m_dirty) { return; }

            m_display = "";
            for(var y = 0; y < m_characters.Length; y++)
            {
                for (var x = 0; x < m_columnCount; x++)
                {
                    var distance = -1;
                    var hasColumn = false;
                    
                    foreach (var column in m_columns)
                    {
                        if(x != column.CurrentColumn) { continue; }
                        distance = column.CalculateDistance(y);
                        hasColumn = true;
                    }

                    if (hasColumn && distance >= 0 && distance < m_lifetime)
                    {
                        m_display += "<alpha=#" + m_fadeHex[distance] + ">" + m_characters[y];
                    }
                    else
                    {
                        m_display += ' ';
                    }
                }

                m_display += '\n';
            }

            m_text.text = m_display;
            m_dirty = false;
        }

        private void ScrollText()
        {
            foreach (var column in m_columns)
            {
                column.Move();
            }
        }

        private void Randomize()
        {
            m_randomizationTimer += Time.deltaTime;
            if(m_randomizationTimer < m_randomizationInterval) { return; }
            m_randomizationTimer -= m_randomizationInterval;
            
            GenerateCharacters();
            m_dirty = true;
        }

        private void GenerateCharacters()
        {
            if (m_characters == null || m_characters.Length != m_rowCount + m_lifetime)
            {
                m_characters = new char[m_rowCount + m_lifetime];
            }
            
            for (var y = 0; y < m_characters.Length; y++)
            {
                m_characters[y] = Random.Range(0, 2) > 0 ? '1' : '0';
            }
        }

        private void GenerateFades()
        {
            if(m_fadeHex != null && m_fadeHex.Length == m_lifetime) { return; }

            m_fadeHex = new string[m_lifetime];
            var fadeByte = new byte[1];
            
            for (var i = 0; i < m_fadeHex.Length; i++)
            {
                var t = 1.0f - i / (float)m_lifetime;
                fadeByte[0] = (byte)(t * byte.MaxValue);
                m_fadeHex[i] = BitConverter.ToString(fadeByte);
            }
        }

        private void CreateColumns()
        {
            m_columns = new RainColumn[m_rainCount];
            
            var increment = m_columnCount / (float) m_rainCount;
            for (var i = 0; i < m_rainCount; i++)
            {
                m_columns[i] = new RainColumn(this)
                {
                    MinColumn = (int)(i * increment),
                    MaxColumn = (int)((i + 1) * increment - 1)
                };
            }

            m_dirty = true;
        }
    }
}

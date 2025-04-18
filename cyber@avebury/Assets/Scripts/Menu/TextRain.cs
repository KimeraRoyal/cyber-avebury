using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
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

            public int CurrentRow
            {
                get => m_currentRow;
                set => m_currentRow = value;
            }
            
            public int CurrentColumn
            {
                get => m_currentColumn;
                set => m_currentColumn = value;
            }

            public RainColumn(TextRain _rain)
            {
                m_rain = _rain;
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

        private Tilemap m_tilemap;
        
        [SerializeField] private RainColumn[] m_columns;

        [SerializeField] private int m_columnCount = 123;
        [SerializeField] private int m_rowCount = 54;
        [SerializeField] private Vector3Int m_offset;
        
        [SerializeField] private int m_rainCount = 8;
        
        [SerializeField] private Tile[] m_tilePalette;
        [SerializeField] private byte m_lifetime = 5;

        [SerializeField] private bool m_moveDown = true;
        [SerializeField] private float m_minMovementInterval = 0.1f;
        [SerializeField] private float m_maxMovementInterval = 0.1f;
        
        [SerializeField] private float m_minRandomizationInterval = 1.0f;
        [SerializeField] private float m_maxRandomizationInterval = 1.0f;
        private float m_randomizationInterval = -1.0f;
        private float m_randomizationTimer;

        private char[] m_characters;
        private bool m_dirty;

        private void Awake()
        {
            m_tilemap = GetComponent<Tilemap>();
        }

        private void Start()
        {
            Randomize();
            CreateColumns();
            DrawText();
        }

        private void Update()
        {
            Randomize();
            ScrollText();
            DrawText();
        }

        private void DrawText()
        {
            if(!m_dirty) { return; }

            foreach (var columnRain in m_columns)
            {
                for (var distance = 0; distance <= m_lifetime; distance++)
                {
                    var x = columnRain.CurrentColumn;
                    var y = columnRain.CurrentRow - distance;
                    if (!m_moveDown) { y = m_characters.Length - y - 1; }

                    if (y < 0) { y += m_characters.Length; }
                    if (y >= m_characters.Length) { y -= m_characters.Length; }
                    
                    var character = m_characters[y];
                    var tileIndex = distance * 2 + character;
                    m_tilemap.SetTile(new Vector3Int(x, m_rowCount - y, 0) - m_offset, distance < m_lifetime ? m_tilePalette[tileIndex] : null);
                }
            }
            m_dirty = false;
        }

        private void Randomize()
        {
            m_randomizationTimer += Time.deltaTime;
            if(m_randomizationTimer < m_randomizationInterval) { return; }
            m_randomizationTimer -= m_randomizationInterval;

            GenerateCharacters();
            m_randomizationInterval = Random.Range(m_minRandomizationInterval, m_maxRandomizationInterval);
            m_dirty = true;
        }

        private void ScrollText()
        {
            foreach (var column in m_columns)
            {
                column.Move();
            }
        }

        private void GenerateCharacters()
        {
            if (m_characters == null || m_characters.Length != m_rowCount + m_lifetime)
            {
                m_characters = new char[m_rowCount + m_lifetime];
            }
            
            for (var y = 0; y < m_characters.Length; y++)
            {
                m_characters[y] = (char) Random.Range(0, 2);
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
                m_columns[i].Reset();
                m_columns[i].CurrentRow = Random.Range(0, m_rowCount);
            }

            m_dirty = true;
        }
    }
}

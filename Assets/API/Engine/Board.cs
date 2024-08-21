using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
namespace Engine
{
    public class Board
    {
        public List<Card>[] rows = new List<Card>[3];
        public List<Card> cemetery = new();
        public Board()
        {
            rows = new List<Card>[3];  // Asume 3 filas, ajusta según sea necesario
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new List<Card>();
            }
        }
        public void ResetEngineBoard()
        {
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new List<Card>();
            }
        }
    }  
}
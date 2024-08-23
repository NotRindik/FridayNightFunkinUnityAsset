using History;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING 
{
    public class HistoryTesting : MonoBehaviour
    {
        public HistoryState state = new HistoryState();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                state = HistoryState.Capture();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                state.Load();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BroccoliBunnyStudios
{
    public class Persistant : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}

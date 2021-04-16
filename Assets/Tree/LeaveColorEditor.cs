using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeScripts.TreeGenerator;
using TreeScripts;

namespace TreeScripts
{
    public class LeaveColorEditor : MonoBehaviour
    {
        [SerializeField, PersistentAmongPlayMode] Color NewBie = new Color(0 / 256f, 171 / 256f, 214 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color PPC = new Color(0 / 256f, 216 / 256f, 133 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color Web = new Color(137 / 256f, 91 / 256f, 0 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color Crypto = new Color(173 / 256f, 166 / 256f, 145 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color Binary = new Color(177 / 256f, 249 / 256f, 114 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color Pwn = new Color(150 / 256f, 200 / 256f, 255 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color Misc = new Color(219 / 256f, 43 / 256f, 0 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color Shell = new Color(198 / 256f, 198 / 256f, 198 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color Foresic = new Color(125 / 256f, 0 / 256f, 188 / 256f);
        [SerializeField, PersistentAmongPlayMode] Color OSINT = new Color(0 / 256f, 38 / 256f, 255 / 256f);

        public void Start() {
            SetColor();
        }

        public void SetColor() {
            GENRE_TO_COLOR[0] = NewBie;
            GENRE_TO_COLOR[1] = PPC;
            GENRE_TO_COLOR[2] = Web;
            GENRE_TO_COLOR[3] = Crypto;
            GENRE_TO_COLOR[4] = Binary;
            GENRE_TO_COLOR[5] = Pwn;
            GENRE_TO_COLOR[6] = Misc;
            GENRE_TO_COLOR[7] = Shell;
            GENRE_TO_COLOR[8] = Foresic;
            GENRE_TO_COLOR[9] = OSINT;
        }
    }
}


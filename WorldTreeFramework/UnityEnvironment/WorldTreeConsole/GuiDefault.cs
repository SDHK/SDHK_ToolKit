using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WorldTree
{
    public static class GUIDefault
    {
        private static string textFoldoutOn = "▼";
        private static string textFoldoutOff = "▶";
        private static string textClose = "X";

        private static GUIStyle styleBlack;
        private static GUIStyle styleBlack1;
        private static GUIStyle styleBlack2;
        private static GUIStyle styleBlack3;
        private static GUIStyle styleBlack4;
        private static GUIStyle styleRed;
        private static GUIStyle styleBlue;
        private static GUIStyle styleTransparent;
        private static GUIStyle styleLine;

        public static GUILayoutOption optionWidth2 = GUILayout.Width(2);
        public static GUILayoutOption optionWidth25 = GUILayout.Width(25);
        public static GUILayoutOption optionWidth40 = GUILayout.Width(40);
        public static GUILayoutOption optionWidth60 = GUILayout.Width(60);
        public static GUILayoutOption optionWidth80 = GUILayout.Width(120);
        public static GUILayoutOption optionExpandWidth = GUILayout.ExpandWidth(false);

        public static GUILayoutOption optionHeight2 = GUILayout.Height(2);
        public static GUILayoutOption optionHeight25 = GUILayout.Height(25);
        public static GUIStyle StyleBlack => styleBlack ?? (styleBlack = ColorGUIStyle(Color.black));
        public static GUIStyle StyleBlack1 => styleBlack1 ?? (styleBlack1 = ColorGUIStyle(new Color(0.1f, 0.1f, 0.1f)));
        public static GUIStyle StyleBlack2 => styleBlack2 ?? (styleBlack2 = ColorGUIStyle(new Color(0.2f, 0.2f, 0.2f)));
        public static GUIStyle StyleBlack3 => styleBlack3 ?? (styleBlack3 = ColorGUIStyle(new Color(0.3f, 0.3f, 0.3f)));
        public static GUIStyle StyleBlack4 => styleBlack4 ?? (styleBlack4 = ColorGUIStyle(new Color(0.4f, 0.4f, 0.4f)));
        public static GUIStyle StyleRed => styleRed ?? (styleRed = ColorGUIStyle(new Color(0.5f, 0, 0), TextAnchor.MiddleCenter));
        public static GUIStyle StyleBlue => styleBlue ?? (styleBlue = ColorGUIStyle(new Color(0.2f, 0.3f, 0.5f)));
        public static GUIStyle StyleTransparent => styleTransparent ?? (styleTransparent = ColorGUIStyle(new Color(0, 0, 0, 0)));
        public static GUIStyle StyleLine => styleLine ?? (styleLine = ColorGUIStyle(new Color(0.1f, 0.1f, 0.1f), name: "Label"));

        //new Color(0, 1, 1)
        private static GUIStyle ColorGUIStyle(Color color, TextAnchor textAnchor = TextAnchor.MiddleLeft, string name = "Box")
        {
            GUIStyle style = new GUIStyle(name);
            style.normal.background = new Texture2D(1, 1, Texture2D.grayTexture.format, true);
            style.normal.background.SetPixel(0, 0, color);
            style.alignment = textAnchor;
            style.fontSize = 16;
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;
            style.border = new RectOffset(1, 1, 1, 1);
            //style.padding = new RectOffset(1, 1, 1, 1);

            style.normal.background.Apply();
            return style;
        }


        public static void Button(string text, Action action, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(text, StyleTransparent, options))
            {
                action();
            }
        }


        public static void CloseButton(Action action)
        {
            if (GUILayout.Button(textClose, StyleRed, optionWidth25, optionHeight25))
            {
                action();
            }
        }

        public static void FoldoutButton(bool foldout, Action<bool> action)
        {
            if (GUILayout.Button(foldout ? textFoldoutOn : textFoldoutOff, StyleTransparent, optionWidth25, optionHeight25))
            {
                action(foldout);
            }
        }

        public static void LineHorizontal()
        {
            GUILayout.Box(default(string), StyleLine, optionHeight2, GUILayout.ExpandWidth(true));
        }

        public static void LineVertical()
        {
            GUILayout.Box(default(string), StyleLine, optionWidth2, GUILayout.ExpandHeight(true));
        }


    }
}

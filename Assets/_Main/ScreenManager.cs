using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public static class ScreenManager
    {
        private static List<ScreenBehaviour> screenBehaviours = new List<ScreenBehaviour>();
        public static ScreenBehaviour currentScreen;

        public static void AssignScreen(ScreenBehaviour screenBehaviour)
        {
            if (!screenBehaviours.Contains(screenBehaviour))
            {
                screenBehaviours.Add(screenBehaviour);
                screenBehaviour.OnDeactivate();
            }                
        }

        public static void UnassignScreen(ScreenBehaviour screenBehaviour)
        {
            if (screenBehaviours.Contains(screenBehaviour))
                screenBehaviours.Remove(screenBehaviour);
        }

        public static void LoadScreen(Type screenType)
        {
            var screen = screenBehaviours.Find((screen) => screen.GetType() == screenType);

            if (screen)
            {
                // Disable previous screen
                currentScreen?.OnDeactivate();

                // Enable new screen
                screen.OnActivate();
                currentScreen = screen;
            }
        }
    }
}
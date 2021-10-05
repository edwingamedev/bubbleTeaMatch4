using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public static class ScreenManager
    {
        public static ScreenBehaviour currentScreen;

        private static List<ScreenBehaviour> screenBehaviours = new List<ScreenBehaviour>();
        private static Stack<Type> loadedScreens = new Stack<Type>();

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
            {
                screenBehaviours.Remove(screenBehaviour);
            }
        }

        public static void LoadPreviousScreen()
        {
            // Pop previous scene from stack           
            var screen = GetScreenByType(loadedScreens.Pop());

            if (screen)
            {
                Load(screen);
            }
        }

        public static void LoadScreen(Type screenType)
        {
            var screen = GetScreenByType(screenType);

            if (screen)
            {
                // Add loaded screen to stack                
                if (currentScreen != null)
                    loadedScreens.Push(currentScreen.GetType());

                Load(screen);
            }
        }

        private static void Load(ScreenBehaviour screen)
        {
            // Disable previous screen
            currentScreen?.OnDeactivate();

            // Sets new screen
            currentScreen = screen;

            // Enable new screen
            currentScreen?.OnActivate();
        }

        private static ScreenBehaviour GetScreenByType(Type screenType)
        {
            return screenBehaviours.Find((screen) => screen.GetType() == screenType);
        }
    }
}
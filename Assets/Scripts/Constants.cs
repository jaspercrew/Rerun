using UnityEngine;

public static class Constants
{
        public const int NumWorlds = 7;
        public const int LevelsPerWorld = 4;
        
        public const float DisabledOpacity = 0.2f;
        public const float EnabledOpacity = 1.0f;

        public const float BreakingTime = 1f;

        public const float RewindTime = 1.3f;

        public static readonly Color BreakableColor = new Color(1, 0, 0);
        public static readonly Color LinkedColor = new Color(0, 1, 0);
        public static readonly Color DegenColor = new Color(1, 0.391f, 0);
        public static readonly Color RegenColor = new Color(1, 1, 0);
        public static readonly Color GravityColor = new Color(0.651f, 0, 1);
        public static readonly Color ButtonedColor = new Color(0, 1, 1);
        public static readonly Color BoostColor = new Color(1, .37f, 0);
}
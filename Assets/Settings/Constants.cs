using UnityEngine;

namespace Settings
{
    public static class Constants
    {
        #region Game
    
        public const string GameAssetPath = "Game/";

        #endregion
    
        #region Ui
    
        public const string UiAssetPath = "UI/";
    
        #endregion

        #region Shader property

        public static readonly int ColorProperty = Shader.PropertyToID("_Color");

        #endregion
    }
}

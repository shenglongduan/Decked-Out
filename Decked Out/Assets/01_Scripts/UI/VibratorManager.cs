using UnityEngine;

public class VibratorManager : MonoBehaviour
{
    public static void Vibrate(long milliseconds)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                var vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                vibrator.Call("vibrate", milliseconds);
            }
        }
    }
    public static void VibratePattern(long[] pattern, int repeat)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                var vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                // Check for Android version
                int sdkInt = new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
                if (sdkInt >= 26)
                {
                    using (var vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect"))
                    {
                        var effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                        vibrator.Call("vibrate", effect);
                    }
                }
                else
                {
                    // Fallback for older versions
                    vibrator.Call("vibrate", pattern, repeat);
                }
            }
        }
    }
}

using UnityEngine;
 
public static class VibratorWrapper
{
#if UNITY_ANDROID
    static AndroidJavaObject vibrator = null;
    // static AndroidJavaClass vibrationEffectClass = null;
    // static AndroidJavaObject vibrationEffect = null;
#endif
    static VibratorWrapper()
    {
#if UNITY_ANDROID
        var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var unityPlayerActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = unityPlayerActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        // vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
#endif
    }
 
     public static bool HasVibrator()
    {
#if UNITY_ANDROID
        return vibrator.Call<bool>("hasVibrator");
#else
        return false;
#endif
    }
 
    public static void Cancel()
    {
#if UNITY_ANDROID
        if (HasVibrator()) vibrator.Call("cancel");
#endif
    }
 
    public static void Vibrate(float time)
    {
#if UNITY_ANDROID
        Vibrate(FloatToLongTime(time));
#endif
    }
 
    public static void Vibrate(float[] pattern, int repeate = -1)
    {
#if UNITY_ANDROID
        long[] longPattern = new long[pattern.Length];
        for (int x = 0; x < longPattern.Length; x += 1)
        {
            longPattern[x] = FloatToLongTime(pattern[x]);
        }
        Vibrate(longPattern, repeate);
#endif
    }
 
    public static void Vibrate(long[] pattern, int repeate = -1)
    {
#if UNITY_ANDROID
        if (HasVibrator()) vibrator.Call("vibrate", pattern, repeate);
#endif
    }
 
    public static void Vibrate(long time)
    {
#if UNITY_ANDROID
        if (HasVibrator()) vibrator.Call("vibrate", time);
#endif
    }

//     public static void Vibrate(float time, int amplitude)
//     {
// #if UNITY_ANDROID
//         vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", FloatToLongTime(time), amplitude);
        
//         if (HasVibrator()) vibrator.Call("vibrate", vibrationEffect.GetRawObject());
// #endif
//     }
 
    static long FloatToLongTime(float time)
    {
        time *= 800f;
        return (long)time;
    }
}

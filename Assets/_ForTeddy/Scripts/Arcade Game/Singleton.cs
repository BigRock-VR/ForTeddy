using UnityEngine;
// Singleton design pattern, inherit from this abstrat class
// How To: MyClass : Singleton<MyClass>
public abstract class Singleton<T> : MonoBehaviour where T : Component
{

    private bool isQuitting = false;
    private static T instance;
    public static T Instance
    {
        get
        {
            Init();
            return instance;
        }
    }

    // Instantiate the singleton gameObject
    protected static void Init()
    {
        if (instance == null || instance.Equals(null))
        {
            instance = new GameObject().AddComponent<T>();
            instance.name = typeof(T).Name;
            DontDestroyOnLoad(instance);
        }
    }



    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    // Re-Load the GameObject if is deleted and the game is not closing
    private void OnDestroy()
    {
        if (!isQuitting)
        {
            instance = null;
            Init();
        }
    }
}

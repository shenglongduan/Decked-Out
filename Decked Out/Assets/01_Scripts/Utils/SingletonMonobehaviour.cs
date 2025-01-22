using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    //Private
    private static T instance;

    //Protected
    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
        else
            Destroy(this.gameObject);
    }

    //Public
    public static T Instance
    {
        get { return instance; }
    }
}

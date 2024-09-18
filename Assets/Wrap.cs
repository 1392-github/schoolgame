[System.Serializable]
public class Wrap<T>
{
    public T value;
    public static implicit operator T(Wrap<T> v) => v.value;
    public static implicit operator Wrap<T>(T v) => new Wrap<T> { value = v };
}

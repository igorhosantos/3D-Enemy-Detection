public interface IObserver<T> {
    void OnNotify(T item);
}

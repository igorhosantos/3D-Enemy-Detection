using System.Collections.Generic;
using UnityEngine;

public abstract class ObservableMonoBehaviour<T>: MonoBehaviour, IObservable<T> {
    
    private readonly List<IObserver<T>> observers = new List<IObserver<T>>();
    
    public void Register(IObserver<T> observer) {
        observers.Add(observer);
    }

    public void Unregister(IObserver<T> observer) {
        observers.Remove(observer);
    }
  
    public void NotifyAllObservers(T item) {
        observers.ForEach(o=>o.OnNotify(item));
    }
  
}

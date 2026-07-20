
public class Interactor<T> {
    protected T output;
    public void ReceiveOutput(T outPut) {
        this.output = outPut;
    }
}

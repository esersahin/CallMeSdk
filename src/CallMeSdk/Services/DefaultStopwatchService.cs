namespace CallMeSdk.Services;

public interface IStopwatchService
{
    void Start();
    void Stop();
    long ElapsedMilliseconds { get; }
    void Reset();
}

public sealed class DefaultStopwatchService : IStopwatchService
{
    private readonly Stopwatch _stopwatch = new();
    public void Start() => _stopwatch.Start();
    public void Stop() => _stopwatch.Stop();
    public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
    public void Reset() => _stopwatch.Reset();
}

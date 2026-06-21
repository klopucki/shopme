using Microsoft.AspNetCore.Http;

namespace Tests;

internal sealed class FakeSession : ISession
{
    private readonly Dictionary<string, byte[]> _values = [];

    public IEnumerable<string> Keys => _values.Keys;

    public string Id { get; } = Guid.NewGuid().ToString();

    public bool IsAvailable => true;

    public void Clear()
    {
        _values.Clear();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task LoadAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public void Remove(string key)
    {
        _values.Remove(key);
    }

    public void Set(string key, byte[] value)
    {
        _values[key] = value;
    }

    public bool TryGetValue(string key, out byte[] value)
    {
        return _values.TryGetValue(key, out value!);
    }
}

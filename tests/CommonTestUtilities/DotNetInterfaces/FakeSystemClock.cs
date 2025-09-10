using Microsoft.Extensions.Internal;

namespace CommonTestUtilities.DotNetInterfaces;

/// <summary>
/// Provides a testable implementation of <see cref="ISystemClock"/> that allows manual control of the current time.
/// </summary>
/// <remarks>
/// This class allows you to control the computer's time virtually, enabling tests that the computer needs to wait for, instantly.
/// </remarks>
public class FakeSystemClock : ISystemClock
{
    public DateTimeOffset UtcNow { get; private set; }

    public FakeSystemClock(DateTime? startTime = null)
    {
        UtcNow = startTime ?? DateTimeOffset.UtcNow;
    }

    public void Advance(TimeSpan amount) => UtcNow.Add(amount);
}

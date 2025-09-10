using System.Collections;

namespace WebApi.Test.InlineData;

internal class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en" };
        yield return new object[] { "pt" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

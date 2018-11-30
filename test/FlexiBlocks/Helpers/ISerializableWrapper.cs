using Xunit.Abstractions;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public interface ISerializableWrapper<out T> : IXunitSerializable
    {
        // Can't have a setter since T is covariant. Consider ISerializableWrapper<Exception> wrapper = ISerializableWrapper<IOException>.
        // This would be illogical since Value must be an IOException instance: wrapper.Value = new FileNotFoundException();
        // The compiler simply bans creating a setter to avoid the above.
        T Value { get; }
    }
}

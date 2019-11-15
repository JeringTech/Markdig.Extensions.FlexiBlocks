using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class SerializableWrapper<T> : ISerializableWrapper<T>
    {
        private const string VALUE_KEY = "VALUE_KEY";
        public T Value { get; private set; }

        // Required by xUnit
        public SerializableWrapper()
        {
        }

        public SerializableWrapper(T target)
        {
            Value = target;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Value = JsonConvert.DeserializeObject<T>(info.GetValue<string>(VALUE_KEY));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(VALUE_KEY, JsonConvert.SerializeObject(Value));
        }
    }
}

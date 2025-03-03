using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace BM.Utils.Tools
{
    public class DeepCloneClass<T> where T : class
    {
        public async Task<T> DeepClone()
        {
            await using var objectStream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, this);
            objectStream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(objectStream) as T;
        }
    }
}

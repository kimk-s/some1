using MagicOnion.Client;
using Some1.Wait.Back;

namespace Some1.User.Unity
{
    [MagicOnionClientGeneration(typeof(IWaitBackMagicService), Serializer = MagicOnionClientGenerationAttribute.GenerateSerializerType.MemoryPack)]
    partial class MagicOnionGeneratedClientInitializer { }
}

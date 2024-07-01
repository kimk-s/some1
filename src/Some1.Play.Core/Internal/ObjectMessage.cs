namespace Some1.Play.Core.Internal
{
    internal readonly struct ObjectMessage
    {
        internal ObjectMessage(
            ObjectMessageBody body,
            int frameCount)
        {
            Body = body;
            FrameCount = frameCount;
        }

        internal ObjectMessageBody Body { get; }
        internal int FrameCount { get; }
    }

    internal readonly struct ObjectMessageBody
    {
        public ObjectMessageBody(
            ObjectMessageType type,
            int intParam1 = 0,
            int intParam2 = 0,
            int intParam3 = 0,
            int intParam4 = 0,
            int intParam5 = 0,
            int intParam6 = 0,
            float floatParam1 = 0,
            float floatParam2 = 0,
            float floatParam3 = 0,
            float floatParam4 = 0)
        {
            Type = type;
            IntParam1 = intParam1;
            IntParam2 = intParam2;
            IntParam3 = intParam3;
            IntParam4 = intParam4;
            IntParam5 = intParam5;
            IntParam6 = intParam6;
            FloatParam1 = floatParam1;
            FloatParam2 = floatParam2;
            FloatParam3 = floatParam3;
            FloatParam4 = floatParam4;
        }

        internal ObjectMessageType Type { get; }
        internal int IntParam1 { get; }
        internal int IntParam2 { get; }
        internal int IntParam3 { get; }
        internal int IntParam4 { get; }
        internal int IntParam5 { get; }
        internal int IntParam6 { get; }
        internal float FloatParam1 { get; }
        internal float FloatParam2 { get; }
        internal float FloatParam3 { get; }
        internal float FloatParam4 { get; }
    }
}

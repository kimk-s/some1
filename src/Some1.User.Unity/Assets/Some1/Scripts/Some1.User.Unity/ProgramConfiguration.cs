namespace Some1.User.Unity
{
    public class ProgramConfiguration
    {
        public ProgramConfiguration(bool inMemory, string waitServerAddress)
        {
            InMemory = inMemory;
            WaitServerAddress = waitServerAddress;
        }

        public bool InMemory { get; }
        public string WaitServerAddress { get; }
    }
}

// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("r04ejdoBGy28EoN5iLqt5gDxWcg1tri3hzW2vbU1tra3C3XdQaoA8WcwBXPZ17lPhYj5xOy40rPznUt4dZ8taRmZH2wmjSeiKNLICj2JzsyHNbaVh7qxvp0x/zFAura2trK3tPKpEBv1S8sWco2WDO11Rl7eHAxbacnKHBvB4uxXIJvX5QvGklkXar4kcbeNWeKs8SBb9f/eXxvqNCgsNMbd/5KK5PJZ9znDlalwbPTRp5EcEqG5oVzDG1EGUvI211jeatZKBDEtVBg7Mqv/mZd2EIzCra54eB5ZkOphk0ysP+dL7QA+s+Sw/VTXCtM8Lg0fA8AkQ61rq4ejHLCTgPiB7pyUSv9KPjcPgp4YGfR4Tzvv7LbH48nllRfjEWKPJLW0tre2");
        private static int[] order = new int[] { 8,1,10,9,8,11,13,13,8,13,13,12,12,13,14 };
        private static int key = 183;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

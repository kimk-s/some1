// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("3CbdTqwFujeG8DSxNltIwo85xxb/iOsDy2K9LHoxAoNbh2mJ4qJxyFskXQy7gTSBKxEnmq1/N1s7EIJ0ZTn4AQaa1rlaqE6x4AbLKNezzZKw5ix+MSWxiAymKmGIp2SW15H+4pwurY6coaqlhirkKluhra2tqayvBZjHF8jWwlkMZ1ji6FRFs2YowpwPudOU/zN+tUlz55x1Nb33mkcGGOUgF9IJzp4Aen0vgu1rCoU4VyZYay+zaJxwHUeQDyZGH7QW+2oSV9nJnXRXeL1viigNEBoYmH7HYWroxC6to6ycLq2mri6trawZ7Sy3jtI4S6HO3+vs651np+P3z8578QxZkAndjME5She2W/wEdJYHmWxjLkdhHRP0jKAq2ctkOa6vrayt");
        private static int[] order = new int[] { 9,5,2,12,10,9,13,13,13,11,13,11,13,13,14 };
        private static int key = 172;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

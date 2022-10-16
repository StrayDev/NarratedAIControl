// System
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

// Otherworld
namespace Otherworld.Core
{
    public static class ListExtention
    {
        // Source
        // https://stackoverflow.com/questions/273313/randomize-a-listt
        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = new RNGCryptoServiceProvider();
            var n = list.Count;
            
            while (n > 1)
            {
                var box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                var k = (box[0] % n);
                n--;
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
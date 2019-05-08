﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SafeComm.Core.Encryption
{
    internal class StrongNumberProvider
    {
        private static readonly RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();

        public uint NextUInt32()
        {
            byte[] res = new byte[4];
            csp.GetBytes(res);
            return BitConverter.ToUInt32(res, 0);
        }

        public int NextInt()
        {
            byte[] res = new byte[4];
            csp.GetBytes(res);
            return BitConverter.ToInt32(res, 0);
        }

        public float NextSingle()
        {
            float numerator = NextUInt32();
            const float denominator = uint.MaxValue;
            return numerator / denominator;
        }
    }
}

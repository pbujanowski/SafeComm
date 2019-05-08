﻿using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace SafeComm.Core.Encryption
{
    public class DiffieHellman : IDisposable
    {
        #region - Util -
        private static readonly StrongNumberProvider _strongRng = new StrongNumberProvider();
        #endregion

        #region - Fields -
        /// <summary>
        /// The number of bits to generate.
        /// </summary>
        private readonly int bits = 256;

        /// <summary>
        /// The shared prime.
        /// </summary>
        private BigInteger prime;

        /// <summary>
        /// The shared base.
        /// </summary>
        private BigInteger g;

        /// <summary>
        /// The private prime.
        /// </summary>
        private BigInteger mine;

        /// <summary>
        /// The string representation/packet.
        /// </summary>
        private string representation;
        #endregion

        #region - Properties -
        /// <summary>
        /// Gets the final key to use for encryption.
        /// </summary>
        public byte[] Key { get; private set; }
        #endregion

        #region - Ctor -
        public DiffieHellman()
        {
        }

        public DiffieHellman(int bits)
        {
            this.bits = bits;
        }

        ~DiffieHellman()
        {
            Dispose();
        }
        #endregion

        #region - Implementation Methods -
        #region Flow
        /// <summary>
        /// Generates a request packet.
        /// </summary>
        /// <returns></returns>
        public DiffieHellman GenerateRequest()
        {
            // Generate the parameters.
            prime = BigInteger.GenPseudoPrime(bits, 30, _strongRng);
            mine = BigInteger.GenPseudoPrime(bits, 30, _strongRng);
            g = 5;

            // Gemerate the string.
            StringBuilder rep = new StringBuilder();
            rep.Append(prime.ToString(36));
            rep.Append("|");
            rep.Append(g.ToString(36));
            rep.Append("|");

            // Generate the send BigInt.
            using (var send = g.ModPow(mine, prime))
            {
                rep.Append(send.ToString(36));
            }

            representation = rep.ToString();
            return this;
        }

        /// <summary>
        /// Generate a response packet.
        /// </summary>
        /// <param name="request">The string representation of the request.</param>
        /// <returns></returns>
        public DiffieHellman GenerateResponse(string request)
        {
            string[] parts = request.Split('|');

            // Generate the would-be fields.
            using (var prime = new BigInteger(parts[0], 36))
            using (var g = new BigInteger(parts[1], 36))
            using (var mine = BigInteger.GenPseudoPrime(bits, 30, _strongRng))
            {
                // Generate the key.
                using (var given = new BigInteger(parts[2], 36))
                using (var key = given.ModPow(mine, prime))
                {
                    this.Key = key.GetBytes();
                }
                // Generate the response.
                using (var send = g.ModPow(mine, prime))
                {
                    this.representation = send.ToString(36);
                }
            }

            return this;
        }

        /// <summary>
        /// Generates the key after a response is received.
        /// </summary>
        /// <param name="response">The string representation of the response.</param>
        public void HandleResponse(string response)
        {
            // Get the response and modpow it with the stored prime.
            using (BigInteger given = new BigInteger(response, 36))
            using (BigInteger key = given.ModPow(mine, prime))
            {
                this.Key = key.GetBytes();
            }
            Dispose();
        }
        #endregion

        public override string ToString()
        {
            return representation;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Ends the calculation. The key will still be available.
        /// </summary>
        public void Dispose()
        {
            if (!ReferenceEquals(prime, null))
                prime.Dispose();
            if (!ReferenceEquals(mine, null))
                mine.Dispose();
            if (!ReferenceEquals(g, null))
                g.Dispose();

            prime = null;
            mine = null;
            g = null;

            representation = null;
            GC.Collect();
            GC.Collect();
        }

        #endregion
    }
}

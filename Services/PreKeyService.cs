﻿using CoreLib.Crypto;
using CoreLib.Models.Entitys;
using Org.BouncyCastle.Crypto;

namespace CoreLib.Services
{
    internal class PreKeyService
    {
        public async Task<PreKey> GeneratePreKeyAsync(AsymmetricKeyParameter dilithiumPrivateKey)
        {
            var kyberKey = new KyberKey();
            var shake256 = new Shake256Generator();
            var dilithiumKey = new DilitiumKey();
            var (publicKey, privateKey) = await kyberKey.GenerateKeyPairAsync();
            var preKey = new PreKey
            {
                Id = await shake256.GetHex(await shake256.ComputeHash(publicKey.GetEncoded())),
                PK = publicKey.GetEncoded(),
                PrK = privateKey.GetEncoded(),
                Signature = await dilithiumKey.SignAsync(publicKey.GetEncoded(), dilithiumPrivateKey)
            };
            return preKey;
        }
    }
}

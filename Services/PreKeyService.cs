﻿using CoreLib.Crypto;
using CoreLib.Models.Entitys;
using Org.BouncyCastle.Crypto.Parameters;

namespace CoreLib.Services
{
    public interface IPreKeyService
    {
        Task<PreKey> CreateAsync(MLDsaPrivateKeyParameters dilithiumPrivateKey, string deviceId);
    }
    internal class PreKeyService(IMLKemKey mLKemKey, IMLDsaKey mLDsaKey, IShakeGenerator shakeGenerator) : IPreKeyService
    {
        public async Task<PreKey> CreateAsync(MLDsaPrivateKeyParameters dilithiumPrivateKey, string deviceId)
        {
            try
            {
                var (mLKemKeyPK, mLKemKeyPrK) = await mLKemKey.GenerateKeyPairAsync();
                var preKey = new PreKey
                {
                    Id = await shakeGenerator.GetString(await shakeGenerator.ComputeHash256(mLKemKeyPK.GetEncoded(),64)),
                    PK = mLKemKeyPK.GetEncoded(),
                    PrK = mLKemKeyPrK.GetEncoded(),
                    Signature = await mLDsaKey.SignAsync(mLKemKeyPK.GetEncoded(), dilithiumPrivateKey)
                };
                return preKey;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating prekey", ex);
            }
        }
    }
}

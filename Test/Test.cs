using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Neo.SmartContract.Examples
{
    [DisplayName("TestNNS")]
    [ManifestExtra("Author", "Neo")]
    [ManifestExtra("Email", "dev@neo.org")]
    [ManifestExtra("Description", "This is a test contract for invoking NNS")]
    [ContractPermission("*", "*")]
    public class Test : Framework.SmartContract
    {
        [InitialValue("NdUL5oDPD159KeFpD5A9zw5xNF1xLX6nLT", ContractParameterType.Hash160)]
        private static readonly UInt160 owner = default;

        private const byte Prefix_Contract = 0x02;
        public static readonly StorageMap ContractMap = new(Storage.CurrentContext, Prefix_Contract);
        private static readonly byte[] ownerKey = "owner".ToByteArray();
        private static bool IsOwner() => Runtime.CheckWitness(GetOwner());

        [InitialValue("0x50ac1c37690cc2cfc594472833cf57505d5f46de", ContractParameterType.Hash160)]
        private static readonly UInt160 nnsHash = default;

        public static void _deploy(object data, bool update)
        {
            if (update) return;
            ContractMap.Put(ownerKey, owner);
        }

        public static UInt160 GetOwner()
        {
            return (UInt160)ContractMap.Get(ownerKey);
        }

        public static bool Update(ByteString nefFile, ByteString manifest)
        {
            if (!IsOwner()) throw new InvalidOperationException("No Authorization!!");
            ContractManagement.Update(nefFile, manifest, null);
            return true;
        }

        public static bool RegisterNameService(string name)
        {
            bool result = (bool)Contract.Call(nnsHash, "register", CallFlags.All, name, Runtime.ExecutingScriptHash);
            return result;
        }

        public static void onNEP11Payment(UInt160 from, BigInteger amount, ByteString tokenId, Object data)
        {

        }
    }
}

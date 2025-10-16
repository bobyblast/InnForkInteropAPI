
using InnForkInteropAPI.BlockChain.NeoN3.NeoSC_Helpers.MessagesParsing;
using Neo;
using Neo.Extensions;
using Neo.Network.P2P.Payloads;
using Neo.Network.RPC;
using Neo.Network.RPC.Models;
using Neo.SmartContract;
using Neo.SmartContract.Manifest;
using Neo.VM;
using Neo.Wallets;
using static InnFork.Blockchain.NEO3.NeoConnectClientService;

namespace InnFork.Blockchain.NEO3
{
    public enum NeoN3Scope { CallByEntry, Global }

    public class SignVerifyResponse
    {
        public string? PublicKey { get; set; }
        public string? Message { get; set; }
        public bool Error { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public interface INeoSCInterop
    {
        static string InnforkGatewayAddress { get; set; } = ""; //!!

        string GetScriptHashOfPublicKey(string publicKey);
        UInt160 GetUInt160ScriptHashOfPublicKey(string publicKey);
        Task<RpcApplicationLog?> InvokeSC(UInt160 contractScriptHash, string methodName, NeoN3Scope scope, InnFork_RpcStack[] rpcStackArgs, int extraNetworkFee = 1000000, int extraSystemFee = 50000, CancellationToken cancellationToken = default);
        Task<RpcInvokeResult?> NeoContractClient_TestInvoke(UInt160 contractScriptHash, string methodName, RpcStack[] rpcStackArgs, string projectId, CancellationToken cancellationToken = default);
        Task<SignVerifyResponse> SignVerifyGetPublicKeyByWCUser(Guid baseActorId, Guid backerId, UInt160 n3WalletAddress);
        Task<RpcInvokeResult?> TestInvokeSC(UInt160 contractScriptHash, string methodName, NeoN3Scope scope, InnFork_RpcStack[] rpcStackArgs, int extraNetworkFee = 1000000, int extraSystemFee = 50000, CancellationToken cancellationToken = default);
    }







    public partial class NeoSCInterop : INeoSCInterop
    {
        private readonly NeoRpcClientOptions _options;

        private readonly NeoConnectClientService _clientService;
        public NeoSCInterop(bool useTestNet = true)
            : this(NeoRpcClientOptions.CreateDefault(useTestNet))
        {
        }

        public NeoSCInterop(NeoRpcClientOptions options, NeoConnectClientService? clientService = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _clientService = clientService ?? new NeoConnectClientService(_options);
        }

        public string? DefaultUserWIF { get; set; }

        private RpcClient RpcClient => _clientService.RpcClient;
        private ContractClient ContractClient => _clientService.ContractClient;
        public bool TestNet => _options.UseTestNet;

        public static string InnforkGatewayAddress { get; set; } = INeoSCInterop.InnforkGatewayAddress;

        public async Task<RpcApplicationLog?> InvokeSC(UInt160 contractScriptHash, string methodName, NeoN3Scope scope, InnFork_RpcStack[] rpcStackArgs, int extraNetworkFee = 0, int extraSystemFee = 0, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException("MethodName required", nameof(methodName));
            if (rpcStackArgs is null) throw new ArgumentNullException(nameof(rpcStackArgs));
            if (string.IsNullOrEmpty(DefaultUserWIF)) throw new InvalidOperationException("DefaultUserWIF not set for InvokeSC");

            var txHash = await InvokeSC_RpcStack_WithWIF(contractScriptHash, methodName, rpcStackArgs, DefaultUserWIF!, scope, extraSystemFee, extraNetworkFee, cancellationToken).ConfigureAwait(false);
            if (txHash == null) return null;
            return await GetInvokedResultsExecutionsListByTxIdAsync(txHash, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<RpcInvokeResult?> TestInvokeSC(UInt160 contractScriptHash, string methodName, NeoN3Scope scope, InnFork_RpcStack[] rpcStackArgs, int extraNetworkFee = 0, int extraSystemFee = 0, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException("MethodName required", nameof(methodName));
            if (rpcStackArgs is null) throw new ArgumentNullException(nameof(rpcStackArgs));
            return await TestInvokeSC_RpcStack_WithWIF(contractScriptHash, methodName, rpcStackArgs, DefaultUserWIF ?? string.Empty, scope, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<RpcApplicationLog?> UpdateContract(string nefFilePath, string manifestFilePath, UInt160? contractHash = null, string method = "update", CancellationToken cancellationToken = default)
        {
            try
            {
                byte[] nefBytes = await File.ReadAllBytesAsync(nefFilePath, cancellationToken).ConfigureAwait(false);
                NefFile nefFile = NefFile.Parse(nefBytes);

                ContractManifest manifest = ContractManifest.Parse(await File.ReadAllBytesAsync(manifestFilePath, cancellationToken).ConfigureAwait(false));

                InnFork_RpcStack nefStack = new() { Type = nameof(NeoRpcStackFabric.RpcEnumTypes.ByteArray), Value = Convert.ToBase64String(nefFile.ToArray()) };
                InnFork_RpcStack manifestStack = new() { Type = nameof(NeoRpcStackFabric.RpcEnumTypes.String), Value = manifest.ToJson().ToString() };
                InnFork_RpcStack dataStack = new() { Type = nameof(NeoRpcStackFabric.RpcEnumTypes.Any), Value = null };

                UInt160 targetContract = contractHash ?? UInt160.Parse(InnforkGatewayAddress);

                if (!string.IsNullOrEmpty(DefaultUserWIF))
                {
                    var txHash = await InvokeSC_RpcStack_WithWIF(targetContract, method, new[] { nefStack, manifestStack, dataStack }, DefaultUserWIF!, NeoN3Scope.Global, cancellationToken: cancellationToken).ConfigureAwait(false);
                    if (txHash == null) return null;
                    return await GetInvokedResultsExecutionsListByTxIdAsync(txHash, cancellationToken: cancellationToken).ConfigureAwait(false);
                }

                await TestInvokeSC_RpcStack_WithWIF(targetContract, method, new[] { nefStack, manifestStack, dataStack }, string.Empty, NeoN3Scope.Global, cancellationToken).ConfigureAwait(false);
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating InnFork contract: {ex.Message}");
                return null;
            }
        }

        public async Task DeployContract(string nefFilePath, string manifestFilePath, string wif, string? rpcAddress = null, CancellationToken cancellationToken = default)
        {
            var rpcUri = !string.IsNullOrWhiteSpace(rpcAddress) ? new Uri(rpcAddress) : _options.RpcUri;
            ProtocolSettings protocolSettings = _options.ProtocolSettings;
            RpcClient client = new RpcClient(rpcUri, null, null, protocolSettings);
            ContractClient contractClient = new ContractClient(client);

            byte[] nefBytes = await File.ReadAllBytesAsync(nefFilePath, cancellationToken).ConfigureAwait(false);
            NefFile nefFile = NefFile.Parse(nefBytes);
            ContractManifest manifest = ContractManifest.Parse(await File.ReadAllBytesAsync(manifestFilePath, cancellationToken).ConfigureAwait(false));

            KeyPair senderKey = Neo.Network.RPC.Utility.GetKeyPair(wif);
            Transaction transaction = await contractClient.CreateDeployContractTxAsync(nefFile.ToArray(), manifest, senderKey).ConfigureAwait(false);

            await client.SendRawTransactionAsync(transaction).ConfigureAwait(false);

            WalletAPI neoAPI = new WalletAPI(client);
            var result = await neoAPI.WaitTransactionAsync(transaction).ConfigureAwait(false);
            Console.WriteLine($"Transaction vm state is {result.VMState}");
        }

        public string GetScriptHashOfPublicKey(string publicKey)
        {
            var ecPointPublicKey = Neo.Cryptography.ECC.ECPoint.Parse(publicKey, Neo.Cryptography.ECC.ECCurve.Secp256r1);
            UInt160 scriptHash = Contract.CreateSignatureContract(ecPointPublicKey).ScriptHash;
            return scriptHash.ToString();
        }

        public UInt160 GetUInt160ScriptHashOfPublicKey(string publicKey)
        {
            var ecPointPublicKey = Neo.Cryptography.ECC.ECPoint.Parse(publicKey, Neo.Cryptography.ECC.ECCurve.Secp256r1);
            return Contract.CreateSignatureContract(ecPointPublicKey).ScriptHash;
        }

        public async Task<RpcInvokeResult?> NeoContractClient_TestInvoke(UInt160 contractScriptHash, string methodName, RpcStack[] rpcStackArgs, string projectId, CancellationToken cancellationToken = default)
        {
            try
            {
                _ = projectId;
                var parameters = rpcStackArgs?.Select(InnForkRpcStackConverter.FromRpcStack).Select(stack => stack.ToContractParameter()).ToArray() ?? Array.Empty<ContractParameter>();
                return await ContractClient.TestInvokeAsync(contractScriptHash, methodName, parameters).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in NeoContractClient_TestInvoke: {ex.Message}");
                return null;
            }
        }

        public Task<RpcInvokeResult?> TestInvokeSCWithContractParamenets(UInt160 contractHash, string method, ContractParameter[] contractParameters, CancellationToken cancellationToken = default)
            => TestInvokeSC_WithWIF(contractHash, method, contractParameters, DefaultUserWIF ?? string.Empty, NeoN3Scope.Global, cancellationToken);

        public async Task<RpcApplicationLog?> InvokeSCWithContractParamenets(UInt160 contractHash, string method, ContractParameter[] contractParameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(DefaultUserWIF)) throw new InvalidOperationException("DefaultUserWIF not set");
            var txHash = await InvokeSC_WithWIF(contractHash, method, contractParameters, DefaultUserWIF!, NeoN3Scope.Global, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (txHash == null) return null;
            return await GetInvokedResultsExecutionsListByTxIdAsync(txHash, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<RpcApplicationLog?> InvokeSC_ContractParameters(UInt160 contractScriptHash, string methodName, NeoN3Scope scope, ContractParameter[] contractParameterArgs, int extraNetworkFee = 0, int extraSystemFee = 0, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException("MethodName required", nameof(methodName));
            if (contractParameterArgs is null) throw new ArgumentNullException(nameof(contractParameterArgs));
            if (string.IsNullOrEmpty(DefaultUserWIF)) throw new InvalidOperationException("DefaultUserWIF not set");
            var txHash = await InvokeSC_WithWIF(contractScriptHash, methodName, contractParameterArgs, DefaultUserWIF!, scope, extraSystemFee, extraNetworkFee, cancellationToken).ConfigureAwait(false);
            if (txHash == null) return null;
            return await GetInvokedResultsExecutionsListByTxIdAsync(txHash, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public Task<RpcInvokeResult?> InvokeSC_RpcStacks(UInt160 contractScriptHash, string methodName, NeoN3Scope scope, RpcStack[] rpcStackArgs, int extraNetworkFee = 0, int extraSystemFee = 0, CancellationToken cancellationToken = default)
        {
            var stacks = rpcStackArgs?.Select(InnForkRpcStackConverter.FromRpcStack).ToArray() ?? Array.Empty<InnFork_RpcStack>();
            return TestInvokeSC(contractScriptHash, methodName, scope, stacks, extraNetworkFee, extraSystemFee, cancellationToken);
        }

        public async Task<RpcApplicationLog?> InvokeSC_IFRpcStacks(UInt160 contractScriptHash, string methodName, NeoN3Scope scope, InnFork_RpcStack[] rpcStackArgs, int extraNetworkFee = 0, int extraSystemFee = 0, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException("MethodName required", nameof(methodName));
            if (rpcStackArgs is null) throw new ArgumentNullException(nameof(rpcStackArgs));
            if (string.IsNullOrEmpty(DefaultUserWIF)) throw new InvalidOperationException("DefaultUserWIF not set");
            var txHash = await InvokeSC_RpcStack_WithWIF(contractScriptHash, methodName, rpcStackArgs, DefaultUserWIF!, scope, extraSystemFee, extraNetworkFee, cancellationToken).ConfigureAwait(false);
            if (txHash == null) return null;
            return await GetInvokedResultsExecutionsListByTxIdAsync(txHash, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<RpcInvokeResult?> TestInvokeSC_WithWIF(UInt160 contractScriptHash, string methodName, ContractParameter[] parameters, string wif, NeoN3Scope scope = NeoN3Scope.CallByEntry, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException("MethodName is required", nameof(methodName));
            if (parameters is null) throw new ArgumentNullException(nameof(parameters));
            _ = scope;

            return await ContractClient.TestInvokeAsync(contractScriptHash, methodName, parameters).ConfigureAwait(false);
        }

        public async Task<string?> InvokeSC_WithWIF(UInt160 contractScriptHash, string methodName, ContractParameter[] parameters, string wif, NeoN3Scope scope = NeoN3Scope.CallByEntry, long extraSystemFee = 0, long extraNetworkFee = 0, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(wif)) throw new ArgumentException("WIF is required", nameof(wif));
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException("MethodName is required", nameof(methodName));
            if (parameters is null) throw new ArgumentNullException(nameof(parameters));

            KeyPair keyPair = Neo.Network.RPC.Utility.GetKeyPair(wif);
            UInt160 sender = Contract.CreateSignatureContract(keyPair.PublicKey).ScriptHash;

            using ScriptBuilder sb = new();
            sb.EmitDynamicCall(contractScriptHash, methodName, parameters);
            byte[] script = sb.ToArray();

            var testInvokeResult = await RpcClient.InvokeScriptAsync(script).ConfigureAwait(false);

            long systemFee = (long)Math.Ceiling((double)testInvokeResult.GasConsumed) + Math.Max(extraSystemFee, _options.DefaultExtraSystemFee);
            long networkFee = Math.Max(extraNetworkFee, _options.DefaultExtraNetworkFee);

            uint currentHeight = await RpcClient.GetBlockCountAsync().ConfigureAwait(false);

            Transaction tx = new()
            {
                Version = 0,
                Script = script,
                Signers = new[] { new Signer { Account = sender, Scopes = ResolveScope(scope) } },
                Attributes = Array.Empty<TransactionAttribute>(),
                ValidUntilBlock = currentHeight + 100,
                SystemFee = systemFee,
                NetworkFee = networkFee
            };

            tx.Sign(keyPair, _options.ProtocolSettings.Network);
            var sentHash = await RpcClient.SendRawTransactionAsync(tx).ConfigureAwait(false);
            if (sentHash == null) return null;

            return tx.Hash.ToString();
        }

        public async Task<RpcInvokeResult?> TestInvokeSC_RpcStack_WithWIF(UInt160 contractScriptHash, string methodName, InnFork_RpcStack[] rpcStacks, string wif, NeoN3Scope scope = NeoN3Scope.CallByEntry, CancellationToken cancellationToken = default)
        {
            var parameters = InnForkRpcStackConverter.ToContractParameters(rpcStacks);
            return await TestInvokeSC_WithWIF(contractScriptHash, methodName, parameters, wif, scope, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string?> InvokeSC_RpcStack_WithWIF(UInt160 contractScriptHash, string methodName, InnFork_RpcStack[] rpcStacks, string wif, NeoN3Scope scope = NeoN3Scope.CallByEntry, long extraSystemFee = 0, long extraNetworkFee = 0, CancellationToken cancellationToken = default)
        {
            var parameters = InnForkRpcStackConverter.ToContractParameters(rpcStacks);
            return await InvokeSC_WithWIF(contractScriptHash, methodName, parameters, wif, scope, extraSystemFee, extraNetworkFee, cancellationToken).ConfigureAwait(false);
        }

        public Task<SignVerifyResponse> SignVerifyGetPublicKeyByWCUser(Guid baseActorId, Guid backerId, UInt160 n3WalletAddress)
            => Task.FromResult(new SignVerifyResponse { PublicKey = null, Message = null, Error = true, ErrorMessage = "Not implemented in NeoInteropService" });

        private WitnessScope ResolveScope(NeoN3Scope scope) => scope == NeoN3Scope.Global ? WitnessScope.Global : WitnessScope.CalledByEntry;

        private async Task<RpcApplicationLog?> GetInvokedResultsExecutionsListByTxIdAsync(string txId, int maxAttempts = 30, int delayMs = 2000, CancellationToken cancellationToken = default)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    RpcApplicationLog rpcLog = await RpcClient.GetApplicationLogAsync(txId).ConfigureAwait(false);
                    if (rpcLog?.Executions?.Count > 0 && rpcLog.Executions[0].Stack != null)
                    {
                        return rpcLog;
                    }
                }
                catch when (attempt < maxAttempts - 1)
                {
                }

                if (attempt < maxAttempts - 1)
                {
                    await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);
                }
            }

#if DEBUG
            Console.WriteLine($"RpcLog is NULL. time out of get txid data! : {txId}");
#endif

            return null;
        }
    }
}

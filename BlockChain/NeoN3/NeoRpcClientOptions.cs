using System.Globalization;
using System.IO;
using System;
using Neo;
using Neo.Network.RPC;

namespace InnFork.Blockchain.NEO3;

/// <summary>
/// Centralised configuration for NeoN3 RPC interactions.  The project previously relied on
/// several hard-coded strings which made switching between networks or environments difficult.
/// The <see cref="NeoRpcClientOptions"/> class encapsulates these values, exposes well-known
/// defaults and allows callers to customise the behaviour through environment variables or
/// explicit construction.
/// </summary>
public sealed class NeoRpcClientOptions
{
    private const string TestNetFlag = "test";

    public const string DefaultTestNetUrl = "https://rpc.t5.n3.nspcc.ru:20331";
    public const string DefaultMainNetUrl = "https://rpc.t1.n3.nspcc.ru:20331";

    public const string DefaultTestNetProtocolConfig = "./settings/neo/config.TestNet.json";
    public const string DefaultMainNetProtocolConfig = "./settings/neo/config.mainnet.json";

    public static bool DefaultUseTestNet { get; set; } = true;

    public NeoRpcClientOptions(Uri rpcUri,
                               ProtocolSettings protocolSettings,
                               bool useTestNet,
                               long defaultExtraNetworkFee = 1_000_000,
                               long defaultExtraSystemFee = 50_000,
                               string? protocolConfigPath = null)
    {
        RpcUri = rpcUri ?? throw new ArgumentNullException(nameof(rpcUri));
        ProtocolSettings = protocolSettings ?? throw new ArgumentNullException(nameof(protocolSettings));
        UseTestNet = useTestNet;
        DefaultExtraNetworkFee = defaultExtraNetworkFee;
        DefaultExtraSystemFee = defaultExtraSystemFee;
        ProtocolConfigPath = protocolConfigPath;
    }

    public Uri RpcUri { get; }
    public ProtocolSettings ProtocolSettings { get; }
    public bool UseTestNet { get; }
    public long DefaultExtraNetworkFee { get; }
    public long DefaultExtraSystemFee { get; }
    public string? ProtocolConfigPath { get; }

    public static NeoRpcClientOptions CreateDefault(bool useTestNet = true)
    {
        return useTestNet ? CreateTestNetDefaults() : CreateMainNetDefaults();
    }

    public static NeoRpcClientOptions CreateFromEnvironment(bool? useTestNetOverride = null)
    {
        string? rpcUrlFromEnv = Environment.GetEnvironmentVariable("NEO_RPC_ENDPOINT");
        string? protocolConfig = Environment.GetEnvironmentVariable("NEO_PROTOCOL_CONFIG");
        string? network = Environment.GetEnvironmentVariable("NEO_NETWORK");
        string? systemFee = Environment.GetEnvironmentVariable("NEO_EXTRA_SYSTEM_FEE");
        string? networkFee = Environment.GetEnvironmentVariable("NEO_EXTRA_NETWORK_FEE");

        bool useTestNet = useTestNetOverride ?? ResolveNetwork(network);

        var defaults = CreateDefault(useTestNet);
        var rpcUri = !string.IsNullOrWhiteSpace(rpcUrlFromEnv) ? new Uri(rpcUrlFromEnv) : defaults.RpcUri;

        long parsedSystemFee = ParseFee(systemFee, defaults.DefaultExtraSystemFee);
        long parsedNetworkFee = ParseFee(networkFee, defaults.DefaultExtraNetworkFee);

        ProtocolSettings protocolSettings = ResolveProtocolSettings(protocolConfig, defaults.ProtocolSettings);

        return new NeoRpcClientOptions(rpcUri, protocolSettings, useTestNet, parsedNetworkFee, parsedSystemFee, protocolConfig ?? defaults.ProtocolConfigPath);
    }

    private static bool ResolveNetwork(string? network)
    {
        if (string.IsNullOrWhiteSpace(network))
        {
            return DefaultUseTestNet;
        }

        network = network.Trim();
        if (network.Equals("main", StringComparison.OrdinalIgnoreCase) ||
            network.Equals("mainnet", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (network.Equals("TestNet", StringComparison.OrdinalIgnoreCase) ||
            network.Equals(TestNetFlag, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // fall back to boolean parsing
        if (bool.TryParse(network, out bool boolValue))
        {
            return boolValue;
        }

        return DefaultUseTestNet;
    }

    private static long ParseFee(string? value, long defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long parsed))
        {
            return parsed;
        }

        return defaultValue;
    }

    private static NeoRpcClientOptions CreateTestNetDefaults()
    {
        ProtocolSettings settings = ResolveProtocolSettings(DefaultTestNetProtocolConfig, ProtocolSettings.Default);
        return new NeoRpcClientOptions(new Uri(DefaultTestNetUrl), settings, useTestNet: true, protocolConfigPath: DefaultTestNetProtocolConfig);
    }

    private static NeoRpcClientOptions CreateMainNetDefaults()
    {
        ProtocolSettings settings = ResolveProtocolSettings(DefaultMainNetProtocolConfig, ProtocolSettings.Default);
        return new NeoRpcClientOptions(new Uri(DefaultMainNetUrl), settings, useTestNet: false, protocolConfigPath: DefaultMainNetProtocolConfig);
    }

    private static ProtocolSettings ResolveProtocolSettings(string? path, ProtocolSettings fallback)
    {
        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            try
            {
                return ProtocolSettings.Load(path);
            }
            catch
            {
                // fall back to provided default
            }
        }

        return fallback ?? ProtocolSettings.Default;
    }
}



using Neo.SmartContract.Framework.Native;
using System;
using System.Numerics;
using InnFork.Blockchain.NEO3;

using Neo.SmartContract;
using Neo;

namespace InnForkInteropAPI.BlockChain.NeoN3.SC_Wrappers
{
    public enum ViolationFlag : int
    {
        None = 0,
        Spam = 1 << 0,              // Спам -> BanReason.SpamVoting
        Scam = 1 << 1,              // Мошенничество -> BanReason.FraudulentActivity
        OffensiveLanguage = 1 << 2, // Оскорбительные выражения -> BanReason.InappropriateBehavior
        FalseInformation = 1 << 3,  // Ложная информация -> BanReason.FalseDispute
        Copyright = 1 << 4,         // Нарушение авторских прав -> BanReason.IntellectualPropertyTheft
        AdultContent = 1 << 5,      // Контент для взрослых -> BanReason.ViolationOfTerms
        Violence = 1 << 6,          // Насилие -> BanReason.InappropriateBehavior
        IllegalContent = 1 << 7,    // Незаконный контент -> BanReason.RegulatoryViolation
        Plagiarism = 1 << 8,        // Плагиат -> BanReason.IntellectualPropertyTheft
        LowQuality = 1 << 9,        // Низкое качество -> BanReason.QualityIssues
        Unrealistic = 1 << 10,      // Нереалистичные обещания -> BanReason.FraudSuspicion
        MissingInfo = 1 << 11,      // Недостаточно информации -> BanReason.ViolationOfTerms
        SuspiciousActivity = 1 << 12 // Подозрительная активность -> BanReason.SystemAbuse
    }


    public partial class InnFork_SCPlatform
    {


        enum Network { TestNet, MainNet }
        public class AiModeratorContract
        {
            public static UInt160 Address = "";
            public static bool testnet = false;
            public static bool TestInvoke = false;
            public static string? DefaultUserWif { get; set; }

            public static Dictionary<string, object> AnalyzeParticipantViolationPatterns(UInt160 participant, string projectId)
            {
                return ExecuteContractWithResult<Dictionary<string, object>>(Address,
                                                                      nameof(AnalyzeParticipantViolationPatterns),
                                                                      testnet,
                                                                      TestInvoke,
                                                                      DefaultUserWif,
                                                                      BuildParameters(participant, projectId));
            }

            public static void DoRequest()
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(DoRequest),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters());
            }


            public static BigInteger GetConfidenceScore(string requestId)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(GetConfidenceScore),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(requestId));
            }


            public static Dictionary<string, BigInteger> GetEnhancedModerationStats(string requestId, string projectId)
            {
                return ExecuteContractWithResult<Dictionary<string, BigInteger>>(Address,
                                                                          nameof(GetEnhancedModerationStats),
                                                                          testnet,
                                                                          TestInvoke,
                                                                          DefaultUserWif,
                                                                          BuildParameters(requestId, projectId));
            }

            public static string[] GetModerationHistory(UInt160 address)
            {
                return ExecuteContractWithResult<string[]>(Address,
                                                         nameof(GetModerationHistory),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(address)) ?? Array.Empty<string>();
            }


            public static object[] GetModerationHistoryWithContext(UInt160 address, string projectId)
            {
                return ExecuteContractWithResult<object[]>(Address,
                                                         nameof(GetModerationHistoryWithContext),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(address, projectId)) ?? Array.Empty<object>();
            }


            public static Dictionary<string, BigInteger> GetModerationStats(string requestId)
            {
                return ExecuteContractWithResult<Dictionary<string, BigInteger>>(Address,
                                                                          nameof(GetModerationStats),
                                                                          testnet,
                                                                          TestInvoke,
                                                                          DefaultUserWif,
                                                                          BuildParameters(requestId));
            }

            public static string GetResponse()
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(GetResponse),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters()) ?? string.Empty;
            }


            public static bool HasViolationFlag(string requestId, ViolationFlag flag)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(HasViolationFlag),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(requestId, flag));
            }


            public static bool IsModerationApproved(string requestId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(IsModerationApproved),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(requestId));
            }


            public static void ManuallyApplySanctions(string requestId, string projectId, UInt160 admin)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(ManuallyApplySanctions),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(requestId, projectId, admin));
            }


            public static string ModerateDisputeEvidence(string projectId, string disputeId, string evidenceJson, UInt160 requester, bool autoEnforce = true)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(ModerateDisputeEvidence),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, disputeId, evidenceJson, requester, autoEnforce)) ?? string.Empty;
            }


            public static string ModerateManufacturerProfile(string projectId, UInt160 manufacturerAddress, string profileJson, UInt160 requester, bool autoEnforce = true)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(ModerateManufacturerProfile),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, manufacturerAddress, profileJson, requester, autoEnforce)) ?? string.Empty;
            }


            public static string ModerateProjectDescription(string projectId, string descriptionJson, UInt160 requester, bool autoEnforce = false)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(ModerateProjectDescription),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, descriptionJson, requester, autoEnforce)) ?? string.Empty;
            }


            public static string ModerateProjectOffer(string offerId, string offerJson, UInt160 requester, bool autoEnforce = false)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(ModerateProjectOffer),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(offerId, offerJson, requester, autoEnforce)) ?? string.Empty;
            }


            public static string ModerateProjectUpdate(string projectId, string updateId, string updateJson, UInt160 requester, bool autoEnforce = false)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(ModerateProjectUpdate),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, updateId, updateJson, requester, autoEnforce)) ?? string.Empty;
            }


            public static string ModerateUserComment(string projectId, string commentId, string commentJson, UInt160 author, bool autoEnforce = false)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(ModerateUserComment),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, commentId, commentJson, author, autoEnforce)) ?? string.Empty;
            }


            public static void OnOracleModerationResponse(string requestedUrl, object userData, OracleResponseCode oracleResponse, string jsonResponse)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(OnOracleModerationResponse),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(requestedUrl, userData, oracleResponse, jsonResponse));
            }


            public static void onOracleResponse(string requestedUrl, object userData, OracleResponseCode oracleResponse, string jsonString)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(onOracleResponse),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(requestedUrl, userData, oracleResponse, jsonString));
            }


            public static void RehabilitateParticipant(string projectId, UInt160 participant, UInt160 admin)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(RehabilitateParticipant),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, participant, admin));
            }


            public static void SetMainGatewayContract(UInt160 newAddress, UInt160 admin)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetMainGatewayContract),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(newAddress, admin));
            }


            public static void SetStateStorageContract(UInt160 newAddress, UInt160 admin)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetStateStorageContract),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(newAddress, admin));
            }

        }


    }

}
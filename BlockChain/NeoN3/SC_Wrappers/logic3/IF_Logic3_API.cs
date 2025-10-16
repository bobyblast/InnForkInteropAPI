using InnFork.NeoN3;

using System;
using System.Numerics;
using InnFork.Blockchain.NEO3;
using Neo;
using Neo.SmartContract;
using Neo.VM;

namespace InnForkInteropAPI.BlockChain.NeoN3.SC_Wrappers
{
    public partial class InnFork_SCPlatform
    {

        public partial class SCPlatform_Logic3
        {
            public static UInt160 Address = "";
            public static bool testnet = false;
            public static bool TestInvoke = false;
            public static string? DefaultUserWif { get; set; }
            public static void addConditionalVotingRule(string projectId, string ruleId, BigInteger threshold)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(addConditionalVotingRule),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, ruleId, threshold));
            }


            public static BigInteger calculateVoteWeight(string projectId, UInt160 voterAddress, string voteId)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(calculateVoteWeight),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress, voteId));
            }


            public static bool checkConditionalVotingStatus(string projectId, string ruleId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(checkConditionalVotingStatus),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, ruleId));
            }


            public static string createBlockedFundsKey(UInt160 backerAddress, BanReason reason)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(createBlockedFundsKey),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(backerAddress, reason)) ?? string.Empty;
            }


            public static string createDispute(string projectId, UInt160 initiator, DisputeType disputeType, string description, UInt160 manufacturerInvolved = null, byte milestoneStepInvolved = 0)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(createDispute),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, initiator, disputeType, description, manufacturerInvolved, milestoneStepInvolved)) ?? string.Empty;
            }


            public static void createMilestoneDispute(UInt160 initiatorAddress, string projectId, UInt160 manufacturerAddress, byte stepNumber, string disputeReason)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(createMilestoneDispute),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(initiatorAddress, projectId, manufacturerAddress, stepNumber, disputeReason));
            }


            public static void Destroy()
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(Destroy),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters());
            }


            public static void distributeTokenRewards(string projectId, UInt160 backerAddress, BigInteger tokenAmount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(distributeTokenRewards),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backerAddress, tokenAmount));
            }


            public static Dictionary<string, BigInteger> getMilestonePerformanceAnalytics(string projectId, UInt160 manufacturerAddress)
            {
                return ExecuteContractWithResult<Dictionary<string, BigInteger>>(Address,
                                                                         nameof(getMilestonePerformanceAnalytics),
                                                                         testnet,
                                                                         TestInvoke,
                                                                         DefaultUserWif,
                                                                         BuildParameters(projectId, manufacturerAddress));
            }

            public static UInt160 GetOwner()
            {
                return ExecuteContractWithResult<UInt160>(Address,
                                                         nameof(GetOwner),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters()) ?? UInt160.Zero;
            }


            public static BigInteger getParticipationTrend(string projectId, ulong startTimestamp, ulong endTimestamp)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(getParticipationTrend),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, startTimestamp, endTimestamp));
            }


            public static Dictionary<string, BigInteger> getProjectAnalytics(string projectId)
            {
                return ExecuteContractWithResult<Dictionary<string, BigInteger>>(Address,
                                                                         nameof(getProjectAnalytics),
                                                                         testnet,
                                                                         TestInvoke,
                                                                         DefaultUserWif,
                                                                         BuildParameters(projectId));
            }

            public static bool isValidStatusTransition(DisputeStatus current, DisputeStatus next)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(isValidStatusTransition),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(current, next));
            }


            public static void resolveDispute(string projectId, string disputeId, DisputeStatus newStatus, string resolutionNote)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(resolveDispute),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId, newStatus, resolutionNote));
            }


            public static void SetOwner(UInt160 newOwner)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetOwner),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(newOwner));
            }


            public static void setRewardTokenContract(string projectId, UInt160 tokenContractAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setRewardTokenContract),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, tokenContractAddress));
            }

            public static void updateDailyParticipation(string projectId, ulong timestamp, BigInteger participantsCount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(updateDailyParticipation),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, timestamp, participantsCount));
            }


            public static void voteForTransferProjectManagementToInnFork(string projectId, UInt160 backer, bool votingFor)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(voteForTransferProjectManagementToInnFork),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, votingFor));
            }


            public static void voteFundraisingCompletion(string projectId, UInt160 backer, BackerVotesEnum vote)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(voteFundraisingCompletion),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, vote));
            }


            public static void voteIncreaseProjectBudget(string projectId, UInt160 backer, BackerVotesEnum vote)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(voteIncreaseProjectBudget),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, vote));
            }


            public static void votePauseResume(string projectId, UInt160 backer, BackerVotesEnum vote)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(votePauseResume),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, vote));
            }


            public static void voteSuccessfulClosure(string projectId, UInt160 backer, BackerVotesEnum vote)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(voteSuccessfulClosure),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, vote));
            }


            public static void voteTerminationWithRefund(string projectId, UInt160 backer, BackerVotesEnum vote)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(voteTerminationWithRefund),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, vote));
            }


            public static void _deploy(object data, bool update)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(_deploy),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(data, update));
            }

        }
    }
}
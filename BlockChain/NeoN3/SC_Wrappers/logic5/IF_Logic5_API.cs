
using System;
using System.Numerics;
using InnFork.Blockchain.NEO3;
using Neo;
using Neo.SmartContract;

namespace InnForkInteropAPI.BlockChain.NeoN3.SC_Wrappers
{

    public partial class InnFork_SCPlatform
    {

        public partial class SCPlatform_Logic5
        {
            public static UInt160 Address = "";
            public static bool testnet = false;
            public static bool TestInvoke = false;
            public static string? DefaultUserWif { get; set; }
            public static BigInteger calculateManufacturerFinalScore(string projectId, UInt160 manufacturer)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(calculateManufacturerFinalScore),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, manufacturer));
            }


            public static BigInteger calculateManufacturerWeightedVoteResult(string projectId, UInt160 manufacturer)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(calculateManufacturerWeightedVoteResult),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, manufacturer));
            }


            public static bool canActivateManufacturer(string projectId, UInt160 manufacturer)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(canActivateManufacturer),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, manufacturer));
            }


            public static bool canBackerVote(string projectId, UInt160 backer, bool ignoreAlreadyVoted = false)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(canBackerVote),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, backer, ignoreAlreadyVoted));
            }


            public static void ClearVotingTierWeightsAll(string projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(ClearVotingTierWeightsAll),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId));
            }


            public static BigInteger getBackerVoteWeight(string projectId, UInt160 backer)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(getBackerVoteWeight),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, backer));
            }


            public static BigInteger getExternalRating(string projectId, UInt160 manufacturer)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(getExternalRating),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, manufacturer));
            }


            public static byte GetVoterTier(string projectId, string voteId, UInt160 voter)
            {
                return ExecuteContractWithResult<byte>(Address,
                                                         nameof(GetVoterTier),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voteId, voter));
            }


            public static string[] GetVotingTierIds(string projectId)
            {
                return ExecuteContractWithResult<string[]>(Address,
                                                         nameof(GetVotingTierIds),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId)) ?? Array.Empty<string>();
            }


            public static BigInteger GetVotingTierWeight(string projectId, string tierId)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(GetVotingTierWeight),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, tierId));
            }


            public static void handleContractBreachResolution(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleContractBreachResolution),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleDisputeEscalation(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleDisputeEscalation),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleDisputeRejection(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleDisputeRejection),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleDisputeResolution(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleDisputeResolution),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleFraudDisputeResolution(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleFraudDisputeResolution),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleFraudEscalation(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleFraudEscalation),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleGeneralDisputeResolution(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleGeneralDisputeResolution),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleMilestoneDisputeResolution(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleMilestoneDisputeResolution),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleMilestoneEscalation(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleMilestoneEscalation),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handlePaymentDisputeResolution(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handlePaymentDisputeResolution),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handlePaymentEscalation(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handlePaymentEscalation),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static void handleQualityDisputeResolution(string projectId, string disputeId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(handleQualityDisputeResolution),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, disputeId));
            }


            public static bool hasManufacturerUsedMilestoneFunding(string projectId, UInt160 manufacturerId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(hasManufacturerUsedMilestoneFunding),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, manufacturerId));
            }


            public static void RemoveVotingTierWeight(string projectId, string tierId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(RemoveVotingTierWeight),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, tierId));
            }


            public static void setAutoSelectWinner(string projectId, UInt160 backer, UInt160 manufacturer)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setAutoSelectWinner),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, manufacturer));
            }


            public static void SetVoterTier(string projectId, string voteId, UInt160 voter, byte tier)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetVoterTier),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, voteId, voter, tier));
            }


            public static void SetVotingTierWeight(string projectId, string tierId, BigInteger weight)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetVotingTierWeight),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, tierId, weight));
            }


            public static void validateManufacturer(string projectId, UInt160 manufacturer)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(validateManufacturer),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, manufacturer));
            }

        }
    }
}
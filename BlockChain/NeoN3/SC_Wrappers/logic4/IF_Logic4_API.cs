using InnFork.NeoN3;

using System;
using System.Numerics;
using InnFork.Blockchain.NEO3;
using Neo;
using Neo.SmartContract;

namespace InnForkInteropAPI.BlockChain.NeoN3.SC_Wrappers
{


    public partial class InnFork_SCPlatform
    {

        public partial class SCPlatform_Logic4
        {
            public static UInt160 Address = "";
            public static bool TestNet = false;
            public static bool TestInvoke = false;
            public static string? DefaultUserWif { get; set; }
            public static void allocateFunds(string projectId, UInt160 backer, byte prizeFundPartAllocation)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(allocateFunds),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, prizeFundPartAllocation));
            }


            public static void archiveProject(string projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(archiveProject),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId));
            }


            public static byte calculateReliabilityTier(string projectId, UInt160 manufacturer)
            {
                return ExecuteContractWithResult<byte>(Address,
                                                         nameof(calculateReliabilityTier),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, manufacturer));
            }


            public static void calculateWinners(UInt160 FromAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(calculateWinners),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(FromAddress));
            }


            public static bool canSelectWinner(string projectId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(canSelectWinner),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId));
            }


            public static bool canWithdrawFromProject(string projectId, UInt160 backer)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(canWithdrawFromProject),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, backer));
            }


            public static void ClearMilestoneFraudFlags(string projectId, UInt160 ManufacturerCandidate, byte StepNumber)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(ClearMilestoneFraudFlags),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, ManufacturerCandidate, StepNumber));
            }


            public static void ClearMilestoneTemplateParams(string projectId, string templateId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(ClearMilestoneTemplateParams),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, templateId));
            }


            public static void ClearMilestoneVotes(string projectId, UInt160 ManufacturerCandidate, byte StepNumber)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(ClearMilestoneVotes),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, ManufacturerCandidate, StepNumber));
            }


            public static void collectFeeForProjectAuthor(string projectId, UInt160 ProjectCreatorAddressParam)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(collectFeeForProjectAuthor),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, ProjectCreatorAddressParam));
            }


            public static void configureVotingSystem(string projectId, BigInteger minRequiredParticipationPercent, BigInteger minApprovalPercent)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(configureVotingSystem),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, minRequiredParticipationPercent, minApprovalPercent));
            }


            public static void distributeReferralReward(string projectId, UInt160 referrerAddress, BigInteger amount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(distributeReferralReward),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, referrerAddress, amount));
            }


            public static void finalizeWinnerSelectionVoting(string projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(finalizeWinnerSelectionVoting),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId));
            }


            public static UInt160 getFinalDelegateForVoting(string projectId, Dictionary<UInt160, UInt160> delegationMap, UInt160 backerAddress)
            {
                return ExecuteContractWithResult<UInt160>(Address,
                                                         nameof(getFinalDelegateForVoting),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, delegationMap, backerAddress)) ?? UInt160.Zero;
            }


            public static BackerVotesEnum GetMilestoneBackerVote(string projectId, UInt160 ManufacturerCandidate, UInt160 backer, byte StepNumber)
            {
                return ExecuteContractWithResult<BackerVotesEnum>(Address,
                                                         nameof(GetMilestoneBackerVote),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, ManufacturerCandidate, backer, StepNumber));
            }


            public static bool GetMilestoneFraudFlag(string projectId, UInt160 ManufacturerCandidate, UInt160 backer, byte StepNumber)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(GetMilestoneFraudFlag),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, ManufacturerCandidate, backer, StepNumber));
            }


            public static string GetMilestoneTemplateParam(string projectId, string templateId, string key)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(GetMilestoneTemplateParam),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, templateId, key)) ?? string.Empty;
            }


            public static IF_MainGateway.ProjectAccount getProjectAccount(string projectId)
            {
                return ExecuteContractWithResult<IF_MainGateway.ProjectAccount>(Address,
                                                         nameof(getProjectAccount),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId)) ?? new IF_MainGateway.ProjectAccount();
            }


            public static bool isFundingGoalReached(string projectId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(isFundingGoalReached),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId));
            }


            public static bool isManufacturerCandidateRegistered(string projectId, UInt160 manufacturerId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(isManufacturerCandidateRegistered),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, manufacturerId));
            }


            public static void paymentPrizeFundToManufacturer(string projectId, UInt160 winnerAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(paymentPrizeFundToManufacturer),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, winnerAddress));
            }


            public static void processProjectsWinner(string projectId, UInt160 FromAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(processProjectsWinner),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, FromAddress));
            }


            public static void registerReferrer(string projectId, UInt160 backerAddress, UInt160 referrerAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(registerReferrer),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backerAddress, referrerAddress));
            }


            public static void RemoveMilestoneBackerVote(string projectId, UInt160 ManufacturerCandidate, UInt160 backer, byte StepNumber)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(RemoveMilestoneBackerVote),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, ManufacturerCandidate, backer, StepNumber));
            }


            public static void RemoveMilestoneTemplateParam(string projectId, string templateId, string key)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(RemoveMilestoneTemplateParam),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, templateId, key));
            }


            public static void reserveBackerFundsForManufacturer(string projectId, UInt160 backer, UInt160 manufacturer, BigInteger amount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(reserveBackerFundsForManufacturer),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, manufacturer, amount));
            }


            public static void safeRefundAllDonations(string projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(safeRefundAllDonations),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId));
            }


            public static void saveProjectAccountToProjectsAccountStore(string projectId, IF_MainGateway.ProjectAccount project)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(saveProjectAccountToProjectsAccountStore),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, project));
            }


            public static UInt160 selectWinnerManufacturer(string projectId)
            {
                return ExecuteContractWithResult<UInt160>(Address,
                                                         nameof(selectWinnerManufacturer),
                                                         TestNet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId)) ?? UInt160.Zero;
            }


            public static void sendDeadlineNotifications(string projectId, int voteType)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(sendDeadlineNotifications),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, voteType));
            }


            public static void setBillingProductBuyContractCallbackAddressOwner(string projectId, UInt160 manufacturerAddress, UInt160 callbackAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setBillingProductBuyContractCallbackAddressOwner),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, manufacturerAddress, callbackAddress));
            }


            public static void SetMilestoneBackerVote(string projectId, UInt160 backer, UInt160 ManufacturerCandidate, BackerVotesEnum vote, byte StepNumber)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetMilestoneBackerVote),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, ManufacturerCandidate, vote, StepNumber));
            }


            public static void SetMilestoneFraudFlag(string projectId, UInt160 ManufacturerCandidate, UInt160 backer, bool detected, byte StepNumber)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetMilestoneFraudFlag),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, ManufacturerCandidate, backer, detected, StepNumber));
            }


            public static void SetMilestoneTemplateParam(string projectId, string templateId, string key, string value)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetMilestoneTemplateParam),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, templateId, key, value));
            }


            public static void setVoterTier(string projectId, string voteId, UInt160 voterAddress, byte tierLevel)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setVoterTier),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, voteId, voterAddress, tierLevel));
            }


            public static void setVotingReminder(string projectId, int voteType, ulong reminderTimestamp)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setVotingReminder),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, voteType, reminderTimestamp));
            }




            public static void updateManufacturerKPI(string projectId, UInt160 manufacturer, BigInteger deliveryScore, BigInteger qualityScore)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(updateManufacturerKPI),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, manufacturer, deliveryScore, qualityScore));
            }


            public static void updateProjectActivityScore(string projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(updateProjectActivityScore),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId));
            }


            public static void updateVotingTimeParameters(string projectId, string votingTypeAsString, ulong newDeadline)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(updateVotingTimeParameters),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, votingTypeAsString, newDeadline));
            }


            public static void voteLaunchProject(string projectId, UInt160 backer, BackerVotesEnum vote)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(voteLaunchProject),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, vote));
            }


            public void ClearWinnerVoteFraudFlags(string projectId, UInt160 ManufacturerCandidate)
            {
                ExecuteContractWithoutResult(Address,
                                             nameof(ClearWinnerVoteFraudFlags),
                                             TestNet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, ManufacturerCandidate));
            }

            public BackerVotesEnum GetWinnerSelectionVote(string projectId, UInt160 backer, UInt160 ManufacturerCandidate)
            {
                return ExecuteContractWithResult<BackerVotesEnum>(Address,
                                                                  nameof(GetWinnerSelectionVote),
                                                                  TestNet,
                                                                  TestInvoke,
                                                                  DefaultUserWif,
                                                                  BuildParameters(projectId, backer, ManufacturerCandidate));
            }

            public bool GetWinnerVoteFraudFlag(string projectId, UInt160 backer, UInt160 ManufacturerCandidate)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                       nameof(GetWinnerVoteFraudFlag),
                                                       TestNet,
                                                       TestInvoke,
                                                       DefaultUserWif,
                                                       BuildParameters(projectId, backer, ManufacturerCandidate));
            }

            public bool IsMilestoneStepCompletedSuccessfully(string projectId, UInt160 ManufacturerCandidate, byte StepNumber)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                       nameof(IsMilestoneStepCompletedSuccessfully),
                                                       TestNet,
                                                       TestInvoke,
                                                       DefaultUserWif,
                                                       BuildParameters(projectId, ManufacturerCandidate, StepNumber));
            }

            public bool isMilestoneStepStillRunning(string projectId, UInt160 ManufacturerCandidate, byte StepNumber)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                       nameof(isMilestoneStepStillRunning),
                                                       TestNet,
                                                       TestInvoke,
                                                       DefaultUserWif,
                                                       BuildParameters(projectId, ManufacturerCandidate, StepNumber));
            }

            public void SetWinnerSelectionVote(string projectId, UInt160 backer, UInt160 ManufacturerCandidate, BackerVotesEnum vote)
            {
                ExecuteContractWithoutResult(Address,
                                             nameof(SetWinnerSelectionVote),
                                             TestNet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backer, ManufacturerCandidate, vote));
            }

            public void SetWinnerVoteFraudFlag(string projectId, UInt160 backer, UInt160 ManufacturerCandidate, bool detected)
            {
                ExecuteContractWithoutResult(Address,
                                             nameof(SetWinnerVoteFraudFlag),
                                             TestNet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backer, ManufacturerCandidate, detected));
            }
        }
    }
}
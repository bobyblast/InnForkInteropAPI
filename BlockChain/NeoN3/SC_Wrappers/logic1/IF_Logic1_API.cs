using InnFork.NeoN3;

using System;
using System.Numerics;
using InnFork.Blockchain.NEO3;
using Neo;
using Neo.SmartContract;

namespace InnForkInteropAPI.BlockChain.NeoN3.SC_Wrappers;

public partial class InnFork_SCPlatform
{


    public partial class SCPlatform_Logic1
    {
        public static UInt160 Address = "";
        public static bool testnet = false;
        public static bool TestInvoke = false;
        public static string? DefaultUserWif { get; set; }



        public static void applyBackerBanSanctions(string projectId, UInt160 backerAddress, BanReason reason)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(applyBackerBanSanctions),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backerAddress, reason));
        }


        public static void applyManufacturerBanSanctions(string projectId, UInt160 manufacturerAddress, BanReason reason)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(applyManufacturerBanSanctions),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturerAddress, reason));
        }


        public static void applyRejectionPenalty(string projectId, UInt160 initiator, DisputeType disputeType)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(applyRejectionPenalty),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, initiator, disputeType));
        }


        public static void autoProgressMilestones(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(autoProgressMilestones),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId));
        }


        public static void autoRollbackOnDisputeResolution(string projectId, string disputeId, UInt160 manufacturerAddress, byte stepNumber)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(autoRollbackOnDisputeResolution),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, disputeId, manufacturerAddress, stepNumber));
        }


        public static void autoStartNextMilestone(string projectId, UInt160 manufacturer, byte stepNumber)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(autoStartNextMilestone),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturer, stepNumber));
        }


        public static void banBacker(string projectId, UInt160 backerAddress, BanReason reason)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(banBacker),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backerAddress, reason));
        }


        public static void banManufacturer(string projectId, UInt160 manufacturerAddress, BanReason reason)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(banManufacturer),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturerAddress, reason));
        }


        public static void blockBackerFinance(string projectId, UInt160 backerAddress, BanReason banReason, BigInteger amountToBlock)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(blockBackerFinance),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backerAddress, banReason, amountToBlock));
        }


        public static IF_MainGateway.ProjectAccount.CalculatedVoteOutcome calculateInternalVoteOutcome(string projectId, Dictionary<UInt160, BackerVotesEnum> votesMap, string votingType, BigInteger explicitPositiveRaw, BigInteger explicitNegativeRaw, BigInteger explicitAbstainedRaw, BigInteger totalCastedRaw)
        {
            return ExecuteContractWithResult<IF_MainGateway.ProjectAccount.CalculatedVoteOutcome>(Address,
                                                     nameof(calculateInternalVoteOutcome),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, votesMap, votingType, explicitPositiveRaw, explicitNegativeRaw, explicitAbstainedRaw, totalCastedRaw)) ?? new IF_MainGateway.ProjectAccount.CalculatedVoteOutcome();
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


        public static bool canWithdrawReservedForManufacturerFunds(string projectId, UInt160 backer, UInt160 manufacturer)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                     nameof(canWithdrawReservedForManufacturerFunds),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, backer, manufacturer));
        }


        public static void checkAndRollbackExpiredMilestones(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(checkAndRollbackExpiredMilestones),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId));
        }


        public static bool checkMilestoneExpired(string projectId, UInt160 manufacturer, byte checkMilestoneStep)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                     nameof(checkMilestoneExpired),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, manufacturer, checkMilestoneStep));
        }


        public static void clearPenalty(string projectId, UInt160 manufacturerAddress)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(clearPenalty),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturerAddress));
        }


        public static IF_MainGateway.MilestoneTemplate createDefaultTemplate(byte stepNumber, IF_MainGateway.ProjectAccount project)
        {
            return ExecuteContractWithResult<IF_MainGateway.MilestoneTemplate>(Address,
                                                     nameof(createDefaultTemplate),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(stepNumber, project)) ?? new IF_MainGateway.MilestoneTemplate();
        }


        public static IF_MainGateway.MilestoneTemplate createDevelopmentTemplate(byte stepNumber)
        {
            return ExecuteContractWithResult<IF_MainGateway.MilestoneTemplate>(Address,
                                                     nameof(createDevelopmentTemplate),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(stepNumber)) ?? new IF_MainGateway.MilestoneTemplate();
        }


        public static IF_MainGateway.MilestoneTemplate createManufacturingTemplate(byte stepNumber)
        {
            return ExecuteContractWithResult<IF_MainGateway.MilestoneTemplate>(Address,
                                                     nameof(createManufacturingTemplate),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(stepNumber)) ?? new IF_MainGateway.MilestoneTemplate();
        }


        public static void createMilestoneRequest(string projectId, UInt160 manufacturer, byte stepNumber, string name, string description, BigInteger requestedAmount, ulong deadline, ulong votingDuration, BigInteger minimumVotes)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(createMilestoneRequest),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturer, stepNumber, name, description, requestedAmount, deadline, votingDuration, minimumVotes));
        }


        public static IF_MainGateway.MilestoneTemplate createResearchTemplate(byte stepNumber)
        {
            return ExecuteContractWithResult<IF_MainGateway.MilestoneTemplate>(Address,
                                                     nameof(createResearchTemplate),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(stepNumber)) ?? new IF_MainGateway.MilestoneTemplate();
        }


        public static IF_MainGateway.MilestoneTemplate createTestingTemplate(byte stepNumber)
        {
            return ExecuteContractWithResult<IF_MainGateway.MilestoneTemplate>(Address,
                                                     nameof(createTestingTemplate),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(stepNumber)) ?? new IF_MainGateway.MilestoneTemplate();
        }




        public static void finalizeProjectUpdateVoting(string projectId, string updateId)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(finalizeProjectUpdateVoting),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, updateId));
        }


        public static bool getBackerVoteStatusByComplexKey_BackerAddress_UpdateHash(string projectId, string UpdateHash, UInt160 backerAddress)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                     nameof(getBackerVoteStatusByComplexKey_BackerAddress_UpdateHash),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, UpdateHash, backerAddress));
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


        public static UInt160 getFinalDelegateForVoting(string projectId, Dictionary<Dictionary<string, UInt160>, UInt160> delegationMap, UInt160 backerAddress)
        {
            return ExecuteContractWithResult<UInt160>(Address,
                                                     nameof(getFinalDelegateForVoting),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, delegationMap, backerAddress)) ?? UInt160.Zero;
        }


        public static byte getLatestMilestoneStepByManufacturer(string projectId, UInt160 manufacturer)
        {
            return ExecuteContractWithResult<byte>(Address,
                                                     nameof(getLatestMilestoneStepByManufacturer),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, manufacturer));
        }


        public static ulong getManufacturerLastPenaltyTime(string projectId, UInt160 manufacturerAddress)
        {
            return ExecuteContractWithResult<ulong>(Address,
                                                     nameof(getManufacturerLastPenaltyTime),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, manufacturerAddress));
        }


        public static BigInteger getManufacturerTotalPenalties(string projectId, UInt160 manufacturerAddress)
        {
            return ExecuteContractWithResult<BigInteger>(Address,
                                                     nameof(getManufacturerTotalPenalties),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, manufacturerAddress));
        }


        public static bool getMilestoneCompletionVotingStatus(string projectId, UInt160 manufacturerAddress, byte milestoneStep)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                     nameof(getMilestoneCompletionVotingStatus),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, manufacturerAddress, milestoneStep));
        }


        public static IF_MainGateway.MilestoneTemplate getMilestoneTemplate(IF_MainGateway.ProjectAccount project, UInt160 manufacturer, byte stepNumber, string templateType = "default")
        {
            return ExecuteContractWithResult<IF_MainGateway.MilestoneTemplate>(Address,
                                                     nameof(getMilestoneTemplate),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(project, manufacturer, stepNumber, templateType)) ?? new IF_MainGateway.MilestoneTemplate();
        }




        public static BanReason getParticipantBanReason(string projectId, UInt160 participantAddress)
        {
            return ExecuteContractWithResult<BanReason>(Address,
                                                     nameof(getParticipantBanReason),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, participantAddress));
        }


        public static IF_MainGateway.ProjectAccount getProjectAccount(string projectId)
        {
            return ExecuteContractWithResult<IF_MainGateway.ProjectAccount>(Address,
                                                     nameof(getProjectAccount),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId)) ?? new IF_MainGateway.ProjectAccount();
        }


        public static BigInteger getRiskScoreForBanReason(BanReason reason)
        {
            return ExecuteContractWithResult<BigInteger>(Address,
                                                     nameof(getRiskScoreForBanReason),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(reason));
        }


        public static string[] getSerializedGlobalProjectsAccountsList()
        {
            return ExecuteContractWithResult<string[]>(Address,
                                                     nameof(getSerializedGlobalProjectsAccountsList),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters()) ?? Array.Empty<string>();
        }


        public static void handleGeneralEscalation(string projectId, string disputeId)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(handleGeneralEscalation),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, disputeId));
        }


        public static void handleInvestorRollback(string projectId, UInt160 manufacturerAddress, BigInteger rolledBackAmount)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(handleInvestorRollback),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturerAddress, rolledBackAmount));
        }


        public static void imposePenalty(string projectId, UInt160 manufacturerAddress, BigInteger penaltyAmount)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(imposePenalty),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturerAddress, penaltyAmount));
        }


        public static bool isEligibleToVote(string projectId, UInt160 backerAddress)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                     nameof(isEligibleToVote),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, backerAddress));
        }


        public static bool isProjectOpen(string projectId)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                     nameof(isProjectOpen),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId));
        }


        public static void lockFunds(string projectId, BigInteger amount)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(lockFunds),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, amount));
        }


        public static void processAutomaticUnbans(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(processAutomaticUnbans),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId));
        }


        public static void processMilestoneConditionalFunding(UInt160 manufacturerAddress, string projectId, byte stepNumber, BigInteger conditionalAmount)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(processMilestoneConditionalFunding),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(manufacturerAddress, projectId, stepNumber, conditionalAmount));
        }


        public static void processMilestoneFunding(string projectId, UInt160 manufacturer, byte stepNumber)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(processMilestoneFunding),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturer, stepNumber));
        }




        public static void removeManufacturerCandidate(string projectId, UInt160 ManufacturerAddress)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(removeManufacturerCandidate),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, ManufacturerAddress));
        }


        public static void removeManufacturerCandidateFromAllProjects(UInt160 manufacturerAddress)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(removeManufacturerCandidateFromAllProjects),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(manufacturerAddress));
        }


        public static void requestMilestoneCompletion(UInt160 manufacturerAddress, string projectId, byte stepNumber, string completionProof, byte[] verificationData)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(requestMilestoneCompletion),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(manufacturerAddress, projectId, stepNumber, completionProof, verificationData));
        }


        public static void requireMilestoneVerification(string projectId, string milestoneId, bool required)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(requireMilestoneVerification),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, milestoneId, required));
        }


        public static void resetProjectWinnerStatus(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(resetProjectWinnerStatus),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId));
        }


        public static void returnAllReservedInvestmentsFromCandidate(string projectId, UInt160 manufacturerAddress)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(returnAllReservedInvestmentsFromCandidate),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturerAddress));
        }


        public static void returnInvestmentToInvestor(string projectId, UInt160 investorAddress, UInt160 manufacturerAddress, BigInteger amount)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(returnInvestmentToInvestor),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, investorAddress, manufacturerAddress, amount));
        }


        public static void rollbackMilestoneFunding(string projectId, UInt160 manufacturerAddress, byte stepNumber, string rollbackReason)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(rollbackMilestoneFunding),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturerAddress, stepNumber, rollbackReason));
        }


        public static void saveProjectAccountToProjectsAccountStore(string projectId, IF_MainGateway.ProjectAccount project)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(saveProjectAccountToProjectsAccountStore),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, project));
        }


        public static void setBackerVoteStatusByComplexKey_BackerAddress_UpdateHash(string projectId, string UpdateHash, UInt160 backerAddress, BackerVotesEnum BackerVote)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(setBackerVoteStatusByComplexKey_BackerAddress_UpdateHash),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, UpdateHash, backerAddress, BackerVote));
        }


        public static bool shouldAutoStartNextMilestone(string projectId, UInt160 manufacturer, byte stepNumber)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                     nameof(shouldAutoStartNextMilestone),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(projectId, manufacturer, stepNumber));
        }


        public static void startMilestoneVoting(string projectId, UInt160 manufacturer, byte stepNumber, ulong duration, BigInteger minimumVotes)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(startMilestoneVoting),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturer, stepNumber, duration, minimumVotes));
        }


        public static void submitMilestoneVerification(UInt160 validatorAddress, string projectId, UInt160 manufacturerAddress, byte stepNumber, bool isVerified, string verificationReport)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(submitMilestoneVerification),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(validatorAddress, projectId, manufacturerAddress, stepNumber, isVerified, verificationReport));
        }


        public static void unBlockBackerFinance(string projectId, UInt160 backerAddress, BigInteger amountToRestore)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(unBlockBackerFinance),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backerAddress, amountToRestore));
        }


        public static void unlockFunds(string projectId, BigInteger amount)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(unlockFunds),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, amount));
        }



        public static void updateManufacturerReputationAfterPenalty(string projectId, UInt160 manufacturerAddress, BigInteger penaltyAmount)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(updateManufacturerReputationAfterPenalty),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, manufacturerAddress, penaltyAmount));
        }


        public static void verifyMilestone(string projectId, string milestoneId, bool verified)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(verifyMilestone),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, milestoneId, verified));
        }


        public static void voteAbstain(string projectId, UInt160 voterAddress, int voteType, string updateHash)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(voteAbstain),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, voterAddress, voteType, updateHash));
        }


        public static void voteForManufacturerWinner(string projectId, UInt160 backer, UInt160 manufacturer, BackerVotesEnum vote)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(voteForManufacturerWinner),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backer, manufacturer, vote));
        }


        public static void voteProjectUpdate(string projectId, UInt160 backer, string updateId, BackerVotesEnum vote)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(voteProjectUpdate),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backer, updateId, vote));
        }


        public static void voteToMilestoneCompletionStep(string projectId, UInt160 backer, UInt160 manufacturerCandidate, byte stepNumber, BackerVotesEnum vote)
        {
            ExecuteContractWithoutResult(Address,
                                             nameof(voteToMilestoneCompletionStep),
                                             testnet,
                                             TestInvoke,
                                             DefaultUserWif,
                                             BuildParameters(projectId, backer, manufacturerCandidate, stepNumber, vote));
        }



    }
}
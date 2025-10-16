using System;
using System.Numerics;
using InnFork.Blockchain.NEO3;
using Neo;
using Neo.SmartContract;


namespace InnForkInteropAPI.BlockChain.NeoN3.SC_Wrappers;



public partial class InnFork_SCPlatform
{


    public class InvestingContract
    {
        public static UInt160 Address = "";
        public int version;

        public static bool testnet = false;
        public static bool TestInvoke = false;
        public static string? DefaultUserWif { get; set; }

        public static void collectFeeForProjectAuthor(string projectId, UInt160 ProjectCreatorAddressParam)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(collectFeeForProjectAuthor),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId, ProjectCreatorAddressParam));
        }



        public static void autoFinalizeAllExpiredVotings(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(autoFinalizeAllExpiredVotings),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static string callBackerAccountFacade(string operation, object[] args)
        {
            return ExecuteContractWithResult<string>(Address,
                                                     nameof(callBackerAccountFacade),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(operation, args ?? Array.Empty<object>())) ?? string.Empty;
        }

        public static string callInvestorAccountFacade(string operation, object[] args)
        {
            return ExecuteContractWithResult<string>(Address,
                                                     nameof(callInvestorAccountFacade),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(operation, args ?? Array.Empty<object>())) ?? string.Empty;
        }

        public static string callManufacturerAccountFacade(string operation, object[] args)
        {
            return ExecuteContractWithResult<string>(Address,
                                                     nameof(callManufacturerAccountFacade),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(operation, args ?? Array.Empty<object>())) ?? string.Empty;
        }

        public static string callProjectCreatorAccountFacade(string operation, object[] args)
        {
            return ExecuteContractWithResult<string>(Address,
                                                     nameof(callProjectCreatorAccountFacade),
                                                     testnet,
                                                     TestInvoke,
                                                     DefaultUserWif,
                                                     BuildParameters(operation, args ?? Array.Empty<object>())) ?? string.Empty;
        }

        public static bool canWithdrawInvestment(string projectId, UInt160 investorAddress, UInt160 manufacturerAddress)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                   nameof(canWithdrawInvestment),
                                                   testnet,
                                                   TestInvoke,
                                                   DefaultUserWif,
                                                   BuildParameters(projectId, investorAddress, manufacturerAddress));
        }

        public static void checkAllMilestoneVotings(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(checkAllMilestoneVotings),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static bool checkAndFinalizeMilestoneVoting(string projectId, UInt160 manufacturer, byte stepNumber)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                   nameof(checkAndFinalizeMilestoneVoting),
                                                   testnet,
                                                   TestInvoke,
                                                   DefaultUserWif,
                                                   BuildParameters(projectId, manufacturer, stepNumber));
        }


        public static void donateToProjectManufacturerAsInvestor(string projectId, UInt160 investorAddress, UInt160 manufacturerAddress, BigInteger amount)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(donateToProjectManufacturerAsInvestor),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId, investorAddress, manufacturerAddress, amount));
        }

        public static void finalizeFundraisingIncreaseVoting(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(finalizeFundraisingIncreaseVoting),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static void finalizeFundraisingVoting(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(finalizeFundraisingVoting),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static void finalizeLaunchVoting(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(finalizeLaunchVoting),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static void finalizeManagementTransferVoting(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(finalizeManagementTransferVoting),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static void finalizePauseVoting(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(finalizePauseVoting),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static void finalizeSuccessfulClosureVoting(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(finalizeSuccessfulClosureVoting),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static void finalizeTerminationWithRefundVoting(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(finalizeTerminationWithRefundVoting),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static void finalizeWinnerSelectionVoting(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(finalizeWinnerSelectionVoting),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static string[] getAllProjectsIds()
        {
            return ExecuteContractWithResult<string[]>(Address,
                                                        nameof(getAllProjectsIds),
                                                        testnet,
                                                        TestInvoke,
                                                        DefaultUserWif,
                                                        BuildParameters()) ?? Array.Empty<string>();
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

        public static void investToManufacturer(string projectId, UInt160 investorAddress, UInt160 manufacturerAddress, BigInteger amount)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(investToManufacturer),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId, investorAddress, manufacturerAddress, amount));
        }

        public static bool isFundingGoalReached(string projectId)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                   nameof(isFundingGoalReached),
                                                   testnet,
                                                   TestInvoke,
                                                   DefaultUserWif,
                                                   BuildParameters(projectId));
        }

        public static bool isMilestoneVotingCompleteSuccess(UInt160 manufacturer, string projectId, byte stepNumber)
        {
            return ExecuteContractWithResult<bool>(Address,
                                                   nameof(isMilestoneVotingCompleteSuccess),
                                                   testnet,
                                                   TestInvoke,
                                                   DefaultUserWif,
                                                   BuildParameters(manufacturer, projectId, stepNumber));
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

        public static void paymentPrizeFundToManufacturer(string projectId, UInt160 winnerAddress)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(paymentPrizeFundToManufacturer),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId, winnerAddress));
        }

        public static void refundOfferDonate(UInt160 backer, string offerSha256Id, BigInteger amount)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(refundOfferDonate),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(backer, offerSha256Id, amount));
        }

        public static void registerNewManufacturerCandidate(UInt160 ManufacturerAddress, string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(registerNewManufacturerCandidate),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(ManufacturerAddress, projectId));
        }

        public static void safeRefundAllDonations(string projectId)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(safeRefundAllDonations),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId));
        }

        public static void withdrawInvestment(string projectId, UInt160 investorAddress, UInt160 manufacturerAddress)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(withdrawInvestment),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId, investorAddress, manufacturerAddress));
        }

        public static void withdrawInvestorProfit(string projectId, UInt160 investorAddress, UInt160 manufacturerAddress)
        {
            ExecuteContractWithoutResult(Address,
                                         nameof(withdrawInvestorProfit),
                                         testnet,
                                         TestInvoke,
                                         DefaultUserWif,
                                         BuildParameters(projectId, investorAddress, manufacturerAddress));
        }
    }

}
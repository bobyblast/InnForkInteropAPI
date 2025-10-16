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


        public partial class SCPlatform_Logic2
        {
            public static UInt160 Address = "";
            public static bool testnet = false;
            public static bool TestInvoke = false;
            public static string? DefaultUserWif { get; set; }

            public static bool analyzeAccountConnections(string projectId, UInt160 voterAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(analyzeAccountConnections),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress));
            }


            public static bool analyzeVotingFrequency(string projectId, UInt160 voterAddress, ulong currentVoteTime)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(analyzeVotingFrequency),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress, currentVoteTime));
            }


            public static void applyBackerBanSanctions(string projectId, UInt160 backerAddress, BanReason reason)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(applyBackerBanSanctions),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backerAddress, reason));
            }


            public static void applyFraudPenalties(string projectId, UInt160 voterAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(applyFraudPenalties),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, voterAddress));
            }


            public static void applyGradedFraudPenalties(string projectId, UInt160 voterAddress, FraudType fraudType)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(applyGradedFraudPenalties),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, voterAddress, fraudType));
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


            public static void blockBackerFinance(string projectId, UInt160 backerAddress, BanReason banReason, BigInteger amountToBlock)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(blockBackerFinance),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backerAddress, banReason, amountToBlock));
            }


            public static bool checkIdenticalVotingPatterns(string projectId, UInt160 voterAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(checkIdenticalVotingPatterns),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress));
            }


            public static bool detectBalanceManipulationFraud(string projectId, UInt160 voterAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectBalanceManipulationFraud),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress));
            }


            public static bool detectCircularTransactions(string projectId, UInt160 voterAddress, IF_MainGateway.BackerAccount backerAccount)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectCircularTransactions),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress, backerAccount));
            }


            public static bool detectCollusionPattern(string projectId, UInt160 voterAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectCollusionPattern),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress));
            }


            public static bool detectRapidVotingPattern(string projectId, UInt160 voterAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectRapidVotingPattern),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress));
            }


            public static bool detectSybilAttack(string projectId, UInt160 voterAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectSybilAttack),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress));
            }


            public static bool detectTemporalVotingAnomalies(string projectId, UInt160 voterAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectTemporalVotingAnomalies),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress));
            }


            public static bool detectUniformVotingIntervals(string projectId, UInt160 voterAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectUniformVotingIntervals),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress));
            }


            public static bool detectVoteSwitchingFraud(string projectId, UInt160 voterAddress, string votingType)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectVoteSwitchingFraud),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, voterAddress, votingType));
            }


            public static bool detectVotingFraud(UInt160 voterAddress, string projectId, string votingType)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(detectVotingFraud),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(voterAddress, projectId, votingType));
            }


            public static void distributeConsentedFunds(string projectId, UInt160 manufacturer)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(distributeConsentedFunds),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, manufacturer));
            }


            public static void distributeManufacturerProfit(string projectId, UInt160 manufacturerAddress, BigInteger profitAmount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(distributeManufacturerProfit),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, manufacturerAddress, profitAmount));
            }


            public static bool getBackerAutoConsent_ToUsePrizeFundToMilestoneFunding(string projectId, UInt160 backer)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(getBackerAutoConsent_ToUsePrizeFundToMilestoneFunding),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, backer));
            }


            public static BigInteger getBackerPrizeFundDonation(string projectId, UInt160 backer)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(getBackerPrizeFundDonation),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, backer));
            }


            public static BigInteger getBackerReservedAmountForManufacturer(string projectId, UInt160 backer, UInt160 manufacturerCandidate)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(getBackerReservedAmountForManufacturer),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, backer, manufacturerCandidate));
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


            public static bool isBackerDonatedToPrizeFund(string projectId, UInt160 backer)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(isBackerDonatedToPrizeFund),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, backer));
            }


            public static bool isParticipantBanned(string projectId, UInt160 participantAddress)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(isParticipantBanned),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, participantAddress));
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


            public static void moneyBackFromProjectToBackerAccount(string projectId, UInt160 backerAddress, BigInteger amount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(moneyBackFromProjectToBackerAccount),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backerAddress, amount));
            }


            public static void processBackerPrizeFundDonation(string projectId, UInt160 backerAddress, BigInteger amount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(processBackerPrizeFundDonation),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backerAddress, amount));
            }


            public static void proposeProfitShare(string projectId, UInt160 investorAddress, UInt160 manufacturerAddress, BigInteger profitSharePercentage)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(proposeProfitShare),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, investorAddress, manufacturerAddress, profitSharePercentage));
            }


            public static void ReleaseAcquireLock()
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(ReleaseAcquireLock),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters());
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


            public static void setBackerAutoConsent_ToUsePrizeFundToMilestoneFunding(string projectId, UInt160 backer, bool consent)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setBackerAutoConsent_ToUsePrizeFundToMilestoneFunding),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, backer, consent));
            }


            public static void setManufacturerMinInvestment(string projectId, UInt160 manufacturerId, BigInteger amount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setManufacturerMinInvestment),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId, manufacturerId, amount));
            }


            public static void setProjectOfferShortJsonToExistingProject(string offerShortJson, string ProjectOfferId_Sha256, byte[] projectIdSha256)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setProjectOfferShortJsonToExistingProject),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(offerShortJson, ProjectOfferId_Sha256, projectIdSha256));
            }



            public static bool validateVotingIntegrity(string projectId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(validateVotingIntegrity),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId));
            }

        }
    }
}
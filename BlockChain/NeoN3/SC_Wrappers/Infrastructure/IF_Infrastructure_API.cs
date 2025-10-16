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


        public partial class SCPlatform_Infrastructure
        {
            public static UInt160 Address = "";
            public static bool testnet = false;
            public static bool TestInvoke = false;
            public static string? DefaultUserWif { get; set; }


            public static void autoSetToPause(string projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(autoSetToPause),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId));
            }


            public static BigInteger calculateTotalReservedForOffer(string offerId)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(calculateTotalReservedForOffer),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(offerId));
            }


            public static int getContractVersion()
            {
                return ExecuteContractWithResult<int>(Address,
                                                         nameof(getContractVersion),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters());
            }


            public static string getContractVersionChangelog()
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(getContractVersionChangelog),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters()) ?? string.Empty;
            }


            public static int getCountOfSelectedMapName(string MapName)
            {
                return ExecuteContractWithResult<int>(Address,
                                                         nameof(getCountOfSelectedMapName),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(MapName));
            }


            public static int getCountOf_Sha256Offers()
            {
                return ExecuteContractWithResult<int>(Address,
                                                         nameof(getCountOf_Sha256Offers),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters());
            }


            public static UInt160 GetModeratorContractHash()
            {
                return ExecuteContractWithResult<UInt160>(Address,
                                                         nameof(GetModeratorContractHash),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters()) ?? UInt160.Zero;
            }


            public static bool getProjectStatusAsBoolean(string ProjectId, ProjectStateRequest statusType)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(getProjectStatusAsBoolean),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(ProjectId, statusType));
            }


            public static string[] getProjectStatuses(string projectId)
            {
                return ExecuteContractWithResult<string[]>(Address,
                                                         nameof(getProjectStatuses),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId)) ?? Array.Empty<string>();
            }


            public static string[] getSerializedGlobalProjectsPackagesList()
            {
                return ExecuteContractWithResult<string[]>(Address,
                                                         nameof(getSerializedGlobalProjectsPackagesList),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters()) ?? Array.Empty<string>();
            }


            public static UInt160 GetStateContractHash()
            {
                return ExecuteContractWithResult<UInt160>(Address,
                                                         nameof(GetStateContractHash),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters()) ?? UInt160.Zero;
            }


            public static BigInteger getTotalReservedDonatesToOffer(string offerSha256Id)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(getTotalReservedDonatesToOffer),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(offerSha256Id));
            }


            public static void ImportNewProjectSettings(string jsonSettings)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(ImportNewProjectSettings),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(jsonSettings));
            }


            public static bool isProjectRegistered(string projectId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(isProjectRegistered),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId));
            }


            public static void OverrideModeratorContract(UInt160 newContract)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(OverrideModeratorContract),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(newContract));
            }


            public static void OverrideStateContract(UInt160 newHash)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(OverrideStateContract),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(newHash));
            }


            public static string registerProjectEx(UInt160 ProjectCreatorId, string AuthorPubKey, string ProjectJson, byte[] signatureOfJson, byte[] pubKeyOfJsonSigner, string ProjectOfferId_Sha256, bool bool_StoreOffer, string projectOfferJson, BigInteger WinnerDollars_GoalFund)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(registerProjectEx),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(ProjectCreatorId, AuthorPubKey, ProjectJson, signatureOfJson, pubKeyOfJsonSigner, ProjectOfferId_Sha256, bool_StoreOffer, projectOfferJson, WinnerDollars_GoalFund)) ?? string.Empty;
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


            public static void removeOffer(string offerSha256Id)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(removeOffer),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(offerSha256Id));
            }


            /*            public static void removeProjectFromGlobalProjectsLists(IF_MainGateway.ProjectAccount Account)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(removeProjectFromGlobalProjectsLists),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(Account));
            }
*/

            public static void removeProjectFromGlobalProjectsListsByProjectId(string projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(removeProjectFromGlobalProjectsListsByProjectId),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId));
            }


            public static bool removeProjectOfferByTimeLine(UInt160 projectCreatorAddress, string offerSha256Id)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(removeProjectOfferByTimeLine),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectCreatorAddress, offerSha256Id));
            }


            public static void resumeProject(string projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(resumeProject),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(projectId));
            }


            /*            public static void saveProjectAccountToProjectsAccountStore(string ProjectAddress, IF_MainGateway.ProjectAccount project)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(saveProjectAccountToProjectsAccountStore),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(ProjectAddress, project));
            }


                        public static void saveProjectToProjectsAccountStore(IF_MainGateway.ProjectPackage package)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(saveProjectToProjectsAccountStore),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(package));
            }


                        public static void saveToGlobalProjectsLists(IF_MainGateway.ProjectAccount Account)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(saveToGlobalProjectsLists),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(Account));
            }

            */
            public static void SetModeratorContractHash(UInt160 newContract)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetModeratorContractHash),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(newContract));
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


            public static void SetStateContractHash(UInt160 newContract)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(SetStateContractHash),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(newContract));
            }


            public static string storeProjectOfferMetaData(UInt160 projectCreatorAddress, string AuthorPubKey, string JsonOfferDoc, byte[] signatureOfJson, byte[] pubKeyOfJsonSigner)
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(storeProjectOfferMetaData),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectCreatorAddress, AuthorPubKey, JsonOfferDoc, signatureOfJson, pubKeyOfJsonSigner)) ?? string.Empty;
            }


            public static void updateContractVersion(int newVersion, string changelogHash)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(updateContractVersion),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(newVersion, changelogHash));
            }


            /*           public static void updateProjectPackage(IF_MainGateway.ProjectPackage package, byte[] projectId)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(updateProjectPackage),
                                                 TestNet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(package, projectId));
            }
*/

            public static bool validateProjectIsLiving(string projectId)
            {
                return ExecuteContractWithResult<bool>(Address,
                                                         nameof(validateProjectIsLiving),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId));
            }


            enum Network { TestNet, MainNet }


        }

    }
}
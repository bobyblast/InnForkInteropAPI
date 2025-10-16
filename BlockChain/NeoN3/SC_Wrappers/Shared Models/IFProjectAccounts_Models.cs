

using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.ComponentModel;
using System.Numerics;
using static InnFork.NeoN3.IF_MainGateway;



namespace InnFork.NeoN3;




// ��� ������ ���� � ���������� ����������� �� ������� ��� �������� 

public partial class IF_MainGateway
{
    public class ProjectOfferPackage
    {
        public UInt160 AuthorAccount;
        public string ProjectOfferSha256Id;
        public byte[] AuthorPubKey = new byte[33]; // ������������� ��� ��������� null
        public byte[] Signature = new byte[64];
    }



    // ����� ������ ProjectAccount �� ����������� � �������������� ���������� ����������� �� �������������� ��������
    //[Serializable]
    public partial class ProjectAccount
    {
        // �������� ��� ��������������� ��������� ������ ������ IF_MainGateway ��� ProjectAccount
        //[Serializable]
        public class CalculatedVoteOutcome
        {
            public CalculatedVoteOutcome()
            {
            }

            public BigInteger FinalPositiveVotes { get; internal set; } = 0;       // �������� ������ "��" ����� ���������� ������
            public BigInteger FinalNegativeVotes { get; internal set; } = 0;       // �������� ������ "������"
            public BigInteger FinalAbstainedVotes { get; internal set; } = 0;      // �������� ������ "�����������" (����� + �����������)
            public BigInteger TotalEligibleVoters { get; internal set; } = 0;      // ����� ����� ������� ����� ������
            public BigInteger TotalParticipatedOrAssigned { get; internal set; } = 0; // ����� ����� ���������� ��������������� + ����������� ��������������
            public bool IsSuccessful { get; internal set; } = false;                  // ���� �����������
        }


        //��������: ������� ������� �������������, ������� �������������� ����� ����������(��������, 30%).
        //�������� �� ���������: 30
        public BigInteger Profit_Share_Percentage = 30; // ����������� �������� �� ���������


        // ����� ����������� ��� ��������� ����� �����������
        public bool IsLaunchVotingFinalized { get; internal set; } = false;
        public bool IsFundraisingIncreaseVotingFinalized { get; internal set; } = false; // ���� ����� ��������� ����� �����������
        public bool IsFundraisingCompletionVotingFinalized { get; internal set; } = false;
        public bool IsPauseResumeVotingFinalized { get; internal set; } = false;
        public bool IsTerminationWithRefundVotingFinalized { get; internal set; } = false;
        public bool IsTerminationWithRefundVotingCompleted { get; internal set; } // 
        public bool IsSuccessfulClosureVotingFinalized { get; internal set; } = false;


        public BigInteger ManagementTransferPositiveVotesCount { get; internal set; } = 0;
        public BigInteger ManagementTransferNegativeVotesCount { get; internal set; } = 0;
        public BigInteger ManagementTransferAbstainedVotesCount { get; internal set; } = 0;
        public BigInteger ManagementTransferTotalVotesCount { get; internal set; } = 0;

        public ulong ManagementTransferVotingDeadline { get; internal set; }

        public bool IsManagementTransferVotingFinalized { get; internal set; } = false;
        public bool IsManagementTransferVotingCompleted { get; internal set; } = false; // ����� ���������� � IsManagementTransferVotingFinalized


    }

    public partial class ProjectAccount // ����� ������ � ����������� � �������� ���������� ����������� �� �������
    {

        public bool AutoAssignVoicelessToAbstain { get; internal set; } // <--- ���� ��� ��������������� ����� ���������� ��� ��������������
        public bool AutoAbstainVoteAsSupport { get; internal set; } // <--- ���� ��� ��������������� ����� �������������� ������� ��� ��������������


        public bool DirectInnForkProjectManagement { get; set; } = false; // ���������� �������� �������� ����� InnFork Gateway



        public ProjectAccount(UInt160 AuthorAddress, string uint256_ProjectId)
        {
            projectId = uint256_ProjectId;

            ProjectCreatorAddress = AuthorAddress;

            LaunchVotingDeadline = Runtime.Time + 1000000;
            FundraisingCompletionVotingDeadline = Runtime.Time + 1000000;
            SuccessfulClosureVotingDeadline = Runtime.Time + 1000000;
            PauseResumeVotingDeadline = Runtime.Time + 1000000;
            ManufacturerSelectionVotingDeadline = Runtime.Time + 1000000;
            TerminationWithRefundVotingDeadline = Runtime.Time + 1000000;
            MilestoneCompletionVotingDeadline = Runtime.Time + 1000000;
            FundraisingVotingDeadline = Runtime.Time + 1000000;

        }
        public ProjectAccount()
        {
        }

        public string ProjectName { get; internal set; }


        public bool AutoFinishExpiredVotings { get; internal set; } = true; // <--- ������������� ��������� ����������� � �������� ������

        public bool AutoPauseEnabled { get; internal set; } = true; //  
        public bool AutoDistributeConsentedFunds { get; internal set; } = true; // 
        public bool EnableFraudDetection { get; internal set; } = true;
        public BigInteger WithdrawalTimeout { get; internal set; } = 100000; // 
        public byte DefaultPrizeFundAllocation { get; internal set; }

        // public BigInteger MinManufacturerInvestment { get; internal set; }
        //     public string Description { get; internal set; }



        public byte MaxMilestoneSteps { get; internal set; }
        public MilestoneFundingType DefaultFundingType { get; internal set; } = MilestoneFundingType.Before; // ��� �������������� ������ (��, �����, ����������)


        public int MaxManufacturers { get; internal set; } = 5; // ������������ ���������� ��������������-����������
        public bool AllowSteppedFinancing { get; internal set; } = true; // ��������� �� ��������� �������������� ���������������




        public BigInteger MinimumVotesRequired { get; internal set; }



        ///////////////////////////////////////////////DEADLINES/////////////////////////////////////////////

        public int CurrentVoteType { get; internal set; }

        public ulong DefaultVotingDuration { get; internal set; }

        public ulong FundraisingCompletionVotingDeadline { get; internal set; }
        public ulong SuccessfulClosureVotingDeadline { get; internal set; }
        public ulong PauseResumeVotingDeadline { get; internal set; }
        public ulong ManufacturerSelectionVotingDeadline { get; internal set; }
        public ulong TerminationWithRefundVotingDeadline { get; internal set; }
        public ulong MilestoneCompletionVotingDeadline { get; internal set; }




        /*������ ��� Project Creator
         ������: ��� Project Creator �������� ������� �� �������?
         ��������:
         ��������� ���������:
         �������� � ������� ������(fee).
         �������(��������, 1%) �� ������� ��������� ��������.
         ������������� ����� ��� �������� ������ �������.
         �����������:
         ������ ������ ������� �������� � ��������������, ������������� ��� �������� �������.
         ����������:*/

        // ������� �������� ��� Project Creator (��������, 1%)
        public BigInteger CreatorFeePercentage { get; set; } = 1;
        // ������������� ����� �� �������� ����� �������
        public BigInteger CreatorFixedReward { get; set; } = 1000;

        public bool AuthorSuccessGetFee { get; internal set; } = false;
        public bool InnForkFeeCollected { get; internal set; } = false;
        public UInt160 ProjectCreatorAddress { get; internal set; }




        // ����� ����������� �������
        public ulong RegistrationTime { get; set; } = Runtime.Time;
        // ����� ��������� ���������� �������
        public ulong LastActivityTime { get; set; } = 0;

        // ����-��� ��� ������ ������� (��������, 6 ������� � ��������)
        public readonly BigInteger WITHDRAWAL_TIMEOUT = 15552000;

        // �������������� ���� ��� ������������ ������ �����������
        public ulong LaunchVotingDeadline { get; set; }
        public ulong FundraisingVotingDeadline { get; set; }

        public ulong FundraisingDeadline { get; set; } = 0;   // ������� ��� ����� �������        // ������� ��� ������ �������������
        public ulong ManufacturerSelectionDeadline { get; set; } = 0;    // ������� ��� ������ �������������



        // �������� ���� ������� (��������, �� ����������������� �� ���������������)
        public BigInteger FLMUSD_PrizeFundBalance { get; set; } = 0; // <--- ������� ����� ��������� �����
        public BigInteger FLMUSD_PrizeFundGoal { get; set; } = 100000; // <--- ����� � �������� ������� ���� �������
        public BigInteger FLMUSD_TotalProjectBalance { get; set; } = 0; // <--- ������� ����� ����� ������� (������� �������� ���� � ��������, ����������������� �� ���������������)



        public UInt160 ManufacturerWinnerAddress { get; set; } = UInt160.Zero; // <--- ����� �������������-����������
        public string ManufacturerWinnerProductId_Sha256 { get; set; } = string.Empty; // <--- ��� �������� �������������-����������
        public string ManufacturerWinnerProductDescription_NeoFS_Address { get; set; } = string.Empty; // <--- ����� �������� �������� �������������-���������� � NeoFS

        public const int MAX_MILESTONES = 10;         // ������������ ����� ������
        public const int MAX_MANUFACTURERS = 5;       // ������������ ����� ��������������


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        /*       
                 ��������, ����������������� ��� ������ ��������������, ��������� ������������.
                 ��� ��������� ����� ����� ���������� ������� ������: �������� ������� ��� ����������, �����, ������� ������� ��� ���������� ����������(��������, ��� ��������������-���������� � ������� 6 �������).
                 �����������:
                 ���������� ������� ������:
                 ������ ������ ��� ����������.
                 ������ ��������� � ������� ��������� ������� (��������, 6 �������) � ��� ��������������.
                 ��������� �������� ��������� ����, � ������� ����� ���� ��������.
                */



        // ����� ���� ��� ������������ ������� �����������
        public BigInteger MinRequiredVotingParticipation { get; set; } = 50; // �� ��������� 50%
        public BigInteger MinApprovalPercentage { get; set; } = 66; // �� ��������� 66%





        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public string ProjectOfferId_Sha256 { get; set; } = string.Empty;
        public string projectId { get; set; } = string.Empty;

        public byte[] ProjectDescription_Compressed_MessagePack { get; set; } = new byte[1024]; // <--- ������ �������� ������� � ������� MessagePack
        public string ProjectDescription_NeoFS_Address { get; set; } = string.Empty;

        public ProjectStatus CurrentProjectStatus { get; internal set; } = ProjectStatus.Active; // <--- ������ �������. ��������: "� ��������", "� ��������", "���������", "��������������" � �.�.



        public bool IsProjectHasWinner { get; set; } = false;   // <--- ���� �� ����������      
        public bool IsProjectClosed { get; set; } = false;  // <--- ������ �� ������
        public bool IsProjectPaused { get; internal set; } = false; // <--- ������������� �� ������

        public bool IsLaunchVotingCompleted { get; internal set; } = false; // <--- ��������� �� ����������� �� ������ �������
        public bool IsFundraisingCompletionVotingCompleted { get; internal set; } = false; // <--- ��������� �� ����������� �� ���������� ����� ������� 
        public bool IsProjectUpdatesVotingCompleted { get; internal set; } = false; // <--- ��������� �� ����������� �� ����������
        public bool IsSuccessfulClosureVotingCompleted { get; internal set; } = false; // <--- ��������� �� ����������� �� �������� ����������


        // �������� ��� ��������� ����������� ����� ������������� ��������� �������, ������ ����������� �������� ������������ ��� �� ������� �� �������.

        // Launch Approval Vote
        public BigInteger LaunchApprovalTotalVotesCount { get; internal set; } = 0;// <--- ���������� ������� ��������������� �� ������ �������
        public BigInteger LaunchApprovalPositiveVotesCount { get; internal set; } = 0;// <--- ���������� ������� ��������������� �� ������ �������
        public BigInteger LaunchApprovalNegativeVotesCount { get; internal set; } = 0;
        public BigInteger LaunchApprovalAbstainedVotesCount { get; internal set; } = 0;


        public BigInteger FundraisingIncreaseTotalVotesCount { get; internal set; } = 0; // <--- ���������� ������� �� ���������� ��������������
        public BigInteger FundraisingIncreasePositiveVotesCount { get; internal set; } = 0; // <--- ���������� ������� �� ���������� ��������������
        public BigInteger FundraisingIncreaseNegativeVotesCount { get; internal set; } = 0;
        public BigInteger FundraisingIncreaseAbstainedVotesCount { get; internal set; } = 0;


        // Fundraising Completion Vote Abstains
        public BigInteger FundraisingCompletionTotalVotesCount { get; internal set; } = 0; // <--- ����� ���������� ������� �� ���������� ����� �������
        public BigInteger FundraisingCompletionPositiveVotesCount { get; internal set; } = 0; // <--- ���������� ������� �� ���������� ����� �������
        public BigInteger FundraisingCompletionNegativeVotesCount { get; internal set; } = 0;
        public BigInteger FundraisingCompletionAbstainedVotesCount { get; internal set; } = 0;


        // Successful Closure Vote Abstains
        public BigInteger SuccessfulClosureTotalVotesCount { get; internal set; } = 0; // <--- ������ �� ������ - �������� ���������� 
        public BigInteger SuccessfulClosurePositiveVotesCount { get; internal set; } = 0; // <--- ������ �� ������ - �������� ����������
        public BigInteger SuccessfulClosureNegativeVotesCount { get; internal set; } = 0;
        public BigInteger SuccessfulClosureAbstainedVotesCount { get; internal set; } = 0;

        // Pause Resume Vote 
        public BigInteger PauseResumePositiveVotesCount { get; internal set; } = 0; // <--- ������ �� ������ - �����/����������
        public BigInteger PauseResumeTotalVotesCount { get; internal set; } = 0; // <--- ������ �� ������ - �����/���������� 
        public BigInteger PauseResumeNegativeVotesCount { get; internal set; } = 0; // 
        public BigInteger PauseResumeAbstainedVotesCount { get; internal set; } = 0;

        // Termination With Refund Vote Abstains
        public BigInteger TerminationWithRefundPositiveVotesCount { get; internal set; } = 0; // <--- ���������� ������� �� ���������� Closed � ��������� �������
        public BigInteger TerminationWithRefundTotalVotesCount { get; internal set; } = 0; // <--- ���������� ������� �� ���������� � ��������� �������
        public BigInteger TerminationWithRefundNegativeVotesCount { get; internal set; } = 0;
        public BigInteger TerminationWithRefundAbstainedVotesCount { get; internal set; } = 0;



        public bool IsFundraisingIncreaseVotingCompleted { get; internal set; } = false;
        public ulong FundraisingIncreaseVotingDeadline { get; set; } = 0;

    }


    public partial class ProjectAccount // ����� ������ � ����������� � �������� ���������� ����������� �� �������������� ��������
    {
        // Enhanced refund tracking
        public BigInteger TotalRefundsProcessed { get; internal set; } = 0;

        public bool EmergencyRefundInProgress { get; internal set; } = false;
    }


    ///////////////////////////////////////////////////////////////END DISPUTE //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class ProjectAccount // ����� ������ � ������ ���������� � �������� Milestone / kpi / Reputation / Analytics
    {
        // ������� ������������� 
        public ulong ProjectArchivingTime { get; set; } = 0; // �����, ����� ������ ��� �������������
        public bool IsArchived { get; set; } = false; // ������������� �� ������


        // ���������� � ��������� �������
        public BigInteger TotalUniqueVoters { get; set; } = 0; // ����� ����� ���������� ���������� �����������
        public BigInteger AverageVoterParticipation { get; set; } = 0; // ������� ���� ���������� (%)
        public ulong ProjectActivityScore { get; set; } = 0; // ������ ���������� �������
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public partial class ProjectAccount
    // ����� ������ � ������ ���������� � �������� ��� ����������� ����������������
    {
        public UInt160 rewardTokenContractAddress { get; set; } = UInt160.Zero; // ����� ��������� ������ ��������������

        public BigInteger totalFundsLocked { get; set; } = 0; // ����� ����� ��������������� �������

    }





}
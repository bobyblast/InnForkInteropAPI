
using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.ComponentModel;
using System.Numerics;



namespace InnFork.NeoN3;

public partial class IF_MainGateway
{
    public partial class ProjectAccount // часть класса с переменными и методами подсистемы голосования по проекту
    {

        public ProjectSettings projectSettings { get; set; } = new ProjectSettings(); // <--- настройки проекта
    }

}


public partial class IF_MainGateway
{



    [Serializable]
    public class DisputeRecord
    {
        public string DisputeId;
        public string ProjectId;
        public UInt160 Initiator;
        public DisputeType Type;
        public DisputeStatus Status;
        public string Description;
        public UInt160 ManufacturerInvolved;
        public byte MilestoneStepInvolved;
        public ulong CreatedAt;
        public ulong UpdatedAt;
        public string ResolutionNote;
        public bool SanctionsApplied;
        public BigInteger PenaltyAmount;
        public bool InitiatorPenalized;
        public BigInteger InitiatorPenaltyAmount;
        public BanReason BanReasonApplied;
    }



    //[Serializable]
    public class ProjectPackage
    {
        public string ProjectOfferId_Sha256 { get; set; } = string.Empty;
        public string ProjectOfferShortJson { get; set; } = string.Empty;

        public string ProjectId_Sha256 { get; set; } = string.Empty;
        public string ProjectJson { get; set; } = string.Empty;

        public UInt160 AuthorAccount = UInt160.Zero;

        public byte[] AuthorPubKey = new byte[33];
        public byte[] Signature = new byte[64];
    }

    public partial class ProjectSettings
    {
        // ВОССТАНОВЛЕННЫЕ СВОЙСТВА МОДЕЛИ (совместимость)
        public bool IsWinnerSelectionFinalized { get; set; }
        public ulong ManagementTransferVotingDeadline { get; set; }

        public ProjectFundingType projectFundingType = ProjectFundingType.Flexible;
        public BigInteger AutoPauseLockThresholdPercent { get; set; } = 50;

        // Голосование
        public BigInteger MinimumVotesRequired { get; set; } = 0;
        public BigInteger MinRequiredVotingParticipation { get; set; } = 50;
        public BigInteger MinApprovalPercentage { get; set; } = 66;
        public bool AutoAssignVoicelessToAbstain { get; internal set; }
        public bool AutoAbstainVoteAsSupport { get; internal set; }

        // Этапы/времена
        public MilestoneFundingType DefaultFundingType { get; set; } = MilestoneFundingType.Before;
        public ulong DefaultVotingDuration { get; set; } = 0;
        public bool AllowSteppedFinancing { get; set; } = true;
        public byte MaxMilestoneSteps { get; set; } = 10;
        public int MaxManufacturers { get; set; } = 5;
        public ulong FundraisingDeadline { get; set; } = 0;
        public ulong ManufacturerSelectionDeadline { get; set; } = 0;
        public ulong LaunchVotingDeadline { get; set; } = 0;
        public ulong FundraisingVotingDeadline { get; set; } = 0;

        // Финансы
        public byte DefaultPrizeFundAllocation { get; set; } = 0;
        public BigInteger CreatorFeePercentage { get; set; } = 1;
        public BigInteger CreatorFixedReward { get; set; } = 0;
        public BigInteger FLMUSD_PrizeFundGoal { get; set; } = 100000;
        public BigInteger WithdrawalTimeout { get; set; } = 15552000;
        public BigInteger PrizeFundExcessWithdrawalMultiplier { get; internal set; } = 2;

        // Флаги/автоматизация/безопасность
        public bool EnableFraudDetection { get; set; } = true;
        public bool AutoDistributeConsentedFunds { get; set; } = true;
        public bool AutoPauseEnabled { get; set; } = true;
        public bool AutoFinishExpiredVotings { get; set; } = true;

        // Описание проекта
        public string ProjectDescription_NeoFS_Address { get; set; } = string.Empty;

    }

    //[Serializable]
    public partial class MilestoneCompletionVotesStruct
    {
        public MilestoneCompletionVotesStruct() { }

        public string Sha256Hash_Id = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string ProjectSha256_Id { get; set; } = string.Empty;
        public UInt160 ManufacturerCandidate { get; set; } = UInt160.Zero;
        public string ManufacturerProductId_Sha256 { get; set; } = string.Empty;

        public byte StepNumber { get; set; } = 0;

        public BigInteger FinancialAmount { get; set; } = 0;
        public BigInteger RequestedFinancialAmount { get; set; } = 0;

        public BigInteger TotalVotesCount { get; set; } = 0;
        public BigInteger PositiveVotesCount { get; set; } = 0;
        public BigInteger NegativeVotesCount { get; set; } = 0;
        public BigInteger AbstainedVotesCount { get; set; } = 0;

        public ulong Deadline_UnixTime { get; set; } = 0;
        public ulong VotingStartTime { get; set; } = 0;
        public ulong VotingDuration { get; set; } = 0;

        public BigInteger MinimumVotesRequired { get; set; } = 0;

        public bool isVotingStepComplete = false;


        public bool isFundingSent = false;
        public bool IsVotingSuccessful { get; set; }
        public bool isManufacturerCandidateUsedSteppedFinancial { get; set; } = false;
        public bool IsDisputed { get; set; }


    }


    //[Serializable]
    public partial class CandidateWinnerVotesStruct
    {
        public CandidateWinnerVotesStruct() { }

        public string Sha256Hash_Id { get; set; } = string.Empty;
        public string ProjectSha256_Id { get; set; } = string.Empty;
        public UInt160 ManufacturerCandidate { get; set; } = UInt160.Zero;
        public string ManufacturerProductId_Sha256 { get; set; } = string.Empty;

        public BigInteger TotalVotesCount { get; set; } = 0;
        public BigInteger PositiveVotesCount { get; set; } = 0;
        public BigInteger NegativeVotesCount { get; set; } = 0;
        public BigInteger AbstainedVotesCount { get; set; } = 0;

        public ulong VotingStartTime { get; set; } = 0;
        public ulong VotingDuration = 0;

        public BigInteger MinimumVotesRequired;
        public bool IsVotingSuccessful;


    }

    //[Serializable]
    public class MilestoneTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BigInteger RequestedAmount { get; set; }
        public ulong Duration { get; set; }
        public string TemplateType { get; set; }

    }

}
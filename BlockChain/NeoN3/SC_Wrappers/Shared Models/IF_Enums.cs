
using System.ComponentModel;


namespace InnFork.NeoN3;




// Enhanced fraud detection enums
public enum FraudType : byte
{
    SybilAttack = 1, // Creating multiple fake accounts
    Collusion = 2, // Coordinated actions of a group of participants
    BalanceManipulation = 3, // Balance manipulation to increase voice weight
    TemporalAnomaly = 4, // Suspicious time patterns of voting
    RapidVoting = 5, // Abnormally fast voting
    VoteSwitching = 6 // Frequent change of votes
}
public enum RefundReason : byte
{
    ProjectTermination = 1, // Project termination
    ProjectFailure = 2, // Project failure
    AuthorRequest = 3, // Author's request for a refund
    FraudDetection = 4, // Fraud detection
    EmergencyShutdown = 5, // Emergency shutdown of the project
    RegulatoryCompliance = 6 // Regulatory compliance issues
}


public enum MilestoneFundingType { Before, After, Split } // Type of stage financing (0 - before, 1 - after, 2 - 50/50)






// Dispute Types and Status enums
public enum DisputeType
{
    PaymentDispute = 1, // Payment dispute
    QualityDispute = 2, // Product quality dispute  
    DeliveryDispute = 3, // Dispute over delivery/deadlines
    Refund = 4, // Refund dispute
    MilestoneCompletion = 5, // Dispute at the end of the stage
    ContractBreach = 6, // Breach of contract
    IntellectualProperty = 7, // Intellectual property
    FraudAccusation = 8, // Fraud charge
    TechnicalIssues = 9, // Technical issues
    CommunicationFailure = 10, // Communication problems
    Fraud = 11
}

public enum DisputeStatus
{
    Created = 1, // Created
    UnderReview = 2, // Under consideration
    EvidenceCollection = 3, // Collecting evidence
    Mediation = 4, // Mediation
    Arbitration = 5, // Arbitration
    Resolved = 6, // Allowed
    Rejected = 7, // Rejected
    Escalated = 8, // Escalated
    Closed = 9, // Closed
    Open = 10
}

/// <summary>
/// Reasons for project participants' bans
/// Extended system for comprehensive violation management
/// </summary>
public enum BanReason
{
    FraudulentActivity = 1, // Fraudulent activity
    ViolationOfTerms = 2, // Violation of the terms
    RepeatedDisputes = 3, // Recurring disputes
    QualityIssues = 4, // Quality issues
    PaymentIssues = 5, // Problems with payments
    CommunicationIssues = 6, // Communication problems
    TechnicalViolations = 7, // Technical violations
    SecurityBreach = 8, // Security breach
    InappropriateBehavior = 9, // Inappropriate behavior
    SystemAbuse = 10, // System Abuse
    FraudSuspicion = 11, // Suspicion of fraud
    MilestoneFailure = 12, // Production stage failure

    // New reasons for bans for the extended security system
    SybilAttack = 13, // Sybil attack (multiple fake accounts)
    Collusion = 14, // Collusion of participants
    BalanceManipulation = 15, // Balance manipulation to increase voice weight
    BotActivity = 16, // Bot-like activity
    SpamVoting = 17, // Spam voting
    VoteManipulation = 18, // Voting manipulation
    ContractViolation = 19, // Violation of the terms of the contract
    DeliveryFailure = 20, // Non-fulfillment of delivery obligations
    TimeoutViolation = 21, // Violation of time limits

    // Specialized reasons for Manufacturers
    ProductDefects = 22, // Product defects
    MaterialsFraud = 23, // Fraud with materials
    ProductionDelay = 24, // Systematic production delays
    QualityControlFailure = 25, // Quality control failure
    SpecificationViolation = 26,// Violation of technical requirements

    // Specialized reasons for backers
    MultipleAccountAbuse = 27, // Abuse of multiple accounts
    FakeInvestment = 28, // Fake investments
    ReviewManipulation = 29, // Manipulation of reviews
    ChargebackFraud = 30, // Fraudulent refunds

    // Administrative reasons
    RegulatoryViolation = 31, // Violation of regulatory requirements
    LegalComplaint = 32, // Legal complaints
    IntellectualPropertyTheft = 33, // Theft of intellectual property
    DataPrivacyViolation = 34, // Data privacy violation

    // Reasons related to the dispute system
    FalseDispute = 35, // False disputes
    DisputeAbuse = 36, // Abuse of the dispute system
    EvidenceFalsification = 37, // Falsification of evidence
    ArbitrageManipulation = 38, // Manipulation of arbitration

    // Reasons for an automatic security system
    AnomalousVotingPattern = 39,// Abnormal voting patterns
    SuspiciousTransactions = 40,// Suspicious transactions
    NetworkAttack = 41, // Network attacks
    SmartContractExploit = 42, // Exploiting smart contract vulnerabilities

    // Common reasons
    CommunityComplaint = 43, // Community complaints
    ModerationViolation = 44, // Violation of the moderation rules
    PlatformTOS = 45, // Violation of the user agreement of the platform
    UnverifiedIdentity = 46, // Unverified identity
    SuspiciousGeolocation = 47, // Suspicious geolocation

    // Reasons for emergencies
    EmergencyBan = 98, // Emergency lockdown
    ManualReview = 99, // Manual check by moderator
    RepeatedFraudulentDisputes = 100
}


/// <summary>
/// Types of voting in the project
/// </summary>
public enum VoteType
{
    LaunchApproval = 1, // Project launch
    FundraisingCompletion = 2, // Completion of fundraising
    ProjectUpdates = 3, // Project updates
    SuccessfulClosure = 4, // Successful completion of the project
    PauseResume = 5, // Pause/Resuming the project
    ManufacturerSelection = 6, // Manufacturer selection
    TerminationWithRefund = 7, // Completion with refund
    MilestoneCompletion = 8, // Completion of the production stage
    FundraisingIncrease = 9 // Increase in the target amount of funding
}


/// <summary>
/// Type of votes (positive, negative, abstention)
/// </summary>
public enum VoteCountType
{
    Positive, // Votes For
    Negative, // Votes Against
    Abstained, // Abstained votes
    Total // Total number of votes
}

public enum ProjectStateRequest
{
    HasWinner, // Does the project have a winner
    IsClosed, // Is the project closed
    IsApproved, // Has the project been approved
    Paused, // Is the project suspended
    LaunchVotingCompleted, // Has the launch vote been completed
    FundraisingCompleted, // Has the fundraising completion vote been completed
    UpdatesVotingCompleted, // Whether voting on updates is completed
    SuccessfulClosureCompleted, // Has the vote on successful closure been completed
    PauseVotingCompleted // Has the suspension/resumption voting been completed
}


public enum ProjectStatus
{
    Active, // Active project
    Proposed, // Proposed project
    Fundraising, // Fundraising stage
    Manufacturing, // Manufacturing stage
    Paused, // Project paused
    Completed, // Project completed
    Terminated, // Project terminated  
    Cancelled, // Project cancelled
    Failed // Project failed
}



public enum BackerVotesEnum // <--- ENUM of Baker's vote for financing the product completion stage by the manufacturer
{
    Positive,
    Negative,
    Abstained
}


public enum ProjectCloseReason
{
    [Description("Banned")]
    Banned,

    [Description("Not enough funds")]
    NotEnoughFunds,

    [Description("Not enough votes")]
    NotEnoughVotes,

    [Description("Not enough backers")]
    NotEnoughBackers,

    [Description("Project deadline expired")]
    DeadlineExpired,

    [Description("Author requested closure")]
    AuthorRequested,

    [Description("Regulatory compliance issues")]
    RegulatoryIssues,

    [Description("Suspicious activity detected")]
    FraudSuspicion,

    [Description("Technical implementation unfeasible")]
    TechnicallyUnfeasible,

    [Description("Intellectual property dispute")]
    IntellectualPropertyDispute,

    [Description("Key team members left")]
    TeamDissolved,

    [Description("Market conditions changed")]
    MarketConditionsChanged,

    [Description("Failed due diligence")]
    FailedDueDiligence,

    [Description("Competing project launched")]
    CompetitorLaunched,

    [Description("Manufacturing issues")]
    ManufacturingIssues
}


public enum ProjectFundingType
{
    [Description("All Or Nothing")]
    AllOrNothing,
    [Description("Flexible")]
    Flexible,
    [Description("Rewards Based")]
    RewardsBased,
    [Description("Philanthropic")]
    Philanthropic,
    [Description("Equity")]
    Equity
};









using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.ComponentModel;
using System.Numerics;
using static InnFork.NeoN3.IF_MainGateway;



namespace InnFork.NeoN3;




// Это список карт и переменных голосовалки по проекту как таковому 

public partial class IF_MainGateway
{
    public class ProjectOfferPackage
    {
        public UInt160 AuthorAccount;
        public string ProjectOfferSha256Id;
        public byte[] AuthorPubKey = new byte[33]; // Инициализация для избежания null
        public byte[] Signature = new byte[64];
    }



    // часть класса ProjectAccount со структурами и перечислениями подсистемы голосования по производителям продукта
    //[Serializable]
    public partial class ProjectAccount
    {
        // Добавьте эту вспомогательную структуру внутрь класса IF_MainGateway или ProjectAccount
        //[Serializable]
        public class CalculatedVoteOutcome
        {
            public CalculatedVoteOutcome()
            {
            }

            public BigInteger FinalPositiveVotes { get; internal set; } = 0;       // Итоговые голоса "за" после применения логики
            public BigInteger FinalNegativeVotes { get; internal set; } = 0;       // Итоговые голоса "против"
            public BigInteger FinalAbstainedVotes { get; internal set; } = 0;      // Итоговые голоса "воздержался" (явные + назначенные)
            public BigInteger TotalEligibleVoters { get; internal set; } = 0;      // Общее число имеющих право голоса
            public BigInteger TotalParticipatedOrAssigned { get; internal set; } = 0; // Общее число фактически проголосовавших + назначенных воздержавшихся
            public bool IsSuccessful { get; internal set; } = false;                  // Итог голосования
        }


        //Описание: Процент прибыли производителя, который распределяется среди инвесторов(например, 30%).
        //Значение по умолчанию: 30
        public BigInteger Profit_Share_Percentage = 30; // Установлено значение по умолчанию


        // Флаги финализации для различных типов голосований
        public bool IsLaunchVotingFinalized { get; internal set; } = false;
        public bool IsFundraisingIncreaseVotingFinalized { get; internal set; } = false; // Если будет отдельный метод финализации
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
        public bool IsManagementTransferVotingCompleted { get; internal set; } = false; // Можно совместить с IsManagementTransferVotingFinalized


    }

    public partial class ProjectAccount // часть класса с переменными и методами подсистемы голосования по проекту
    {

        public bool AutoAssignVoicelessToAbstain { get; internal set; } // <--- Флаг для автоматического учета безголосых как воздержавшихся
        public bool AutoAbstainVoteAsSupport { get; internal set; } // <--- Флаг для автоматического учета воздержавшихся голосов как поддерживающих


        public bool DirectInnForkProjectManagement { get; set; } = false; // управление проектом напрямую через InnFork Gateway



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


        public bool AutoFinishExpiredVotings { get; internal set; } = true; // <--- Автоматически завершать голосования с истекшим сроком

        public bool AutoPauseEnabled { get; internal set; } = true; //  
        public bool AutoDistributeConsentedFunds { get; internal set; } = true; // 
        public bool EnableFraudDetection { get; internal set; } = true;
        public BigInteger WithdrawalTimeout { get; internal set; } = 100000; // 
        public byte DefaultPrizeFundAllocation { get; internal set; }

        // public BigInteger MinManufacturerInvestment { get; internal set; }
        //     public string Description { get; internal set; }



        public byte MaxMilestoneSteps { get; internal set; }
        public MilestoneFundingType DefaultFundingType { get; internal set; } = MilestoneFundingType.Before; // Тип финансирования этапов (до, после, разделение)


        public int MaxManufacturers { get; internal set; } = 5; // Максимальное количество производителей-кандидатов
        public bool AllowSteppedFinancing { get; internal set; } = true; // Разрешено ли поэтапное финансирование производителями




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




        /*Профит для Project Creator
         Вопрос: Как Project Creator получает прибыль от проекта?
         Описание:
         Возможные источники:
         Комиссия с каждого доната(fee).
         Процент(например, 1%) от средств успешного продукта.
         Фиксированная сумма при успешном старте проекта.
         Предложение:
         Ввести гибкую систему комиссий и вознаграждений, настраиваемых при создании проекта.
         Переменные:*/

        // Процент комиссии для Project Creator (например, 1%)
        public BigInteger CreatorFeePercentage { get; set; } = 1;
        // Фиксированная сумма за успешный старт проекта
        public BigInteger CreatorFixedReward { get; set; } = 1000;

        public bool AuthorSuccessGetFee { get; internal set; } = false;
        public bool InnForkFeeCollected { get; internal set; } = false;
        public UInt160 ProjectCreatorAddress { get; internal set; }




        // время регистрации проекта
        public ulong RegistrationTime { get; set; } = Runtime.Time;
        // Время последней активности проекта
        public ulong LastActivityTime { get; set; } = 0;

        // Тайм-аут для вывода средств (например, 6 месяцев в секундах)
        public readonly BigInteger WITHDRAWAL_TIMEOUT = 15552000;

        // Дополнительные поля для отслеживания сроков голосований
        public ulong LaunchVotingDeadline { get; set; }
        public ulong FundraisingVotingDeadline { get; set; }

        public ulong FundraisingDeadline { get; set; } = 0;   // Дедлайн для сбора средств        // Дедлайн для выбора производителя
        public ulong ManufacturerSelectionDeadline { get; set; } = 0;    // Дедлайн для выбора производителя



        // Призовой фонд проекта (средства, не зарезервированные за производителями)
        public BigInteger FLMUSD_PrizeFundBalance { get; set; } = 0; // <--- текущая сумма призового фонда
        public BigInteger FLMUSD_PrizeFundGoal { get; set; } = 100000; // <--- сумма в долларах которую надо собрать
        public BigInteger FLMUSD_TotalProjectBalance { get; set; } = 0; // <--- текущая общая сумма проекта (включая призовой фонд и средства, зарезервированные за производителями)



        public UInt160 ManufacturerWinnerAddress { get; set; } = UInt160.Zero; // <--- адрес производителя-победителя
        public string ManufacturerWinnerProductId_Sha256 { get; set; } = string.Empty; // <--- хэш продукта производителя-победителя
        public string ManufacturerWinnerProductDescription_NeoFS_Address { get; set; } = string.Empty; // <--- адрес описания продукта производителя-победителя в NeoFS

        public const int MAX_MILESTONES = 10;         // Максимальное число этапов
        public const int MAX_MANUFACTURERS = 5;       // Максимальное число производителей


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        /*       
                 Средства, зарезервированные для этапов производителей, считаются невыводимыми.
                 Для призового фонда нужно определить условия вывода: закрытие проекта без победителя, пауза, избыток средств или отсутствие активности(например, нет производителей-кандидатов в течение 6 месяцев).
                 Предложение:
                 Определить условия вывода:
                 Проект закрыт без победителя.
                 Проект неактивен в течение заданного времени (например, 6 месяцев) и нет производителей.
                 Собранные средства превышают цель, и излишки могут быть выведены.
                */



        // Новые поля для конфигурации системы голосования
        public BigInteger MinRequiredVotingParticipation { get; set; } = 50; // По умолчанию 50%
        public BigInteger MinApprovalPercentage { get; set; } = 66; // По умолчанию 66%





        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public string ProjectOfferId_Sha256 { get; set; } = string.Empty;
        public string projectId { get; set; } = string.Empty;

        public byte[] ProjectDescription_Compressed_MessagePack { get; set; } = new byte[1024]; // <--- сжатое описание проекта в формате MessagePack
        public string ProjectDescription_NeoFS_Address { get; set; } = string.Empty;

        public ProjectStatus CurrentProjectStatus { get; internal set; } = ProjectStatus.Active; // <--- статус проекта. например: "В ожидании", "В процессе", "Завершено", "Приостановлено" и т.д.



        public bool IsProjectHasWinner { get; set; } = false;   // <--- есть ли победитель      
        public bool IsProjectClosed { get; set; } = false;  // <--- закрыт ли проект
        public bool IsProjectPaused { get; internal set; } = false; // <--- приостановлен ли проект

        public bool IsLaunchVotingCompleted { get; internal set; } = false; // <--- завершено ли голосование за запуск проекта
        public bool IsFundraisingCompletionVotingCompleted { get; internal set; } = false; // <--- завершено ли голосование за завершение сбора средств 
        public bool IsProjectUpdatesVotingCompleted { get; internal set; } = false; // <--- завершено ли голосование за обновление
        public bool IsSuccessfulClosureVotingCompleted { get; internal set; } = false; // <--- завершено ли голосование за успешное завершение


        // Логичено что некоторое голосование может иниициировать Создатель проекта, другие голосования стартуют авоматически или не зависят от времени.

        // Launch Approval Vote
        public BigInteger LaunchApprovalTotalVotesCount { get; internal set; } = 0;// <--- количество бэкеров проголосовавших за запуск проекта
        public BigInteger LaunchApprovalPositiveVotesCount { get; internal set; } = 0;// <--- количество бэкеров проголосовавших за запуск проекта
        public BigInteger LaunchApprovalNegativeVotesCount { get; internal set; } = 0;
        public BigInteger LaunchApprovalAbstainedVotesCount { get; internal set; } = 0;


        public BigInteger FundraisingIncreaseTotalVotesCount { get; internal set; } = 0; // <--- количество голосов ЗА увеличение финансирования
        public BigInteger FundraisingIncreasePositiveVotesCount { get; internal set; } = 0; // <--- количество голосов ЗА увеличение финансирования
        public BigInteger FundraisingIncreaseNegativeVotesCount { get; internal set; } = 0;
        public BigInteger FundraisingIncreaseAbstainedVotesCount { get; internal set; } = 0;


        // Fundraising Completion Vote Abstains
        public BigInteger FundraisingCompletionTotalVotesCount { get; internal set; } = 0; // <--- общее количество голосов ЗА завершение сбора средств
        public BigInteger FundraisingCompletionPositiveVotesCount { get; internal set; } = 0; // <--- количество голосов ЗА завершение сбора средств
        public BigInteger FundraisingCompletionNegativeVotesCount { get; internal set; } = 0;
        public BigInteger FundraisingCompletionAbstainedVotesCount { get; internal set; } = 0;


        // Successful Closure Vote Abstains
        public BigInteger SuccessfulClosureTotalVotesCount { get; internal set; } = 0; // <--- голоса за проект - успешное завершение 
        public BigInteger SuccessfulClosurePositiveVotesCount { get; internal set; } = 0; // <--- голоса за проект - успешное завершение
        public BigInteger SuccessfulClosureNegativeVotesCount { get; internal set; } = 0;
        public BigInteger SuccessfulClosureAbstainedVotesCount { get; internal set; } = 0;

        // Pause Resume Vote 
        public BigInteger PauseResumePositiveVotesCount { get; internal set; } = 0; // <--- голоса за проект - пауза/продолжить
        public BigInteger PauseResumeTotalVotesCount { get; internal set; } = 0; // <--- голоса за проект - пауза/продолжить 
        public BigInteger PauseResumeNegativeVotesCount { get; internal set; } = 0; // 
        public BigInteger PauseResumeAbstainedVotesCount { get; internal set; } = 0;

        // Termination With Refund Vote Abstains
        public BigInteger TerminationWithRefundPositiveVotesCount { get; internal set; } = 0; // <--- количество голосов ЗА завершение Closed с возвратом средств
        public BigInteger TerminationWithRefundTotalVotesCount { get; internal set; } = 0; // <--- количество голосов за завершение с возвратом средств
        public BigInteger TerminationWithRefundNegativeVotesCount { get; internal set; } = 0;
        public BigInteger TerminationWithRefundAbstainedVotesCount { get; internal set; } = 0;



        public bool IsFundraisingIncreaseVotingCompleted { get; internal set; } = false;
        public ulong FundraisingIncreaseVotingDeadline { get; set; } = 0;

    }


    public partial class ProjectAccount // часть класса с переменными и методами подсистемы голосования по производителям продукта
    {
        // Enhanced refund tracking
        public BigInteger TotalRefundsProcessed { get; internal set; } = 0;

        public bool EmergencyRefundInProgress { get; internal set; } = false;
    }


    ///////////////////////////////////////////////////////////////END DISPUTE //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class ProjectAccount // часть класса с новыми свойствами и методами Milestone / kpi / Reputation / Analytics
    {
        // Система архивирования 
        public ulong ProjectArchivingTime { get; set; } = 0; // Время, когда проект был заархивирован
        public bool IsArchived { get; set; } = false; // Заархивирован ли проект


        // Статистика и аналитика проекта
        public BigInteger TotalUniqueVoters { get; set; } = 0; // Общее число уникальных участников голосований
        public BigInteger AverageVoterParticipation { get; set; } = 0; // Средняя явка голосующих (%)
        public ulong ProjectActivityScore { get; set; } = 0; // Оценка активности проекта
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public partial class ProjectAccount
    // часть класса с новыми свойствами и методами для расширенной функциональности
    {
        public UInt160 rewardTokenContractAddress { get; set; } = UInt160.Zero; // Адрес контракта токена вознаграждения

        public BigInteger totalFundsLocked { get; set; } = 0; // Общая сумма заблокированных средств

    }





}
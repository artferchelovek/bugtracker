namespace server.Models.Enums
{
    public enum TicketStatus
    {
        OPEN,
        IN_PROGRESS,
        READY_FOR_TEST,
        REJECTED,
        REOPENED,
        CLOSED,
        DRAFT
    }

    public enum Priority
    {
        P0_BLOCKER,
        P1_CRITICAL,
        P2_HIGH,
        P3_MEDIUM,
        P4_LOW
    }

    public enum RejectionReason
    {
        NOT_A_BUG,
        CANNOT_REPRODUCE,
        DUPLICATE,
        OUT_OF_SCOPE
    }
}

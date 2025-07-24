namespace DNC.CM
{
    public enum DocsWorkflow
    {
        Writing = 0,
        Feedback,
        Consider,
        Approval,
        Reject,
        Cancel
    }

    public enum NCWorkflow
    {
        Writing = 0,
        Consider,
        Approval,
        Reject
    }
    public enum LoginStatus
    {
        EMPTY = 0,
        SUCCESS,
        ERROR,
        LOCK
    }
    public enum ActionType
    {
        LOGIN = 1,
        LOGOUT,
        ADD,
        UPDATE,
        DELETE,
        GET,
        COMMENT,
        DOWNLOAD,
        BOOKMARK,
        SETTING,
        REPORT
    }
}

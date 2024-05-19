namespace ProjectManagment.Singleton
{
    public enum ErrorCodes
    {
        Ok = 0,
        ProjectAlreadyExist = -1,
    }

    public static class ErrorMessages
    {
        public static Dictionary<ErrorCodes, string> ErrorCodeToMessage = new Dictionary<ErrorCodes, string>()
        {
            { ErrorCodes.Ok, "OK" },
            { ErrorCodes.ProjectAlreadyExist, "User already has a project with the same name" }
        };
    }
}

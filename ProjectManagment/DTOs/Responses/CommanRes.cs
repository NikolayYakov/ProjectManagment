using ProjectManagment.Singleton;

namespace ProjectManagment.DTOs.Responses
{
    public class CommanRes
    {
        public CommanRes(ErrorCodes errorCode)
        {
            this.ErrorCode = errorCode;
            this.Succsess = errorCode == ErrorCodes.Ok;
            this.ErrorMessage = ErrorMessages.ErrorCodeToMessage[errorCode];
        }

        public CommanRes()
        {
            
        }

        public bool Succsess { get; set; } = true;
        public ErrorCodes ErrorCode { get; set; } = ErrorCodes.Ok;
        public string ErrorMessage { get; set; } = ErrorMessages.ErrorCodeToMessage[ErrorCodes.Ok];
    }
}

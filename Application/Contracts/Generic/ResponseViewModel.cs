
namespace Application.Contracts.Generic
{
    public class ResponseViewModel<T>
    {
        public FeedBackCode Status { get; set; }
        public string? Message { get; set; }
        public long? Total { get; set; }
        public T? Success { get; set; }
        public object? Errors { get; set; }

        public ResponseViewModel()
        {
        }

        public ResponseViewModel(FeedBackCode status)
        {
            ResponseMessage(status);
            Status = status;
            Total = null;
            Success = default(T);
            Errors = null;
        }

        public ResponseViewModel(FeedBackCode status, T? success)
        {
            ResponseMessage(status);
            Status = status;
            Success = success;
            Errors = null;
        }

        public ResponseViewModel(FeedBackCode status, T? success, long? total = null)
        {
            ResponseMessage(status);
            Status = status;
            Total = total;
            Success = success;
            Errors = null;
        }

        public ResponseViewModel(FeedBackCode status, object? errors = null)
        {
            ResponseMessage(status);
            Status = status;
            Total = null;
            Success = default(T);
            Errors = errors;
        }

        public ResponseViewModel(FeedBackCode status, T? success, object? errors = null)
        {
            ResponseMessage(status);
            Status = status;
            Total = null;
            Success = success;
            Errors = errors;
        }

        private void ResponseMessage(FeedBackCode status)
        {
            //System.Type type = GetType();
            //Helpper.Commen.Enumerations.Type.EnumLang enumLang = CoreUtility.CurrentLang();
            //string propName = status.ToString();
            //ResponseMessageViewModel responseMessageViewModel = ((enumLang == Helpper.Commen.Enumerations.Type.EnumLang.Ar) ? LangUtility.ResponseMessageSettingsAr(ConfigurationUtility.Config) : LangUtility.ResponseMessageSettingsEn(ConfigurationUtility.Config));
            //string text = ((enumLang != Helpper.Commen.Enumerations.Type.EnumLang.Ar) ? responseMessageViewModel.ResponseMessageEn?.GetValue(propName)?.ToString() : responseMessageViewModel.ResponseMessageAr?.GetValue(propName)?.ToString());
            //PropertyUtility.SetPropValue(this, type, "Message", text ?? "Undefined_Key");
        }


    }
}

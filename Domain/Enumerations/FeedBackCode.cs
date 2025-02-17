namespace Domain.Enumerations;

public enum FeedBackCode
{
    OK = 1,
    ValidationNotValid = 409,
    NotAccept = 400,
    AlreadyExists = 410,
    NotFound = 411,
    ValidationFailed = 412,
    Deleted = 413,
    NotDeleted = 414
}


namespace Application.Contracts.Generic;

public class FileViewModel
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public byte[]?  Contents { get; set; }
}
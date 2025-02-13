using static Domain.Enumerations.Version;

namespace Domain.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public int Version { get; set; } = CURRENT_VERSTION;
}
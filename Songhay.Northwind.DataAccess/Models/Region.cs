namespace Songhay.Northwind.DataAccess.Models;

public partial class Region
{
    public long RegionId { get; set; }
    public string RegionDescription { get; set; } = null!;

    public virtual ICollection<Territory> Territories { get; set; } = new HashSet<Territory>();
}

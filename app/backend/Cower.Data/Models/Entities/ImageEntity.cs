using System.ComponentModel.DataAnnotations.Schema;
using Cower.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cower.Data.Models.Entities;

public class ImageEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public ImageType Type { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }
    
    [DeleteBehavior(DeleteBehavior.Restrict)]
    [InverseProperty(nameof(CoworkingFloorEntity.Image))]
    public IReadOnlyCollection<CoworkingFloorEntity> Floors { get; set; }
    
    [DeleteBehavior(DeleteBehavior.Restrict)]
    [InverseProperty(nameof(CoworkingSeatEntity.Image))]
    public IReadOnlyCollection<CoworkingSeatEntity> Seats { get; set; }
}
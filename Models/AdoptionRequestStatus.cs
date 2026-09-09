using System.ComponentModel.DataAnnotations;

namespace Huellitas.Models;

public enum AdoptionRequestStatus
{
    [Display(Name = "Pendiente")]
    Pending,

    [Display(Name = "Aprobada")]
    Approved,

    [Display(Name = "Rechazada")]
    Rejected,

    [Display(Name = "Cancelada")]
    Cancelled
}

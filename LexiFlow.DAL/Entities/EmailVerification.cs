using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class EmailVerification
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(128)]
    [Unicode(false)]
    public string Otp { get; set; } = null!;

    public DateTime ExpiredAt { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}

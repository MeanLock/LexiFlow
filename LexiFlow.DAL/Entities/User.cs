using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

[Index("Email", Name = "IX_Users_Email", IsUnique = true)]
public partial class User
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(200)]
    public string FullName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(500)]
    public string? Password { get; set; }

    [StringLength(200)]
    public string? GoogleId { get; set; }

    public int Role { get; set; }

    public int ContributeScore { get; set; }

    public bool IsVerified { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Owner")]
    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();

    [InverseProperty("User")]
    public virtual ICollection<ReviewRecord> ReviewRecords { get; set; } = new List<ReviewRecord>();

    [InverseProperty("User")]
    public virtual ICollection<ReviewSession> ReviewSessions { get; set; } = new List<ReviewSession>();

    [InverseProperty("User")]
    public virtual ICollection<ReviewState> ReviewStates { get; set; } = new List<ReviewState>();
}

using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class GameSubmission
{
    public int SubmissionId { get; set; }

    public int SubmittedByUserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Developer { get; set; }

    public string? Publisher { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? CoverArtUrl { get; set; }

    public string SubmissionStatus { get; set; } = null!;

    public string? RejectionReason { get; set; }

    public DateTime SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public int? ReviewedByUserId { get; set; }

    public virtual ICollection<GameSubmissionImage> GameSubmissionImages { get; set; } = new List<GameSubmissionImage>();

    public virtual User? ReviewedByUser { get; set; }

    public virtual User SubmittedByUser { get; set; } = null!;
}

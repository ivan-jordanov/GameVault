using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class GameSubmissionImage
{
    public int SubmissionImageId { get; set; }

    public int SubmissionId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public virtual GameSubmission Submission { get; set; } = null!;
}

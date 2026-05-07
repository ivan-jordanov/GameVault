using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? PublishDate { get; set; }

    public bool IsPublished { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User? LastModifiedByUser { get; set; }
}

using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class WebResource
{
    public int ResourceId { get; set; }

    public string Title { get; set; } = null!;

    public string HtmlContent { get; set; } = null!;

    public int? LastModifiedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User? LastModifiedByUser { get; set; }
}

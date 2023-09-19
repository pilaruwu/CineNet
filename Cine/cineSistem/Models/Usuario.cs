using System;
using System.Collections.Generic;

namespace cineSistem.Models;

public partial class Usuario
{
    public int IdUser { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}

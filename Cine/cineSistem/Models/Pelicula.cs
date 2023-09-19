using System;
using System.Collections.Generic;

namespace cineSistem.Models;

public partial class Pelicula
{
    public int IdPeli { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Director { get; set; }

    public string? Gender { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<RankingPelicula> RankingPeliculas { get; set; } = new List<RankingPelicula>();
}

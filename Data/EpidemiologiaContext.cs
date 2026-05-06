using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Epidemiologia.Dominio;

namespace Epidemiologia.Data
{
    public class EpidemiologiaContext : DbContext
    {
        public EpidemiologiaContext (DbContextOptions<EpidemiologiaContext> options)
            : base(options)
        {
        }

        public DbSet<Enfermedad> Enfermedad { get; set; } = default!;
        public DbSet<Paciente> Paciente { get; set; } = default!;
        public DbSet<Epidemiologia.Dominio.RegistroCaso> RegistroCaso { get; set; } = default!;
    }
}

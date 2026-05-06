using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Importante para [ForeignKey]

namespace Epidemiologia.Dominio
{
    public class RegistroCaso
    {
        [Key]
        public int Id_RegistroCaso { get; set; }

        public string Cod_Caso { get; set; }

        public DateTime fecha { get; set; }
        public string Estado { get; set; }

        // --- LLAVE FORÁNEA PARA PACIENTE ---
        public int Id_paciente { get; set; } // El ID físico de la DB

        [ForeignKey("Id_paciente")]
        public virtual Paciente Paciente { get; set; }


        // --- LLAVE FORÁNEA PARA ENFERMEDAD ---
        public int Id_enfermedad { get; set; } // El ID físico de la DB

        [ForeignKey("Id_enfermedad")]
        public virtual Enfermedad Enfermedad { get; set; }
    }
}
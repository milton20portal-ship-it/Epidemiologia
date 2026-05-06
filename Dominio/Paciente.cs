using Epidemiologia.Dominio;
using System.ComponentModel.DataAnnotations;

public class Paciente
{
    [Key]
    public int Id_paciente { get; set; }
    public string Cod_paciente { get; set; }
    public string Nombre { get; set; }
    public string Estado { get; set; }

    // Propiedad de navegación inversa
    public virtual ICollection<RegistroCaso> RegistroCasos { get; set; }
}
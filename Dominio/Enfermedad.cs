using Epidemiologia.Dominio;
using System.ComponentModel.DataAnnotations;

public class Enfermedad
{
    [Key]
    public int Id_enfermedad { get; set; }
    public string Cod_enferemedad { get; set; }
    public string Nombre { get; set; }
    public string Estado { get; set; }

    // Propiedad de navegación inversa
    public virtual ICollection<RegistroCaso> RegistroCasos { get; set; }
}
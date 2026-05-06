namespace Epidemiologia.Dominio.DTO
{
    public class RegistroCasoDTO
    {
        public int Id_RegistroCaso { get; set; }
        public string Cod_Caso { get; set; }
        public DateTime fecha { get; set; }
        public string Estado { get; set; }

        // Estos son los IDs que el usuario enviará o recibirá
        public int Id_paciente { get; set; }
        public int Id_enfermedad { get; set; }

        // Estos campos opcionales son geniales para los reportes
        public string? NombrePaciente { get; set; }
        public string? NombreEnfermedad { get; set; }
    }
}
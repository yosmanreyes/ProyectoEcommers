

namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Ctr_usuarios
    {
        [Key]
        public int IdUsuario { get; set; }
        public int Bloqueado { get; set; } 
        public string? Username { get; set; }
        public string? Identificacion { get; set; } 
        public string? Apellidos { get; set; } 
        public int? CODIGOCARGO { get; set; } 
        public DateTime? Fecha_creacion { get; set; } 
        public int? Usuario_creacion { get; set; } 
        public string? Maquina_creacion { get; set; }
        public int? Usuario_modifica { get; set; }
        public DateTime? Fecha_modifica { get; set; }
        public string? Maquina_modifica { get; set; }
        public string? Correo { get; set; } 
        public string? Funcionario { get; set; } 
        public string? Cargo { get; set; }
        public string? Ciudad { get; set; } 
        public string? Celular { get; set; } 
        public string? Clave { get; set; } 
        public string? Direccion_residencia { get; set; } 
        public string? Vigente { get; set; }
        public DateTime? Fecha_Nacimiento { get; set; }
        public string? Ciuda_nacimiento { get; set; } 
        public string? Cliente { get; set; }
        public DateTime? Fecha_vigencia { get; set; }
       


    }
}

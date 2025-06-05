

namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class ctr_roles_user
    {
        [Key]
        public int ID_USER_ROL { get; set; }
        [ForeignKey("FK_USUARIO")]
        public int ID_USUARIO { get; set; }
        [ForeignKey("FK_ROLES")]
        public int ID_ROL { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public int USUARIO_CREACION { get; set; }
        public string MAQUINA_CREACION { get; set; }
        public string VIGENTE { get; set; }
        public virtual Ctr_usuarios FK_USUARIO { get; set; }
        public virtual ctr_roles FK_ROLES { get; set; }
    }
}

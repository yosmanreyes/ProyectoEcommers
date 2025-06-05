

namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class ctr_menu_roles
    {
        [Key]
        public int ID_MENUROL { get; set; }
        [ForeignKey("FK_ROL")]
        public int ID_ROL { get; set; }
        [ForeignKey("FK_MENU")]
        public int ID_MENU { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public int USUARIO_CREACION { get; set; }
        public string MAQUINA_CREACION { get; set; }
        public string MAQUINA_MODIFICA { get; set; }
        public int USUARIO_MODIFICA { get; set; }
        public DateTime FECHA_MODIFICA { get; set; }

        public virtual ctr_menu FK_MENU { get; set; }
        public virtual ctr_roles FK_ROL { get; set; }
    }
}

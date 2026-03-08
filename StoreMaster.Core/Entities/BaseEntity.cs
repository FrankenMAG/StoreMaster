using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreadoEn {  get; set; }
        public DateTime? ModificadoEn { get; set; }
        public bool Eliminado { get; set; } = false;
    }
}

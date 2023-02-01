using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnection.Models.Interfaces
{
    public interface IBaseModel
    {
        int Id { get; set; }
        string Name { get; set; }
        Location Location { get; set; }
        bool IsJuridical { get; set; }
    }
}

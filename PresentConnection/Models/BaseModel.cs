using PresentConnection.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnection.Models
{
    public abstract class BaseModel : IBaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public bool IsJuridical { get; set; }

        public BaseModel(int id, string name, Location location, bool isJuridical)
        {
            this.Id = id;
            this.Name = name;
            this.Location = location;
            this.IsJuridical = isJuridical;
        }
    }
}

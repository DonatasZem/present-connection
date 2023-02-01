using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentConnection.Models
{
    public class Client : BaseModel
    {
        public Client(int id, string name, Location location, bool isJuridical) : base(id, name, location, isJuridical)
        {
        }
    }
}

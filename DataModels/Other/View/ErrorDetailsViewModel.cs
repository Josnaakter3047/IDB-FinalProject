using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataModels.Other.View
{
    public class ErrorDetailsViewModel
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

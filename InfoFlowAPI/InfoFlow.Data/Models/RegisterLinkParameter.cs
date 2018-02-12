using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoFlow.Data.Models
{
    public class RegisterLinkParameter
    {
        
        public int ID { get; set; }

        public Guid LinkParameter { get; set; }
    }
}

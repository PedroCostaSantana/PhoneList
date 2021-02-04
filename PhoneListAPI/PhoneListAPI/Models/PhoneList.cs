﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneListAPI.Models
{
    public class PhoneList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
    }
}

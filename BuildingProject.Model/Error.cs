﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingProject.Model
{
    public class Error
    {
        [Key]
        public int errorID { get; set; }
        [Display(Name = "Pagina")]
        public string page { get; set; }
        [Display(Name = "Opción")]
        public string option { get; set; }
        [Display(Name = "Descripción")]
        public string description { get; set; }
        [Display(Name = "Fecha")]
        public DateTime date { get; set; }
    }
}

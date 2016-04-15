using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patient_Transport.Models.Persistence {
    public class Dokter {

        [Display(Name="Dokter")]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Staff_Form.Models
{
    public class DistrictListViewModel
    {
        public SelectList Districts { get; set; }
        public string SelectedDistrict { get; set; }
        [Required(ErrorMessage = "This code is required")]
        public string DistrictCode { get; set; }
    }

    public class DistrictSchoolListViewModel
    {
        public SelectList Districts { get; set; }
        public SelectList Schools { get; set; }
        public string SelectedSchool { get; set; }
        public string SelectedDistrict { get; set; }
        [Required(ErrorMessage = "This code is required")]
        public string DistrictCode { get; set; }
    }
}
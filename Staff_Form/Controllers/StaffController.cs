using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Staff_Form.Models;


namespace Staff_Form.Controllers
{
    public class StaffController : Controller
    {
        KUDEREntities db = new KUDEREntities();
        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DistrictInformation(StaffInformationViewModel s)
        {
            var viewModel = new DistrictListViewModel()
            {
                Districts = new SelectList(db.Districts.ToList(), "leaID", "name")
            };

            return View(viewModel);
        }

        public ActionResult SchoolDistrictInformation()
        {
            var viewModel = new DistrictSchoolListViewModel()
            {
                Districts = new SelectList(db.Districts.ToList(), "leaID", "name"),
                Schools = new SelectList(db.Schools.ToList(), "schoolID", "name")
            };
            return View(viewModel);
        }

        public JsonResult GetSchools(string id)
        {
            var selectedDistrict = id;
            var schools = findSchools(selectedDistrict);
            IEnumerable<SelectListItem> filteredSchools = schools.Select(m => new SelectListItem { Text = m.name, Value = m.schoolID.ToString() });
            return Json(filteredSchools, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StaffInformation(DistrictListViewModel item, DistrictSchoolListViewModel ds)
        {

            if (ds.SelectedSchool != null)
            {
                var code = securityCodeCheck(ds.SelectedDistrict);
                if (ds.DistrictCode == code.First().securityCode)
                {
                    Session["schoolID"] = ds.SelectedSchool;
                    Session["leaID"] = ds.SelectedDistrict;
                    return View();
                }
                else
                {
                    return RedirectToAction("SchoolDistrictInformation", "Staff");
                }
            }
            else
            {
                var code = securityCodeCheck(item.SelectedDistrict);
                if (item.DistrictCode == code.First().securityCode)
                {
                    Session["leaID"] = item.SelectedDistrict;
                    Session["schoolID"] = "";
                    return View();
                }
                else
                {
                    return RedirectToAction("DistrictInformation", "Staff");
                }
              
            }

        }

        public ActionResult Create(StaffInformationViewModel info)
        {
            SimplerAES item = new SimplerAES();
            string hash = item.Encrypt(info.Password);
            info.PasswordHash = hash;
            db.Staffs.Add(new Staff
            {
                firstName = info.FirstName,
                lastName = info.LastName,
                positionTitle = info.PositionTitle,
                leaID = Session["leaID"].ToString(),
                schoolID = Session["schoolID"].ToString(),
                phoneNumber = info.PhoneNumber,
                email = info.Email,
                username = info.Username,
                password = info.PasswordHash
            });
            db.SaveChanges();
            return View();
        }
        public ActionResult ButtonCheck(StaffInformationViewModel item)
        {
            if (item.StaffType == "School")
            {

                return RedirectToAction("SchoolDistrictInformation", "Staff");
            }
            else
            {

                return RedirectToAction("DistrictInformation", "Staff");
            }
                
        }

        internal IQueryable<District> securityCodeCheck(string districtCode)
        {
            var query = from District in db.Districts
                        where District.leaID.Equals(districtCode)
                        select District;
            return query;
        }

        internal IQueryable<School> findSchools(string district)
        {
            var query = from School in db.Schools
                        where School.leaID.Equals(district)
                        select School;
            return query;
        }



    }
}
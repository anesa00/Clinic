﻿using Clinic.Data;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var doctors = _context.Doctors.ToList();
            return View(doctors);
        }
        public IActionResult Detail(int id)
        {
            Doctor doctor = _context.Doctors.FirstOrDefault(d => d.Id == id);
            return View(doctor);
        }
    }
}

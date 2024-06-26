﻿using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Clinic.Repository
{
    public class ReportRepository: IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Report report)
        {
            _context.Add(report);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public async Task<IEnumerable<Report>> GetReportsByPatientId(int id)
        {
            return await _context.Reports.Where(r => r.PatientId == id).ToListAsync();
        }

        public async Task<Report> GetReportById(int id)
        {
            return await _context.Reports.Include(p => p.Patient).Include(d => d.Doctor)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Report> GetReportByReception(Reception reception)
        {
            return await _context.Reports.Include(p => p.Patient).Include(d => d.Doctor)
                .FirstOrDefaultAsync(r => r.DateTime == reception.DateTime && reception.DoctorId == r.DoctorId && reception.PatientId == r.PatientId);
        }
    }
}

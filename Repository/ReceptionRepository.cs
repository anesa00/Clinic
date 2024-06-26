﻿using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Clinic.Repository
{
    public class ReceptionRepository : IReceptionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReceptionRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool Add(Reception reception)
        {
            _context.Add(reception);
            return Save();
        }

        public bool Delete(Reception reception)
        {
            _context.Remove(reception);
            return Save();
        }

        public async Task<IEnumerable<Reception>> GetAll()
        {
            var curUser = _httpContextAccessor.HttpContext?.User;
            var curUserId = curUser.GetUserId();
            if (curUser.IsInRole("nurse") || curUser.IsInRole("admin"))
                return await _context.Receptions.Include(p => p.Patient).Include(d => d.Doctor).ToListAsync();
            else
            {
                return await _context.Receptions.Include(p => p.Patient).Include(d => d.Doctor)
                    .Where(r => r.DoctorId == curUserId).ToListAsync();
            }       
        }

        public async Task<Reception> GetByIdAsync(int id)
        {
            return await _context.Receptions.Include(p => p.Patient).Include(d => d.Doctor).FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Reception> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Receptions.Include(p => p.Patient).Include(d => d.Doctor)
                                            .AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Reception>> GetReceptionsByDates(DateTime startDate, DateTime endDate)
        {
            return await _context.Receptions.Include(p => p.Patient).Include(d => d.Doctor)
                                            .Where(r => r.DateTime.Date >= startDate && r.DateTime.Date <= endDate)
                                            .ToListAsync();
                                                 
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Reception reception)
        {
            _context.Update(reception);
            return Save();
        }
    }
}

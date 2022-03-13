﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Core.ViewModels.Projection;
using Infrastructure.Models;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public class ProjectionService : IProjectionService
    {
        private readonly IRepository repository;

        private readonly IMapper mapper;

        public ProjectionService(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public async Task<(bool isCreated, DateTime date)> Create(CreateProjectionModel model)
        {

            bool isCreated = false;
            DateTime date;

            Projection projection = mapper.Map<Projection>(model);

            try
            {

                await repository.AddAsync(projection);
                await repository.SaveChangesAsync();

                date = DateTime.Parse(model.Date.ToString(), new CultureInfo("bg-bg"));
                isCreated = true;
            }
            catch (Exception)
            {
                date = DateTime.MinValue;
            }

            //TODO: Apply date exception handling

            return (isCreated, date);
        }

        public async Task<(bool isDeleted, DateTime date)> Delete(string id)
        {
            bool isDeleted = false;
            DateTime date;

            try
            {
                date = await GetDate(id);
                await repository.DeleteAsync<Projection>(id);
                await repository.SaveChangesAsync();
                isDeleted = true;
            }
            catch (Exception)
            {
                date = DateTime.MinValue;
            }

            return (isDeleted, date);
        }

        public async Task<bool> DeleteAllForMovie(string movieId)
        {
            try
            {
                repository.DeleteRange<Projection>(p => p.MovieId == movieId);
                await repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ListProjectionModel>> GetAllForDate(DateTime date)
        {
            IEnumerable<ListProjectionModel> projections = await repository.All<Projection>()
                .Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month && p.Date.Day == date.Day)
                .ProjectTo<ListProjectionModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return projections;
        }

        public async Task<IEnumerable<ListProjectionModel>> GetAllForMovie(string movieId)
        {
            IEnumerable<ListProjectionModel> projections = await repository.All<Projection>()
                .Where(p => p.MovieId == movieId)
                .ProjectTo<ListProjectionModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return projections;
        }

        public async Task<ProjectionDetails> GetProjectionById(string id)
        {
            Projection projection = await GetById(id);
            ProjectionDetails details = mapper.Map<ProjectionDetails>(projection);

            return details;
        }

        public async Task<ProjectionDetails> GetProjectionDetails(string id)
        {
            Projection projection = await GetById(id);
            ProjectionDetails details = mapper.Map<ProjectionDetails>(projection);

            return details;
        }

        public bool IsPlaceTaken(int seat)
        {
            bool isTaken = repository.AllReadonly<Ticket>()
                .Any(t => t.Seat == seat);

            return isTaken;
        }

        private async Task<Projection> GetById(string id)
        {
            Projection projection = await repository.GetByIdAsync<Projection>(id);

            return projection;
        }

        private async Task<DateTime> GetDate(string id)
        {
            Projection projection = await repository.GetByIdAsync<Projection>(id);
            DateTime date = projection.Date;

            return date;
        }
    }
}
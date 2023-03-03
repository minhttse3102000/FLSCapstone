using AutoMapper;
using AutoMapper.QueryableExtensions;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.RandomKey;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.ScheduleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Reso.Core.Utilities;
using System.Linq;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IGenericRepository<Schedule> _res;
        private readonly IMapper _mapper;

        public ScheduleService(IGenericRepository<Schedule> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<ScheduleViewModel> CreateSchedule(CreateScheduleRequest request)
        {
            try
            {
                var sch = _mapper.Map<Schedule>(request);
                sch.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(sch);
                await _res.SaveAsync();

                var scheduleVM = await GetScheduleById(sch.Id);
                return scheduleVM;
            }
            catch
            {
                return null;
            }
        }



        public async Task<bool> DeleteSchedule(string id)
        {
            var schedule = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)ScheduleStatus.Active)).FirstOrDefault();
            if (schedule == null)
            {
                return false;
            }
            schedule.Status = 0;
            await _res.UpdateAsync(schedule);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<ScheduleViewModel> GetAllSchedule(ScheduleViewModel flitter, int pageIndex, int pageSize, ScheduleSortBy sortBy, OrderBy order)
        {
            var listSchedule = _res.FindBy(x => x.Status == (int)FLSStatus.Active);


            var listScheduleViewModel = (listSchedule.ProjectTo<ScheduleViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "IsPublic":
                    if (order.ToString() == "Asc")
                    {
                        listScheduleViewModel = (IQueryable<ScheduleViewModel>)listScheduleViewModel.OrderBy(x => x.IsPublic);
                    }
                    else
                    {
                        listScheduleViewModel = (IQueryable<ScheduleViewModel>)listScheduleViewModel.OrderByDescending(x => x.IsPublic);
                    }
                    break;
                case "SemesterId":
                    if (order.ToString() == "Asc")
                    {
                        listScheduleViewModel = (IQueryable<ScheduleViewModel>)listScheduleViewModel.OrderBy(x => x.SemesterId);
                    }
                    else
                    {
                        listScheduleViewModel = (IQueryable<ScheduleViewModel>)listScheduleViewModel.OrderByDescending(x => x.SemesterId);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<ScheduleViewModel>(listScheduleViewModel, pageIndex, pageSize);
        }



        public async Task<ScheduleViewModel> GetScheduleById(string id)
        {
            var sche = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)ScheduleStatus.Active)
                .Include(x => x.Semester)
                .FirstOrDefaultAsync();
            if (sche == null)
                return null;
            var scheduleVM = _mapper.Map<ScheduleViewModel>(sche);
            return scheduleVM;
        }

        public async Task<ScheduleViewModel> UpdateSchedule(string id, UpdateScheduleRequest request)
        {
            try
            {
                var listSchedule = await _res.FindByAsync(x => x.Id == id && x.Status == (int)ScheduleStatus.Active);
                if (listSchedule == null)
                {
                    return null;
                }
                var schedule = listSchedule.FirstOrDefault();
                if (schedule == null)
                {
                    return null;
                }
                schedule = _mapper.Map(request, schedule);
                await _res.UpdateAsync(schedule);
                await _res.SaveAsync();

                var scheduleVM = await GetScheduleById(schedule.Id);
                return scheduleVM;
            }
            catch
            {
                return null;
            }

        }


    }
}

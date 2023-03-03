using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Reso.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.RandomKey;
using BEAPICapstoneProjectFLS.Requests.RoomSemesterRequest;

namespace BEAPICapstoneProjectFLS.Services
{
    public class RoomSemesterService : IRoomSemesterService
    {
        private readonly IGenericRepository<RoomSemester> _res;
        private readonly IMapper _mapper;

        public RoomSemesterService(IGenericRepository<RoomSemester> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<RoomSemesterViewModel> CreateRoomSemester(CreateRoomSemesterRequest request)
        {
            try
            {
                var rs = _mapper.Map<RoomSemester>(request);
                rs.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(rs);
                await _res.SaveAsync();

                var rsVM = await GetRoomSemesterById(rs.Id);
                return rsVM;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> DeleteRoomSemester(string id)
        {
            var roomSemester = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)RoomSemesterStatus.Active)).FirstOrDefault();
            if (roomSemester == null)
            {
                return false;
            }
            roomSemester.Status = 0;
            await _res.UpdateAsync(roomSemester);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<RoomSemesterViewModel> GetAllRoomSemester(RoomSemesterViewModel flitter, int pageIndex, int pageSize, RoomSemesterSortBy sortBy, OrderBy order)
        {
            var listRoomSemester = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listRoomSemesterViewModel = (listRoomSemester.ProjectTo<RoomSemesterViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "Quantity":
                    if (order.ToString() == "Asc")
                    {
                        listRoomSemesterViewModel = (IQueryable<RoomSemesterViewModel>)listRoomSemesterViewModel.OrderBy(x => x.Quantity);
                    }
                    else
                    {
                        listRoomSemesterViewModel = (IQueryable<RoomSemesterViewModel>)listRoomSemesterViewModel.OrderByDescending(x => x.Quantity);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listRoomSemesterViewModel = (IQueryable<RoomSemesterViewModel>)listRoomSemesterViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listRoomSemesterViewModel = (IQueryable<RoomSemesterViewModel>)listRoomSemesterViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<RoomSemesterViewModel>(listRoomSemesterViewModel, pageIndex, pageSize);
        }


        public async Task<RoomSemesterViewModel> GetRoomSemesterById(string id)
        {

            var dg = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)RoomSemesterStatus.Active)
                .Include(x => x.RoomType)
                .FirstOrDefaultAsync();
            if (dg == null)
                return null;
            var RoomSemesterVM = _mapper.Map<RoomSemesterViewModel>(dg);
            return RoomSemesterVM;
        }

        public async Task<RoomSemesterViewModel> UpdateRoomSemester(string id, UpdateRoomSemesterRequest request)
        {
            try
            {
                var listRoomSemester = await _res.FindByAsync(x => x.Id == id && x.Status == (int)RoomSemesterStatus.Active);
                if (listRoomSemester == null)
                {
                    return null;
                }
                var roomSemester = listRoomSemester.FirstOrDefault();
                if (roomSemester == null)
                {
                    return null;
                }
                roomSemester = _mapper.Map(request, roomSemester);
                await _res.UpdateAsync(roomSemester);
                await _res.SaveAsync();

                var rsVM = await GetRoomSemesterById(roomSemester.Id);
                return rsVM;
            }
            catch
            {
                return null;
            }
        }
    }
}

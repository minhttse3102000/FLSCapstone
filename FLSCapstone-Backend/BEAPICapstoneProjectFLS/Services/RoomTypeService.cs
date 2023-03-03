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
using BEAPICapstoneProjectFLS.Requests.RoomTypeRequest;

namespace BEAPICapstoneProjectFLS.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IGenericRepository<RoomType> _res;
        private readonly IMapper _mapper;

        public RoomTypeService(IGenericRepository<RoomType> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<RoomTypeViewModel> CreateRoomType(CreateRoomTypeRequest request)
        {
            try
            {
                var rt = _mapper.Map<RoomType>(request);
                rt.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(rt);
                await _res.SaveAsync();


                var rtVM = await GetRoomTypeById(rt.Id);
                return rtVM;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> DeleteRoomType(string id)
        {
            var roomType = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)RoomTypeStatus.Active)).FirstOrDefault();
            if (roomType == null)
            {
                return false;
            }
            roomType.Status = 0;
            await _res.UpdateAsync(roomType);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<RoomTypeViewModel> GetAllRoomType(RoomTypeViewModel flitter, int pageIndex, int pageSize, RoomTypeSortBy sortBy, OrderBy order)
        {
            var listRoomType = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listRoomTypeViewModel = (listRoomType.ProjectTo<RoomTypeViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "RoomTypeName":
                    if (order.ToString() == "Asc")
                    {
                        listRoomTypeViewModel = (IQueryable<RoomTypeViewModel>)listRoomTypeViewModel.OrderBy(x => x.RoomTypeName);
                    }
                    else
                    {
                        listRoomTypeViewModel = (IQueryable<RoomTypeViewModel>)listRoomTypeViewModel.OrderByDescending(x => x.RoomTypeName);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listRoomTypeViewModel = (IQueryable<RoomTypeViewModel>)listRoomTypeViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listRoomTypeViewModel = (IQueryable<RoomTypeViewModel>)listRoomTypeViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<RoomTypeViewModel>(listRoomTypeViewModel, pageIndex, pageSize);
        }


        public async Task<RoomTypeViewModel> GetRoomTypeById(string id)
        {

            var rt = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)RoomTypeStatus.Active)
                .FirstOrDefaultAsync();
            if (rt == null)
                return null;
            var RoomTypeVM = _mapper.Map<RoomTypeViewModel>(rt);
            return RoomTypeVM;
        }

        public async Task<RoomTypeViewModel> UpdateRoomType(string id, UpdateRoomTypeRequest request)
        {
            try
            {
                var listRoomType = await _res.FindByAsync(x => x.Id == id && x.Status == (int)RoomTypeStatus.Active);
                if (listRoomType == null)
                {
                    return null;
                }
                var roomType = listRoomType.FirstOrDefault();
                if (roomType == null)
                {
                    return null;
                }
                roomType = _mapper.Map(request, roomType);
                await _res.UpdateAsync(roomType);
                await _res.SaveAsync();

                var rtVM = await GetRoomTypeById(roomType.Id);
                return rtVM;
            }
            catch
            {
                return null;
            }
        }
    }
}

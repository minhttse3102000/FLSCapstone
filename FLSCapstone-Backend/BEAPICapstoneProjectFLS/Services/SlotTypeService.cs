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
using BEAPICapstoneProjectFLS.Requests.SlotTypeRequest;
using System;

namespace BEAPICapstoneProjectFLS.Services
{
    public class SlotTypeService : ISlotTypeService
    {
        private readonly IGenericRepository<SlotType> _res;
        private readonly IMapper _mapper;

        public SlotTypeService(IGenericRepository<SlotType> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<SlotTypeViewModel> CreateSlotType(CreateSlotTypeRequest request)
        {
            try
            {
                var st = _mapper.Map<SlotType>(request);
                st.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(st);
                await _res.SaveAsync();

                var stVM = await GetSlotTypeById(st.Id);
                return stVM;
            }
            catch
            {
                return null;
            }

        }



        public async Task<ApiResponse> CreateListSlotType(string semesterID, List<CreateSlotTypeInSemesterRequest> requests)
        {
            try
            {

                foreach (var cr in requests)
                {
                    var newSlotType = _mapper.Map<SlotType>(cr);
                    newSlotType.SemesterId = semesterID;
                    newSlotType.Id = RandomPKKey.NewRamDomPKKey();
                    await _res.InsertAsync(newSlotType);
                    await _res.SaveAsync();
                }
                return new ApiResponse
                {
                    Success = true,
                    Message = "CreateListSlotType success"
                };

            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Success = true,
                    Message = "CreateListSlotType fail",
                    Data = ex.Message   
                };
            }
        }

        public async Task<bool> DeleteSlotType(string id)
        {
            var slotType = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)SlotTypeStatus.Active)).FirstOrDefault();
            if (slotType == null)
            {
                return false;
            }
            slotType.Status = 0;
            await _res.UpdateAsync(slotType);
            await _res.SaveAsync();
            return true;
        }


        public async Task<bool> DeleteListSlotTypeInSemester(string SemesterID)
        {
            try
            {
                var listSlotType = await _res.GetAllByIQueryable().Where(x => x.Status == (int)SlotTypeStatus.Active)
                    .Where(x => x.SemesterId == SemesterID)
                    .ToListAsync();
                if (listSlotType.Count <= 0)
                {
                    return false;
                }
                else
                {
                    foreach (var SlotType in listSlotType)
                    {
                        await _res.DeleteAsync(SlotType.Id);
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }


        public IPagedList<SlotTypeViewModel> GetAllSlotType(SlotTypeViewModel flitter, int pageIndex, int pageSize, SlotTypeSortBy sortBy, OrderBy order)
        {
            var listSlotType = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listSlotTypeViewModel = (listSlotType.ProjectTo<SlotTypeViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "SlotNumber":
                    if (order.ToString() == "Asc")
                    {
                        listSlotTypeViewModel = (IQueryable<SlotTypeViewModel>)listSlotTypeViewModel.OrderBy(x => x.SlotNumber);
                    }
                    else
                    {
                        listSlotTypeViewModel = (IQueryable<SlotTypeViewModel>)listSlotTypeViewModel.OrderByDescending(x => x.SlotNumber);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listSlotTypeViewModel = (IQueryable<SlotTypeViewModel>)listSlotTypeViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listSlotTypeViewModel = (IQueryable<SlotTypeViewModel>)listSlotTypeViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
                case "DayOfWeekAndTimeStart":

                    if (order.ToString() == "Asc")
                    {
                        listSlotTypeViewModel = (IQueryable<SlotTypeViewModel>)listSlotTypeViewModel.OrderBy(x => x.DateOfWeek).ThenBy(slot => slot.SlotNumber);
                    }
                    else
                    {
                        listSlotTypeViewModel = (IQueryable<SlotTypeViewModel>)listSlotTypeViewModel.OrderByDescending(x => x.DateOfWeek).ThenBy(slot => slot.SlotNumber);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<SlotTypeViewModel>(listSlotTypeViewModel, pageIndex, pageSize);
        }


        public async Task<SlotTypeViewModel> GetSlotTypeById(string id)
        {

            //var st = await _res.GetAllByIQueryable()
            //    .Where(x => x.Id == id && x.Status == (int)SlotTypeStatus.Active)
            //    .Include(x => x.Semester)
            //    .FirstOrDefaultAsync();

            var st = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)SlotTypeStatus.Active)
                .Include(x => x.Semester)
                .ToListAsync();
            if (st == null)
                return null;
            var SlotTypeVM = _mapper.Map<SlotTypeViewModel>(st);
            return SlotTypeVM;
        }

        public async Task<SlotTypeViewModel> UpdateSlotType(string id, UpdateSlotTypeRequest request)
        {
            try
            {
                var listSlotType = await _res.FindByAsync(x => x.Id == id && x.Status == (int)SlotTypeStatus.Active);
                if (listSlotType == null)
                {
                    return null;
                }
                var slotType = listSlotType.FirstOrDefault();
                if (slotType == null)
                {
                    return null;
                }
                slotType = _mapper.Map(request, slotType);
                await _res.UpdateAsync(slotType);
                await _res.SaveAsync();

                var stVM = await GetSlotTypeById(slotType.Id);
                return stVM;
            }
            catch
            {
                return null;
            }
        }



    }
}

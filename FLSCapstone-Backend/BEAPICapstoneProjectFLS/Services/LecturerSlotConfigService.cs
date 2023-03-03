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
using BEAPICapstoneProjectFLS.Requests.LecturerSlotConfigRequest;
using System;

namespace BEAPICapstoneProjectFLS.Services
{
    public class LecturerSlotConfigService : ILecturerSlotConfigService
    {
        private readonly IGenericRepository<LecturerSlotConfig> _res;
        private readonly IGenericRepository<SlotType> _resSlotType;
        private readonly IGenericRepository<User> _resUser;
        private readonly IMapper _mapper;

        public LecturerSlotConfigService(IGenericRepository<LecturerSlotConfig> repository, IGenericRepository<SlotType> slotTypeRepository, IGenericRepository<User> userRepository, IMapper mapper)
        {
            _res = repository;
            _resSlotType = slotTypeRepository;
            _resUser = userRepository;  
            _mapper = mapper;
        }

        public async Task<LecturerSlotConfigViewModel> CreateLecturerSlotConfig(CreateLecturerSlotConfigRequest request)
        {
            try
            {
                var lsc = _mapper.Map<LecturerSlotConfig>(request);
                lsc.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(lsc);
                await _res.SaveAsync();

                var lscVM = await GetLecturerSlotConfigById(lsc.Id);
                return lscVM;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> DeleteLecturerSlotConfig(string id)
        {
            var lecturerSlotConfig = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)LecturerSlotConfigStatus.Active)).FirstOrDefault();
            if (lecturerSlotConfig == null)
            {
                return false;
            }
            lecturerSlotConfig.Status = 0;
            await _res.UpdateAsync(lecturerSlotConfig);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<LecturerSlotConfigViewModel> GetAllLecturerSlotConfig(LecturerSlotConfigViewModel flitter, int pageIndex, int pageSize, LecturerSlotConfigSortBy sortBy, OrderBy order)
        {
            var listLecturerSlotConfig = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listLecturerSlotConfigViewModel = (listLecturerSlotConfig.ProjectTo<LecturerSlotConfigViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "LecturerId":
                    if (order.ToString() == "Asc")
                    {
                        listLecturerSlotConfigViewModel = (IQueryable<LecturerSlotConfigViewModel>)listLecturerSlotConfigViewModel.OrderBy(x => x.LecturerId);
                    }
                    else
                    {
                        listLecturerSlotConfigViewModel = (IQueryable<LecturerSlotConfigViewModel>)listLecturerSlotConfigViewModel.OrderByDescending(x => x.LecturerId);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listLecturerSlotConfigViewModel = (IQueryable<LecturerSlotConfigViewModel>)listLecturerSlotConfigViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listLecturerSlotConfigViewModel = (IQueryable<LecturerSlotConfigViewModel>)listLecturerSlotConfigViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<LecturerSlotConfigViewModel>(listLecturerSlotConfigViewModel, pageIndex, pageSize);
        }


        public async Task<LecturerSlotConfigViewModel> GetLecturerSlotConfigById(string id)
        {

            var lsc = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)LecturerSlotConfigStatus.Active)
                .Include(x => x.SlotType)
                .Include(x => x.Semester)
                .Include(x => x.Lecturer)
                .FirstOrDefaultAsync();
            if (lsc == null)
                return null;
            var lecturerSlotConfigVM = _mapper.Map<LecturerSlotConfigViewModel>(lsc);
            return lecturerSlotConfigVM;
        }

        public async Task<LecturerSlotConfigViewModel> UpdateLecturerSlotConfig(string id, UpdateLecturerSlotConfigRequest request)
        {
            try
            {
                var listLecturerSlotConfig = await _res.FindByAsync(x => x.Id == id && x.Status == (int)LecturerSlotConfigStatus.Active);
                if (listLecturerSlotConfig == null)
                {
                    return null;
                }
                var lecturerSlotConfig = listLecturerSlotConfig.FirstOrDefault();
                if (lecturerSlotConfig == null)
                {
                    return null;
                }
                lecturerSlotConfig = _mapper.Map(request, lecturerSlotConfig);
                await _res.UpdateAsync(lecturerSlotConfig);
                await _res.SaveAsync();

                var lscVM = await GetLecturerSlotConfigById(lecturerSlotConfig.Id);
                return lscVM;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ApiResponse> CreateSlotTypesAndLecturerSlotConfigsInSemester(string semesterID)
        {


            try
            {
                List<SlotType> listSlotType = new List<SlotType>()
                {
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST11", TimeStart = new TimeSpan(7,0,0), TimeEnd =new TimeSpan(9,15,0), SlotNumber =1, DateOfWeek=36,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST12", TimeStart = new TimeSpan(9,30,0), TimeEnd =new TimeSpan(11,45,0), SlotNumber =2, DateOfWeek=36,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST13", TimeStart = new TimeSpan(12,30,0), TimeEnd =new TimeSpan(14,45,0), SlotNumber =3, DateOfWeek=36,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST14", TimeStart = new TimeSpan(15,0,0), TimeEnd =new TimeSpan(17,15,0), SlotNumber =4, DateOfWeek=36,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST21", TimeStart = new TimeSpan(7,0,0), TimeEnd =new TimeSpan(9,15,0), SlotNumber =1, DateOfWeek=72,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST22", TimeStart = new TimeSpan(9,30,0), TimeEnd =new TimeSpan(11,45,0), SlotNumber =2, DateOfWeek=72,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST23", TimeStart = new TimeSpan(12,30,0), TimeEnd =new TimeSpan(14,45,0), SlotNumber =3, DateOfWeek=72,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST24", TimeStart = new TimeSpan(15,0,0), TimeEnd =new TimeSpan(17,15,0), SlotNumber =4, DateOfWeek=72,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST31", TimeStart = new TimeSpan(7,0,0), TimeEnd =new TimeSpan(9,15,0), SlotNumber =1, DateOfWeek=144,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST32", TimeStart = new TimeSpan(9,30,0), TimeEnd =new TimeSpan(11,45,0), SlotNumber =2, DateOfWeek=144,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST33", TimeStart = new TimeSpan(12,30,0), TimeEnd =new TimeSpan(14,45,0), SlotNumber =3, DateOfWeek=144,SemesterId = semesterID ,Status=1 },
                    new SlotType{Id= RandomPKKey.NewRamDomPKKey(), SlotTypeCode = "ST34", TimeStart = new TimeSpan(15,0,0), TimeEnd =new TimeSpan(17,15,0), SlotNumber =4, DateOfWeek=144,SemesterId = semesterID ,Status=1 },
                };
                foreach (var slotType in listSlotType)
                {
                    await _resSlotType.InsertAsync(slotType);
                    await _resSlotType.SaveAsync();
                }


                var listUser = _resUser.FindBy(x => x.Status == (int)FLSStatus.Active);


                UserViewModel flitter = new UserViewModel { RoleIDs = new List<string>() { "LC" } };
                var listLecturer = await (listUser.ProjectTo<UserViewModel>
                    (_mapper.ConfigurationProvider)).DynamicFilter(flitter).ToListAsync();

                foreach (var lec in listLecturer)
                {
                    foreach (var slotTypeOfLecSlotConfig in listSlotType)
                    {
                        LecturerSlotConfig lecturerSlotConfig = new LecturerSlotConfig { Id = RandomPKKey.NewRamDomPKKey(), SlotTypeId = slotTypeOfLecSlotConfig.Id, LecturerId = lec.Id, SemesterId = semesterID, PreferenceLevel = 0, IsEnable = 1, Status = 1 };
                        await _res.InsertAsync(lecturerSlotConfig);
                        await _res.SaveAsync();
                    }

                }

                return new ApiResponse
                {
                    Success = true,
                    Message = "Create SlotTypes And LecturerSlotConfigs In Semester Success",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Create SlotTypes And LecturerSlotConfigs In Semester Fail",
                    Data = ex.Message
                };
            }
            

        }

        public async Task<ApiResponse> DeleteLecturerSlotConfigInSemester(string semesterID)
        {
            try
            {
                var listLecturerSlotConfig = await _res.GetAllByIQueryable()
                    .Where(x => x.SemesterId == semesterID && x.Status == (int)LecturerSlotConfigStatus.Active)
                    .ToListAsync();
                if (listLecturerSlotConfig.Count == 0)
                {
                    return new ApiResponse()
                    {
                        Success = true,
                        Message = "Delete LecturerSlotConfig In Semester Success",
                        Data = "List already is empty"
                    };
                }
                else
                {
                    foreach (var SlotType in listLecturerSlotConfig)
                    {
                        await _res.DeleteAsync(SlotType.Id);
                    }
                    return new ApiResponse()
                    {
                        Success = true,
                        Message = "Delete LecturerSlotConfig In Semester Success"
                    };
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Success = false,
                    Message = "Delete LecturerSlotConfig In Semester Fail",
                    Data = ex.Message
                };
            }
        }
    }
}

using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BEAPICapstoneProjectFLS.Enum;
using Microsoft.EntityFrameworkCore;

namespace BEAPICapstoneProjectFLS.Services
{
    public class SwapService : ISwapService
    {
        private readonly IGenericRepository<SlotType> _resSlotType;
        private readonly IGenericRepository<CourseAssign> _resCourseAssign;
        private readonly IMapper _mapper;

        public SwapService(IGenericRepository<SlotType> slotTypeRepository, IGenericRepository<CourseAssign> courseAssignRepository, IMapper mapper)
        {
            _resSlotType = slotTypeRepository;
            _resCourseAssign = courseAssignRepository;
            _mapper = mapper;
        }
        public async Task<List<SlotType>> checkGroup(List<SlotType> listBlankSlotType, string scheduleID, string courseID)
        {
            var listCourseAssign = await _resCourseAssign.GetAllByIQueryable()
                    .Where(x => x.ScheduleId == scheduleID && x.Status == (int)CourseAssignStatus.Active)
                    .ToListAsync();

            List<SlotType> result= new List<SlotType>();
            foreach (var st in listBlankSlotType)
            {
                result.Add(st);
            }

            string GroupStudentCode = courseID.Split('_')[1];
            foreach (var courseAssign in listCourseAssign)
            {
                string s= courseAssign.CourseId.Split('_')[1];
                if(GroupStudentCode == s)
                {
                    foreach (var item in listBlankSlotType)
                    {
                        if(item.Id == courseAssign.SlotTypeId)
                        {
                            result.Remove(item);
                        }
                    }
                }
            }

            return result;
        }
        public async Task<IEnumerable<SlotTypeViewModel>> GetBlankSlot(string courseAssignID)
        {
            var ca = await _resCourseAssign.GetAllByIQueryable()
                        .Where(x => x.Id == courseAssignID && x.Status == (int)CourseAssignStatus.Active)
                        .Include(x => x.Lecturer)
                        .Include(x => x.Schedule)
                        .Include(x => x.Course)
                        .FirstOrDefaultAsync();


            //string lecturerID, string semesterID, string scheduleID, string courseID;


            var listSlotType = await _resSlotType.GetAllByIQueryable()
                      .Where(x => x.SemesterId == ca.Schedule.SemesterId && x.Status == (int)SlotTypeStatus.Active)
                      .Include(x => x.Semester)
                      .ToListAsync();

            var listCourseAssign = await _resCourseAssign.GetAllByIQueryable()
                .Where(x => x.LecturerId == ca.LecturerId && x.ScheduleId == ca.ScheduleId && x.Status == (int)CourseAssignStatus.Active)
                .ToListAsync();

            List<SlotType> listBlankSlotType = new List<SlotType>();
            foreach(var slotType in listSlotType)
            {
                listBlankSlotType.Add(slotType);
                foreach (var courseAssign in listCourseAssign)
                {
                    if(slotType.Id == courseAssign.SlotTypeId)
                    {
                        listBlankSlotType.Remove(slotType);
                    }
                }

            }


            listBlankSlotType = await checkGroup(listBlankSlotType, ca.ScheduleId, ca.CourseId);
            return _mapper.Map<IEnumerable<SlotTypeViewModel>>(listBlankSlotType.OrderBy(x => x.SlotTypeCode));

            #region
            /*
            //string lecturerID, string semesterID, string scheduleID, string courseID;


            var listSlotType = await _resSlotType.GetAllByIQueryable()
                      .Where(x => x.SemesterId == semesterID && x.Status == (int)SlotTypeStatus.Active)
                      .ToListAsync();

            var listCourseAssign = await _resCourseAssign.GetAllByIQueryable()
                .Where(x => x.LecturerId == lecturerID && x.ScheduleId == scheduleID && x.Status == (int)CourseAssignStatus.Active)
                .ToListAsync();

            List<SlotType> listBlankSlotType = new List<SlotType>();
            foreach (var slotType in listSlotType)
            {
                listBlankSlotType.Add(slotType);
                foreach (var courseAssign in listCourseAssign)
                {
                    if (slotType.Id == courseAssign.SlotTypeId)
                    {
                        listBlankSlotType.Remove(slotType);
                    }
                }

            }


            listBlankSlotType = await checkGroup(listBlankSlotType, scheduleID, courseID);
            return _mapper.Map<IEnumerable<SlotTypeViewModel>>(listBlankSlotType);*/
            #endregion
        }

        public async Task<bool> checkSwapCourseAssign(string slotTypeID, CourseAssign courseAssign)
        {
            string GroupID= courseAssign.CourseId.Split('_')[1];
            var courseAssigns = await _resCourseAssign.GetAllByIQueryable()
                            .Where(x => x.ScheduleId == courseAssign.ScheduleId &&  x.Status == (int)CourseAssignStatus.Active)
                            .Include(x => x.Lecturer)
                            .Include(x => x.Course)
                            .Include(x => x.SlotType)
                            .ToListAsync();

            foreach (var ca in courseAssigns)
            {
                string s = ca.CourseId.Split('_')[1];
                if (s == GroupID)
                {
                    if(slotTypeID == ca.SlotTypeId)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<IEnumerable<CourseAssignViewModel>> GetCourseAssignToSwap(string courseAssignID)
        {
            List<CourseAssign> result = new List<CourseAssign>();
            var courseAssign = await _resCourseAssign.GetAllByIQueryable()
                        .Where(x => x.Id == courseAssignID && x.Status == (int)CourseAssignStatus.Active)
                        .Include(x => x.Lecturer)
                        .Include(x => x.SlotType)
                        .Include(x => x.Course)
                        .FirstOrDefaultAsync();

            var listCourseAssign = await _resCourseAssign.GetAllByIQueryable()
                        .Where(x => x.Id != courseAssignID && x.LecturerId ==   courseAssign.LecturerId && x.Status == (int)CourseAssignStatus.Active)
                        .Include(x => x.Lecturer)
                        .Include(x => x.SlotType)
                        .Include(x => x.Course)
                        .ToListAsync();
 
            foreach (CourseAssign ca in listCourseAssign)
            {
                bool rsFrom = await checkSwapCourseAssign(courseAssign.SlotTypeId, ca);
                bool rsTo = await checkSwapCourseAssign(ca.SlotTypeId, courseAssign);
                if (rsFrom == true && rsTo == true)
                {
                    result.Add(ca);
                }


            }

            return _mapper.Map<IEnumerable<CourseAssignViewModel>>(result.OrderBy(x => x.SlotType.SlotTypeCode));
        }


    }
}

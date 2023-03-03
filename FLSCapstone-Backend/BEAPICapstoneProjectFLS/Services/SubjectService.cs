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
using BEAPICapstoneProjectFLS.Requests.SubjectRequest;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BEAPICapstoneProjectFLS.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IGenericRepository<Subject> _res;
        private readonly IMapper _mapper;

        public SubjectService(IGenericRepository<Subject> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<SubjectViewModel> CreateSubject(CreateSubjectRequest request)
        {
            try
            {
                var su = _mapper.Map<Subject>(request);
                await _res.InsertAsync(su);
                await _res.SaveAsync();

                var suVM = await GetSubjectById(su.Id);
                return suVM;
            }
            catch
            {
                return null;
            }

        }

        /*public async Task<ApiResponse> CreateListSubject(List<CreateSubjectRequest> requests)
        {
            try
            {
                foreach (var request in requests)
                {
                    var su = _mapper.Map<Subject>(request);
                    su.Id = RandomPKKey.NewRamDomPKKey();
                    await _res.InsertAsync(su);
                    await _res.SaveAsync();
                }
                return new ApiResponse
                {
                    Success = true,
                    Message = "CreateListSubject success"
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Success = true,
                    Message = "CreateListSubject fail",
                    Data = ex.Message
                };
            }
        }*/


        public async Task<bool> DeleteSubject(string id)
        {
            var subject = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)SubjectStatus.Active)).FirstOrDefault();
            if (subject == null)
            {
                return false;
            }
            subject.Status = 0;
            await _res.UpdateAsync(subject);
            await _res.SaveAsync();
            return true;
        }


        public IPagedList<SubjectViewModel> GetAllSubject(SubjectViewModel flitter, int pageIndex, int pageSize, SubjectSortBy sortBy, OrderBy order)
        {
            var listSubject = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listSubjectViewModel = (listSubject.ProjectTo<SubjectViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "SubjectName":
                    if (order.ToString() == "Asc")
                    {
                        listSubjectViewModel = (IQueryable<SubjectViewModel>)listSubjectViewModel.OrderBy(x => x.SubjectName);
                    }
                    else
                    {
                        listSubjectViewModel = (IQueryable<SubjectViewModel>)listSubjectViewModel.OrderByDescending(x => x.SubjectName);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listSubjectViewModel = (IQueryable<SubjectViewModel>)listSubjectViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listSubjectViewModel = (IQueryable<SubjectViewModel>)listSubjectViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<SubjectViewModel>(listSubjectViewModel, pageIndex, pageSize);
        }


        public async Task<SubjectViewModel> GetSubjectById(string id)
        {

            var su = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)SubjectStatus.Active)
                .Include(x => x.Department)
                .FirstOrDefaultAsync();
            if (su == null)
                return null;
            var subjectVM = _mapper.Map<SubjectViewModel>(su);
            return subjectVM;
        }

        public async Task<SubjectViewModel> UpdateSubject(string id, UpdateSubjectRequest request)
        {
            try
            {
                var listSubject = await _res.FindByAsync(x => x.Id == id && x.Status == (int)SubjectStatus.Active);
                if (listSubject == null)
                {
                    return null;
                }
                var subject = listSubject.FirstOrDefault();
                if (subject == null)
                {
                    return null;
                }
                subject = _mapper.Map(request, subject);
                await _res.UpdateAsync(subject);
                await _res.SaveAsync();

                var suVM = await GetSubjectById(subject.Id);
                return suVM;
            }
            catch
            {
                return null;
            }
        }
    }
}

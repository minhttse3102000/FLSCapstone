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
using BEAPICapstoneProjectFLS.Requests.Request;
using System;

namespace BEAPICapstoneProjectFLS.Services
{
    public class RequestService : IRequestService
    {
        private readonly IGenericRepository<Request> _res;
        private readonly IMapper _mapper;

        public RequestService(IGenericRepository<Request> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;

        }

        public async Task<RequestViewModel> CreateRequest(CreateRequest request)
        {
            try
            {
                var re = _mapper.Map<Request>(request);
                re.Id = RandomPKKey.NewRamDomPKKey();
                re.DateCreate = DateTime.Now;
                re.ResponseState = 0;
                await _res.InsertAsync(re);
                await _res.SaveAsync();

                var reVM = await GetRequestById(re.Id);
                return reVM;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> DeleteRequest(string id)
        {
            var request = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)RequestStatus.Active)).FirstOrDefault();
            if (request == null)
            {
                return false;
            }
            request.Status = 0;
            await _res.UpdateAsync(request);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<RequestViewModel> GetAllRequest(RequestViewModel flitter, int pageIndex, int pageSize, RequestSortBy sortBy, OrderBy order)
        {
            var listRequest = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listRequestViewModel = (listRequest.ProjectTo<RequestViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "Title":
                    if (order.ToString() == "Asc")
                    {
                        listRequestViewModel = (IQueryable<RequestViewModel>)listRequestViewModel.OrderBy(x => x.Title);
                    }
                    else
                    {
                        listRequestViewModel = (IQueryable<RequestViewModel>)listRequestViewModel.OrderByDescending(x => x.Title);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listRequestViewModel = (IQueryable<RequestViewModel>)listRequestViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listRequestViewModel = (IQueryable<RequestViewModel>)listRequestViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
                case "DateCreate":
                    if (order.ToString() == "Asc")
                    {
                        listRequestViewModel = (IQueryable<RequestViewModel>)listRequestViewModel.OrderBy(x => x.DateCreate);
                    }
                    else
                    {
                        listRequestViewModel = (IQueryable<RequestViewModel>)listRequestViewModel.OrderByDescending(x => x.DateCreate);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<RequestViewModel>(listRequestViewModel, pageIndex, pageSize);
        }


        public async Task<RequestViewModel> GetRequestById(string id)
        {
            var listRequest = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listRequestViewModel = (listRequest.ProjectTo<RequestViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(new RequestViewModel { Id = id });

            var re = await listRequestViewModel.FirstOrDefaultAsync();
            if (re == null)
                return null;
            return re;
            /*try
            {
                 var re = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)RequestStatus.Active)
                .Include(l => l.Lecturer)
                .Include(d => d.DepartmentManager)
                .Include(x => x.Semester)
                .Include(x => x.Subject).FirstOrDefaultAsync();
                re.DepartmentManager = await _resDepartmentManager.FindAsync(x => x.Id == re.DepartmentManagerId && x.Status == (int)UserStatus.Active);
                if (re == null)
                    return null;
                var RequestVM = _mapper.Map<RequestViewModel>(re);
                return RequestVM;

            }
            catch (Exception ex)
            {
                var error = ex.Message ;
                return null;
            }*/

        }

        public async Task<RequestViewModel> UpdateRequest(string id, UpdateRequest request)
        {
            try
            {
                var listRequest = await _res.FindByAsync(x => x.Id == id && x.Status == (int)RequestStatus.Active);
                if (listRequest == null)
                {
                    return null;
                }
                var upRequest = listRequest.FirstOrDefault();
                if (upRequest == null)
                {
                    return null;
                }
                upRequest = _mapper.Map(request, upRequest);
                upRequest.DateRespone = DateTime.Now;
                await _res.UpdateAsync(upRequest);
                await _res.SaveAsync();

                var reVM = await GetRequestById(upRequest.Id);
                return reVM;
            }
            catch
            {
                return null;
            }


        }


        public async Task<ApiResponse> DeleteRequestInSemester(string semesterID)
        {
            try
            {
                var listRequests = await _res.GetAllByIQueryable()
                    .Where(x => x.SemesterId == semesterID && x.Status == (int)RequestStatus.Active)
                    .ToListAsync();
                if (listRequests.Count == 0)
                {
                    return new ApiResponse()
                    {
                        Success = true,
                        Message = "Delete LecturerCourseGroup In Semester Success",
                        Data = "List already is empty"
                    };
                }
                else
                {
                    foreach (var request in listRequests)
                    {
                        await _res.DeleteAsync(request.Id);
                    }
                    return new ApiResponse()
                    {
                        Success = true,
                        Message = "Delete Requests In Semester Success"
                    };
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Success = false,
                    Message = "Delete Requests In Semester Fail",
                    Data = ex.Message
                };
            }
        }
    }
}

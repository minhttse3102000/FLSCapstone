using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.SubjectRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ISubjectService
    {
        Task<SubjectViewModel> GetSubjectById(string id);
        IPagedList<SubjectViewModel> GetAllSubject(SubjectViewModel flitter, int pageIndex,
           int pageSize, SubjectSortBy sortBy, OrderBy order);
        Task<SubjectViewModel> CreateSubject(CreateSubjectRequest request);
        Task<SubjectViewModel> UpdateSubject(string id, UpdateSubjectRequest request);
        Task<bool> DeleteSubject(string id);
    }
}

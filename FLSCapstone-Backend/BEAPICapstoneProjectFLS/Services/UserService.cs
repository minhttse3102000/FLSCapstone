using AutoMapper;
using AutoMapper.QueryableExtensions;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.RandomKey;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.UserRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Reso.Core.Utilities;
using System.Linq;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _res;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<UserViewModel> CreateUser(CreateUserRequest request)
        {
            try
            {
                var user = _mapper.Map<User>(request);
                string[] strS = request.Email.Split('@');
                user.Id = strS[0];
                await _res.InsertAsync(user);
                await _res.SaveAsync();

                var userVM = await GetUserById(user.Id);
                return userVM;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)UserStatus.Active)).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            user.Status = 0;
            await _res.UpdateAsync(user);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<UserViewModel> GetAllUser(UserViewModel flitter, int pageIndex, int pageSize, UserSortBy sortBy, OrderBy order)
        {
            var listUser = _res.FindBy(x => x.Status == (int)FLSStatus.Active);


            var listUserViewModel = (listUser.ProjectTo<UserViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "Name":
                    if (order.ToString() == "Asc")
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderBy(x => x.Name);
                    }
                    else
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderByDescending(x => x.Name);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
                case "Email":
                    if (order.ToString() == "Asc")
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderBy(x => x.Email);
                    }
                    else
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
                case "PriorityLecturer":
                    if (order.ToString() == "Asc")
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderBy(x => x.PriorityLecturer);
                    }
                    else
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderByDescending(x => x.PriorityLecturer);
                    }
                    break;
                case "IsFullTime":
                    if (order.ToString() == "Asc")
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderBy(x => x.IsFullTime);
                    }
                    else
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderByDescending(x => x.IsFullTime);
                    }
                    break;
                case "DepartmentId":
                    if (order.ToString() == "Asc")
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderBy(x => x.DepartmentId);
                    }
                    else
                    {
                        listUserViewModel = (IQueryable<UserViewModel>)listUserViewModel.OrderByDescending(x => x.DepartmentId);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<UserViewModel>(listUserViewModel, pageIndex, pageSize);
        }

        public async Task<UserViewModel> GetUserById(string id)
        {
            var user = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)UserStatus.Active)
                .Include(x => x.Department)
                .Include(x => x.UserAndRoles)
                .FirstOrDefaultAsync();
            if (user == null)
                return null;
            var userVM = _mapper.Map<UserViewModel>(user);
            return userVM;
        }
        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            var user = await _res.GetAllByIQueryable()
                .Where(x => x.Email == email && x.Status == (int)UserStatus.Active)
                .Include(x => x.Department)
                .Include(x => x.UserAndRoles)
                .FirstOrDefaultAsync();
            if (user == null)
                return null;
            var userVM = _mapper.Map<UserViewModel>(user);
            return userVM;
        }

        public async Task<UserViewModel> UpdateUser(string id, UpdateUserRequest request)
        {
            try
            {
                var listUser = await _res.FindByAsync(x => x.Id == id && x.Status == (int)UserStatus.Active);
                if (listUser == null)
                {
                    return null;
                }
                var user = listUser.FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                user = _mapper.Map(request, user);
                await _res.UpdateAsync(user);
                await _res.SaveAsync();

                var userVM = await GetUserById(user.Id);
                return userVM;
            }
            catch
            {
                return null;
            }

        }

        //public async Task<IEnumerable<LecturerViewModel>> GetAllLecturerByDepartmentID(string departmentID)
        //{
        //    var lectueres = await _res.GetAllByIQueryable().Where(x => x.Status == (int)LecturerStatus.Active)
        //        .Include(b => b.Department)
        //        .Where(b => b.DepartmentId == departmentID).ToListAsync();
        //    return _mapper.Map<IEnumerable<LecturerViewModel>>(lectueres);
        //}
    }
}

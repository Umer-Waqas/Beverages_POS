

namespace Restaurant_MS_Infrastructure.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployee
    {
        AppDbContext cxt = null;
        public EmployeeRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public decimal GetNewMrNo()
        {
            decimal? MrNo = 0;
            return (decimal)(MrNo == null ? 1 : MrNo + 1);
        }

        public List<Employee> GetActiveEmployees()
        {
            return cxt.Employees
                .Where(p => p.IsActive)
                .ToList();
        }
        public IPagedList<Employee> GetEmployeesAsync(int pageNo, int pageSize)
        {
            return cxt.Employees.Where(p => p.IsActive).OrderByDescending(p => p.EmployeeId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<Employee> GetEmployeesAsync_ByTag(int TagId, int pageNo, int pageSize)
        {

            return (from p in cxt.Employees
                    where p.IsActive
                    orderby p.EmployeeId descending
                    select p).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<Employee> GetEmployeesAsync_BySearch(string SearchString, int pageNo, int pageSize)
        {
            return (from p in cxt.Employees
                    where p.IsActive && (p.Name.Contains(SearchString) ||
                    p.Phone.Contains(SearchString) ||
                    p.Phone2.Contains(SearchString) ||
                    p.Phone3.Contains(SearchString) ||
                    p.Email.Contains(SearchString))
                    orderby p.EmployeeId descending
                    select p).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<Employee> GetEmployeesAsync_ByAdvancedSearch(string SearchString, string SearchBy, int pageNo, int pageSize)
        {
            switch (SearchBy)
            {
                case "Name":
                    return (from p in cxt.Employees
                            where p.IsActive && p.Name.Contains(SearchString)
                            orderby p.EmployeeId descending
                            select p).ToPagedList(pageNo, pageSize);
                case "MR#":
                    return (from p in cxt.Employees
                            where p.IsActive
                            orderby p.EmployeeId descending
                            select p).ToPagedList(pageNo, pageSize);
                case "Email":
                    return (from p in cxt.Employees
                            where p.IsActive && p.Email.Contains(SearchString)
                            orderby p.EmployeeId descending
                            select p).ToPagedList(pageNo, pageSize);

                case "Phone":
                    return (from p in cxt.Employees
                            where p.IsActive && p.Phone.Contains(SearchString)
                            orderby p.EmployeeId descending
                            select p).ToPagedList(pageNo, pageSize);

                case "Date Of Birth":
                    return (from p in cxt.Employees
                            where p.IsActive && p.DateOfBirth.ToString().Contains(SearchString)
                            orderby p.EmployeeId descending
                            select p).ToPagedList(pageNo, pageSize);

                case "Address":
                    return (from p in cxt.Employees
                            where p.IsActive && p.Address.Contains(SearchString)
                            orderby p.EmployeeId descending
                            select p).ToPagedList(pageNo, pageSize);

                case "Referred By":
                    return (from p in cxt.Employees
                            where p.IsActive && p.ReferredBy.Contains(SearchString)
                            orderby p.EmployeeId descending
                            select p).ToPagedList(pageNo, pageSize);

            }
            return null;
        }
        public IPagedList<EmployeeSearchVM> SearchEmployee(bool IsByName, bool IsByPhone, bool IsByMrNo, string SearchString, int pageNo)
        {
            IPagedList<EmployeeSearchVM> Result = null;
            if (IsByName)
            {
                return Result = cxt.Employees
                     .Where(p => p.IsActive)
                     .Where(p => p.Name.ToLower().Contains(SearchString))
                     .Select(p => new EmployeeSearchVM()
                     {
                         EmployeeId = p.EmployeeId,
                         EmployeeName = p.Name,
                         Phone = p.Phone,
                     }).OrderBy(p => p.EmployeeId).ToPagedList(pageNo, 20);
            }

            if (IsByPhone)
            {
                return Result = cxt.Employees
                   .Where(p => p.IsActive)
                   .Where(p => p.Phone.Contains(SearchString))
                   .Select(p => new EmployeeSearchVM()
                   {
                       EmployeeId = p.EmployeeId,
                       EmployeeName = p.Name,
                       Phone = p.Phone,
                   }).OrderBy(p => p.EmployeeId).ToPagedList(pageNo, 20);
            }

            if (IsByMrNo)
            {
                Result = cxt.Employees
                   .Where(p => p.IsActive)
                   .Select(p => new EmployeeSearchVM()
                   {
                       EmployeeId = p.EmployeeId,
                       EmployeeName = p.Name,
                       Phone = p.Phone,
                   }).OrderBy(p => p.EmployeeId).ToPagedList(pageNo, 20);
            }
            return Result;
        }
        public List<EmployeeSearchVM> LoadEmployeesCombo()
        {
            return cxt.Employees
                .Where(p => p.IsActive)
                .Select(p => new EmployeeSearchVM()
            {
                EmployeeId = p.EmployeeId,
                EmployeeName = p.Name + "    | " + p.Phone
                //Phone = p.Phone,
                //MrNo = (decimal)p.MRNo
            }).ToList();
        }
        public Employee GetEmployeeById(long EmployeeId)
        {
            return cxt.Employees
                .Where(p => p.EmployeeId == EmployeeId && p.IsActive).FirstOrDefault();
        }
        public void UpdateStatus(long employeeId, int status)
        {
            string sql = "UPDATE Employees SET Status = @Status WHERE EmployeeId = @EmployeeId";

            var p1 = new SqlParameter("@Status", status);
            var p2 = new SqlParameter("@EmployeeId", employeeId);

            cxt.Database.ExecuteSqlRaw(sql, p1, p2);
        }


        public void SetInActive(long employeeId)
        {
            string sql = "UPDATE Employees SET IsActive = 0 WHERE EmployeeId = @EmployeeId";

            var p1 = new SqlParameter("@EmployeeId", employeeId);

            cxt.Database.ExecuteSqlRaw(sql, p1);
        }

        public double getDuesByEmployeeId(long EmployeeId, long invoiceId)
        {
            var res = cxt.Invoices
                .Where(i => i.Employee.EmployeeId == EmployeeId && i.InvoiceId != invoiceId)
                .Include(i => i.InvoicePayments).ToList();
            if (res != null)
            {
                return res.Sum(r => r.GrandTotal) - res.Sum(r => r.InvoicePayments.Where(ip => ip.IsActive).Sum(ip => ip.Payment));
            }
            else
            {
                return 0;
            }
        }
        public List<SelectListVM> GetSelectList()
        {
            return cxt.
                Employees
                .Where(p => p.IsActive)
                .Select(p => new SelectListVM
                {
                    Value = p.EmployeeId,
                    Text = p.Name
                }).ToList();
        }
    }
}
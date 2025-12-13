
namespace Restaurant_MS_Infrastructure.Repository
{
    public class ExpenseRepository : Repository<Expense>
    {
        AppDbContext cxt;
        public ExpenseRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public int GetVoucherNo()
        {
            int VoucherNo = 0;
            if (cxt.Expenses.FirstOrDefault() == null)
            {
                VoucherNo = 1;
            }
            else
            {
                VoucherNo = cxt.Expenses.Max(n => n.VoucherNo) + 1;
            }
            return VoucherNo;
        }


        public object getExpenses(long? CategoryId, DateTime? from, DateTime? to)
        {
            var query = cxt.Expenses.Where(e => e.IsActive && e.Date.Date >= from.Value.Date
                    && e.Date.Date <= to.Value.Date)
                    .Select(e => e);
            if (CategoryId.HasValue && CategoryId.Value > 0)
            {
                query = query.Where(e => e.ExpenseCategoryId == CategoryId.Value);
            }

            return query.Include(e => e.ExpenseCategory).OrderByDescending(e => e.Date).ToList();
        }

        public bool DeleteExpense(long expenseId)
        {
            try
            {
                var exp = cxt.Expenses.Where(e => e.ExpenseId == expenseId).FirstOrDefault();
                if (exp != null)
                {
                    exp.IsActive = false;
                    exp.UpdatedAt = DateTime.Now;
                    exp.UserId = SharedVariables.LoggedInUser.UserId;
                    cxt.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}

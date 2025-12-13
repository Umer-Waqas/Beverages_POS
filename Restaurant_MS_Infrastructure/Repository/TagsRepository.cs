

namespace Restaurant_MS_Infrastructure.Repository
{

    public class TagsRepository : Repository<Tag>, ITag
    {
        AppDbContext cxt = null;
        public TagsRepository(AppDbContext cxt) : base (cxt)
        {
            this.cxt=  cxt;
        }

        public Tag FindTagByName(string TagName)
        {
             return cxt.Tags.Where(t => t.TagName.ToLower() == TagName.ToLower()).FirstOrDefault();
        }
        public void RemoveTag(long TagId, long PatientId)
        {
            string qry = "delete from TagPatients where patient_patientId = @PatientId AND tag_tagId = @TagId";
            int rowsEffected = cxt.Database.ExecuteSqlRaw(qry, new SqlParameter("@PatientId", PatientId), new SqlParameter("@TagId", TagId));
        }
        public void AddTag(long TagId, long PatientId)
        {            
            string qry = "insert into TagPatients (Tag_TagId, Patient_PatientId) values(@TagId,@PatientId)";
            cxt.Database.ExecuteSqlRaw(qry, new SqlParameter("@TagId", TagId), new SqlParameter("@PatientId", PatientId));
        }
    }
}
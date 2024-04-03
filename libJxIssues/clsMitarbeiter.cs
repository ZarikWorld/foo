using libJxIssues.Models;

namespace libJxIssues
{
    public enum enumTeam
    {
        other = 0,
        programmierung = 1,
        technik = 2,
        support = 3,
    }
    
    public enum enumMitarbeiterRole
    {
        user = 0,
        admin = 1,
    }

    public class clsMitarbeiter
    {
        public string? avatar_url { get; set; }
        public int gitlab_id { get; set; }
        public string gitUsername { get; set; }
        public string gitToken { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public enumMitarbeiterRole mitarbeiterRole { get; set; }
        public enumTeam team_id { get; set; }
        public int montag { get; set; }
        public int dienstag { get; set; }
        public int mittwoch { get; set; }
        public int donnerstag { get; set; }
        public int freitag { get; set; }

        public clsMitarbeiter()
        {
        }
        public void insert()
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                Mitarbeiter tmpMA = cls2db(this);
                context.Add(tmpMA);
                context.SaveChanges();
                this.id = tmpMA.id;
            }
        }
        public void update()
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                Mitarbeiter? ob = (from x in context.mitarbeiter where x.id == this.id select x).FirstOrDefault();
                if (ob != null)
                {
                    cls2db(this, ob);
                    context.SaveChanges();
                }
            }
        }
        public void delete()
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                var ob = (from x in context.jxIssue where x.id == this.id select x).FirstOrDefault();

                if (ob != null)
                {
                    context.jxIssue.Remove(ob);
                    context.SaveChanges();
                }
            }
        }
        private static void db2cls(Mitarbeiter xxx, clsMitarbeiter yyy)
        {
            yyy.avatar_url = xxx.avatar_url;
            yyy.gitlab_id = xxx.gitlab_id;
            yyy.gitUsername = xxx.gitUsername;
            yyy.gitToken = xxx.gitToken;
            yyy.id = xxx.id;
            yyy.name= xxx.name;
            
            if (xxx.mitarbeiterRole.HasValue)
            {
                yyy.mitarbeiterRole= (enumMitarbeiterRole)xxx.mitarbeiterRole;
            }
            else
            {
                yyy.mitarbeiterRole = enumMitarbeiterRole.user;
            }
            
            yyy.team_id = (enumTeam)xxx.team_id;
            yyy.montag = xxx.montag;
            yyy.dienstag = xxx.dienstag;
            yyy.mittwoch = xxx.mittwoch;
            yyy.donnerstag = xxx.donnerstag;
            yyy.freitag = xxx.freitag;
        }
        private static void cls2db(clsMitarbeiter xxx, Mitarbeiter yyy)
        {
            yyy.avatar_url= xxx.avatar_url;
            yyy.gitlab_id= xxx.gitlab_id;
            yyy.gitUsername= xxx.gitUsername;
            yyy.gitToken= xxx.gitToken;
            yyy.name = xxx.name;
            yyy.mitarbeiterRole = (int)xxx.mitarbeiterRole;
            yyy.team_id = (int)xxx.team_id;
            yyy.montag = xxx.montag;
            yyy.dienstag = xxx.dienstag;
            yyy.mittwoch = xxx.mittwoch;
            yyy.donnerstag = xxx.donnerstag;
            yyy.freitag = xxx.freitag;
        }
        private static clsMitarbeiter db2cls(Mitarbeiter xxx)
        {
            clsMitarbeiter yyy = new clsMitarbeiter();
            db2cls(xxx, yyy);
            return yyy;
        }
        private static Mitarbeiter cls2db( clsMitarbeiter xxx)
        {
            Mitarbeiter yyy = new Mitarbeiter();
            cls2db(xxx, yyy);
            return yyy;
        }
        public static clsMitarbeiter? getMitarbeiter(int id_)
        {
            try
            {
                using (jxIssuesContext context = efHelper.getConnection())
                {
                    Mitarbeiter? mitarbeiterX = (from x in context.mitarbeiter where x.id == id_ select x).FirstOrDefault();

                    if (mitarbeiterX != null)
                    {
                        clsMitarbeiter result;
                        result = new clsMitarbeiter();
                        return db2cls(mitarbeiterX);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(libCodLibCS.nsFileSystem.clsFileSystem.getLogFilePath(), innerException, $"<clsMitarbeiter.getMitarbeiter>: {id_}");
                throw new Exception(ex.Message, ex);
            }
        }
        public static clsMitarbeiter? getMitarbeiter(string gitUserName)
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                Mitarbeiter? mitarbeiterX = (from x in context.mitarbeiter where x.gitUsername == gitUserName select x).FirstOrDefault();
                
                if (mitarbeiterX != null)
                {
                    clsMitarbeiter result;
                    result = new clsMitarbeiter();
                    return db2cls(mitarbeiterX);
                }
            }
        
            return null;
        }
        public static  List<clsMitarbeiter> getMitarbeiters()
        {
            List < clsMitarbeiter> res = new List<clsMitarbeiter>();

            using (jxIssuesContext context = efHelper.getConnection())
            {
                var ob = from x in context.mitarbeiter 
                         orderby x.name
                         select x ;

                foreach (var item in ob)
                {
                    res.Add(db2cls(item));
                }
            }

            return res;
        }
    }
}
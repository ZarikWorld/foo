using libJxIssues.Models;

namespace libJxIssues
{
    public class clsProgramInformation
    {
        #region Properties
        public int? id { get; set; }
        public DateTime? lastSyncGIT { get; set; } = new DateTime(2000, 1, 1);
        public int project_id { get; set; }
        #endregion

        #region Methods
        public void insert()
        {
            try
            {
                using (jxIssuesContext context = efHelper.getConnection())
                {
                    ProgramInformation programInformation = new ProgramInformation();
                    programInformation.lastSyncGIT = this.lastSyncGIT;
                    programInformation.project_id = this.project_id;

                    context.programInformation.Add(programInformation);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(libCodLibCS.nsFileSystem.clsFileSystem.getLogFilePath(), innerException, $"<clsLastSyncInfo.insert>: id: {this.id} lastSyncGIT: {this.lastSyncGIT} project_id: {project_id}");
                throw new Exception(ex.Message, ex);
            }
        }
        public void update()
        {
            try
            {
                using (jxIssuesContext context = efHelper.getConnection())
                {
                    var ob = (from row in context.programInformation where row.id == this.id select row).FirstOrDefault();
                    if (ob != null)
                    {
                        ob.lastSyncGIT = this.lastSyncGIT;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(libCodLibCS.nsFileSystem.clsFileSystem.getLogFilePath(), innerException, $"<clsLastSyncInfo.update>: id: {this.id} lastSyncGIT: {this.lastSyncGIT} project_id: {project_id}");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Static
        public static bool projectFirstTimeDbSync(int project_id)
        {
            try
            {
                using (jxIssuesContext context = efHelper.getConnection())
                {
#pragma warning disable CS8629
                    ProgramInformation? lastSyncInfo = (from row in context.programInformation
                                                        where row.project_id == project_id &&
                                                        row.lastSyncGIT != null &&
                                                        !row.lastSyncGIT.Equals(clsProgramInformation.historyStartDate)
                                                        select row).FirstOrDefault();

                    return lastSyncInfo == null;
#pragma warning restore CS8629
                }
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(libCodLibCS.nsFileSystem.clsFileSystem.getLogFilePath(), innerException, $"<clsLastSyncInfo.firstTimeDbSync>");
                throw new Exception(ex.Message, ex);
            }
        }
        public static readonly DateTime historyStartDate = new DateTime(2000, 1, 1);
        public static DateTime getProjectLastSyncDateTime(int project_id)
        {
            DateTime result;

            try
            {
#pragma warning disable CS8629
                using (jxIssuesContext context = efHelper.getConnection())
                {
                    ProgramInformation? lastSyncInfo = (from row in context.programInformation
                                                        where row.project_id == project_id
                                                        select row).FirstOrDefault();

                    if (lastSyncInfo != null)
                    {
                        //there's record => get the last sync time
                        result = lastSyncInfo.lastSyncGIT.Value;
                    }
                    else
                    {
                        //no record => create the record with the minimum time
                        clsProgramInformation tmpSyncInfo = new clsProgramInformation();
                        tmpSyncInfo.project_id = project_id;
                        tmpSyncInfo.insert();
                        result = tmpSyncInfo.lastSyncGIT.Value;
                    }
                }
#pragma warning restore CS8629

                return result;
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(libCodLibCS.nsFileSystem.clsFileSystem.getLogFilePath(), innerException, $"<clsLastSyncInfo.getProjectLastSyncDateTime>: {project_id}");
                throw new Exception(ex.Message, ex);
            }

        }
        public static void updateLastSync(int project_id, DateTime syncTime)
        {
            try
            {
                using (jxIssuesContext context = efHelper.getConnection())
                {
                    ProgramInformation? lastSyncInfo = (from row in context.programInformation
                                                        where row.project_id == project_id
                                                        select row).FirstOrDefault();

                    if (lastSyncInfo is not null)
                    {
                        //there's record => get the last sync time
                        lastSyncInfo.lastSyncGIT = syncTime;
                        context.SaveChanges();
                    }
                    else
                    {
                        //no record => create the record with the minimum time
                        clsProgramInformation tmpSyncInfo = new clsProgramInformation();
                        tmpSyncInfo.project_id = project_id;
                        tmpSyncInfo.insert();
                    }
                }
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(libCodLibCS.nsFileSystem.clsFileSystem.getLogFilePath(), innerException, $"<clsLastSyncInfo.updateLastSync>: project_id: {project_id} syncTime: {syncTime}");
                throw new Exception(ex.Message, ex);
            }
        }
        public static List<ProgramInformation> getProjects()
        {
            try
            {
                using (jxIssuesContext context = efHelper.getConnection())
                {
                    List<ProgramInformation>? syncedProjects = (from row in context.programInformation
                                                                select row).ToList();

                    if (syncedProjects is not null)
                    {
                        return syncedProjects;
                    }
                    else
                    {
                        return new List<ProgramInformation>();
                    }
                }
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(libCodLibCS.nsFileSystem.clsFileSystem.getLogFilePath(), innerException, $"<clsLastSyncInfo.getProjects>: can not retrive projects from db");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion
    }
}

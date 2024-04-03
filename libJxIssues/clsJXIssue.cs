using libJxIssues.Models;

namespace libJxIssues
{
    public class clsJxIssue
    {
        #region Enums
        public enum enumTyp
        {
            story = 1,
            bug = 2,
            performance = 3,
            luxus = 4,
            refactor = 5,
        }
        public enum enumStatus
        {
            opened = 1,
            closed = 2,
            imTest = 3,
            inBearbeitung = 4,
            klaerungsbedarf = 5
        }
        #endregion
        #region Properties
        public int id { set; get; }
        public int iid { set; get; }
        public int project_id { get; set; }
        public bool erledigt { get; set; }
        public int? prioPunkte { get; set; }
        public string titel { get; set; }
        public bool git { get; set; }
        public string web_url { get; set; }
        public int? sortOrder { get; set; }
        public DateTime? start { get; set; }
        public DateTime? ende { get; set; }
        public DateTime? deadline { get; set; }
        public int creator_id { get; set; }
        private int? mitarbeiter_id_;
        public int? mitarbeiter_id
        {
            get
            {
                return this.mitarbeiter_id_;
            }
            set
            {
                this.mitarbeiter_id_ = value;
                if (this.mitarbeiter_id_.HasValue)
                {
                    var mitarbeiter = clsMitarbeiter.getMitarbeiter(this.mitarbeiter_id_.Value);
                    if (mitarbeiter != null)
                    {
                        this.xxxMitarbeiterName = mitarbeiter.name;
                    }
                }
            }
        }
        public int? schaetzung { get; set; }        
        public int? schaetzungOffiziell { get; set; }
        public DateTime created_at { get; set; }
        public enumTyp typ { get; set; }
        public enumStatus status { get; set; }
        public string xxxMitarbeiterName { get; set; }
        public string? aktenzahl { get; set; }
        public string kunde { get; set; }
        public string anmerkung { get; set; }
        #endregion

        public clsJxIssue()
        {
        }
        public clsJxIssue(jxIssue x)
        {
            db2cls(x, this);
        }
        [Obsolete("The assignee property is deprecated. Use getIssueByGitIid instead.")]
        public void insert()
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                jxIssue tmpIssue = cls2db(this);
                context.Add(tmpIssue);
                context.SaveChanges();
                this.id = tmpIssue.id;
            }

            clsJxIssue.repairSortOrder();
        }
        public void update()
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                bool sortOrderChanged = false;

                var ob = (from x in context.jxIssue where x.id == this.id select x).FirstOrDefault();
                if (ob != null)
                {
                    if (this.sortOrder != ob.sortOrder)
                    {
                        sortOrderChanged = true;
                    }

                    cls2db(this, ob);
                    context.SaveChanges();
                }

                if (sortOrderChanged)
                {
                    clsJxIssue.repairSortOrder();
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
        private static void db2cls(jxIssue xxx, clsJxIssue yyy)
        {
            yyy.id = xxx.id;
            yyy.iid = xxx.iid;
            yyy.project_id = xxx.project_id;
            yyy.erledigt = xxx.erledigt;
            yyy.prioPunkte = xxx.prioPunkte;
            yyy.titel = xxx.titel;
            yyy.git = xxx.git;
            yyy.web_url = xxx.web_url;
            yyy.sortOrder = xxx.sortOrder;
            yyy.start = xxx.start;
            yyy.ende = xxx.ende;
            yyy.creator_id = xxx.creator_id;
            yyy.mitarbeiter_id = xxx.mitarbeiter_id;
            yyy.schaetzung = xxx.schaetzung;
            yyy.schaetzungOffiziell = xxx.schaetzungOffiziell;
            yyy.created_at = xxx.created_at;
            yyy.deadline = xxx.deadline;
            yyy.typ = (enumTyp)xxx.typ;
            yyy.status = (enumStatus)xxx.status;
            yyy.aktenzahl = xxx.aktenzahl;
            yyy.kunde = xxx.kunde;
            yyy.anmerkung = xxx.anmerkung;

        }
        private static void cls2db(clsJxIssue xxx, jxIssue yyy)
        {
            yyy.id = xxx.id;
            yyy.iid = xxx.iid;
            yyy.project_id = xxx.project_id;
            yyy.erledigt = xxx.erledigt;
            yyy.prioPunkte = xxx.prioPunkte;
            yyy.titel = xxx.titel;
            yyy.git = xxx.git;
            yyy.web_url = xxx.web_url;
            yyy.sortOrder = xxx.sortOrder;
            yyy.start = xxx.start;
            yyy.ende = xxx.ende;
            yyy.creator_id = xxx.creator_id;
            yyy.mitarbeiter_id = xxx.mitarbeiter_id;
            yyy.schaetzung = xxx.schaetzung;
            yyy.schaetzungOffiziell = xxx.schaetzungOffiziell;            
            yyy.created_at = xxx.created_at;
            yyy.deadline = xxx.deadline;
            yyy.typ = (int)xxx.typ;
            yyy.status = (int)xxx.status;
            yyy.aktenzahl = xxx.aktenzahl;
            yyy.kunde = xxx.kunde;
            yyy.anmerkung = xxx.anmerkung;
        }
        private static clsJxIssue db2cls(jxIssue xxx)
        {
            clsJxIssue yyy = new clsJxIssue();
            db2cls(xxx, yyy);
            return yyy;
        }
        private static jxIssue cls2db(clsJxIssue xxx)
        {
            jxIssue yyy = new jxIssue();
            cls2db(xxx, yyy);
            return yyy;
        }
        public static clsJxIssue? getIssueByURL(string webUrl)
        {
            clsJxIssue? result = null;
            using (jxIssuesContext context = efHelper.getConnection())
            {
                jxIssue? ob = (from x in context.jxIssue where x.web_url == webUrl select x).FirstOrDefault();
                if (ob != null)
                {
                    result = new clsJxIssue();
                    result = db2cls(ob);
                }
            }
            return result;
        }
        public static clsJxIssue? getIssueByGitIID(int git_iid, int project_id)
        {
            clsJxIssue? result = null;
            using (jxIssuesContext context = efHelper.getConnection())
            {
                jxIssue? ob = (from x in context.jxIssue
                               where x.project_id == project_id &&
                                     x.iid == git_iid
                               select x).FirstOrDefault();
                if (ob != null)
                {
                    result = new clsJxIssue();
                    result = db2cls(ob);
                }
            }
            return result;
        }
        public static List<clsJxIssue> getIssues()
        {
            List<clsJxIssue> res = new List<clsJxIssue>();

            using (jxIssuesContext context = efHelper.getConnection())
            {
                var ob = context.jxIssue;

                foreach (var item in ob)
                {
                    res.Add(db2cls(item));
                }
            }

            return res;
        }
        public static clsJxIssue? getIssueById(int issue_id)
        {
            jxIssue? jxIssue = null;
            using (jxIssuesContext context = efHelper.getConnection())
            {
                jxIssue = (from x in context.jxIssue
                           where x.iid == issue_id
                           select x).FirstOrDefault();
            }

            if (jxIssue is null)
            {
                return null;
            }

            return db2cls(jxIssue);
        }
        public static List<clsJxIssue>? getIssuesByProjectId(int project_id)
        {
            List<jxIssue>? jxIssues = null;
            using (jxIssuesContext context = efHelper.getConnection())
            {
                jxIssues = (from x in context.jxIssue where x.project_id == project_id select x).ToList();
            }

            if (jxIssues is null)
            {
                return null;
            }

            List<clsJxIssue> res = new List<clsJxIssue>();

            foreach (var jxIssue_ in jxIssues)
            {
                res.Add(db2cls(jxIssue_));
            }

            return res;
        }
        private static void repairNegativeSortOrders()
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                var alleNegative = (from x in context.jxIssue
                                    where !x.erledigt &&
                                            x.sortOrder < 0
                                    orderby x.sortOrder descending
                                    select x).ToList();

                for (int i = 0; i < alleNegative.Count()/* - 1*/; i++)
                {
                    alleNegative[i].sortOrder = -(i + 1);
                }
                context.SaveChanges();
            }
        }
        private static void repairPositiveSortOrder()
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                var alle = (from x in context.jxIssue
                            where !x.erledigt &&
                                    x.sortOrder > 0
                            orderby x.sortOrder
                            select x).ToList();

                for (int i = 0; i < alle.Count()/* - 1*/; i++)
                {
                    alle[i].sortOrder = i + 1;
                }
                context.SaveChanges();
            }
        }
        public static void repairSortOrder()
        {
            repairNegativeSortOrders();
            repairPositiveSortOrder();
        }
        public static int getNextAvailableNegativeSortOrder()
        {
            repairSortOrder();

            int result = -1;
            using (jxIssuesContext context = efHelper.getConnection())
            {
                jxIssue? issue = (from x in context.jxIssue
                                  where x.sortOrder != null
                                  orderby x.sortOrder
                                  select x).FirstOrDefault();

                int? newSortOrder = issue?.sortOrder - 1;
                if (newSortOrder != null && newSortOrder.HasValue)
                {
                    result = newSortOrder.Value != 0 ? newSortOrder.Value : -1;
                }
            }

            return result;
        }
        public static void changeSortOrder(int sortOrderFrom, int sortOrderTo, int project_id)
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                if (sortOrderFrom > 0)
                {
                    //Dragging UP in List
                    if (sortOrderTo < sortOrderFrom)
                    {
                        var alleAbZiel = from x in context.jxIssue
                                         where x.sortOrder >= sortOrderTo &&
                                               x.sortOrder != sortOrderFrom
                                         select x;

                        var gedroppt = (from x in context.jxIssue
                                        where x.sortOrder == sortOrderFrom
                                        select x).FirstOrDefault();

                        if (gedroppt is null)
                        {
                            return;
                        }

                        gedroppt.sortOrder = sortOrderTo;

                        foreach (var item in alleAbZiel)
                        {
                            item.sortOrder++;
                        }
                    }
                    //Dragging DOWN in list
                    else
                    {
                        var betweenSortOrderFromAndSortOrderTo = from x in context.jxIssue
                                                                 where x.sortOrder <= sortOrderTo &&
                                                                       x.sortOrder > sortOrderFrom
                                                                 select x;

                        var gedroppt = (from x in context.jxIssue
                                        where x.sortOrder == sortOrderFrom
                                        select x).FirstOrDefault();

                        if (gedroppt is null)
                        {
                            return;
                        }

                        gedroppt.sortOrder = sortOrderTo;

                        foreach (var item in betweenSortOrderFromAndSortOrderTo)
                        {
                            item.sortOrder--;
                        }
                    }
                }
                else
                {
                    //When the sortOrder is negative it will only change the positive sort orders and other negatives will stay intact

                    var allUntilSortOrderTo = from x in context.jxIssue
                                              where x.sortOrder <= sortOrderTo
                                              orderby x.sortOrder ascending
                                              select x;

                    var gedroppt = (from x in context.jxIssue
                                    where x.sortOrder == sortOrderFrom
                                    select x).FirstOrDefault();


                    if (gedroppt is null)
                    {
                        return;
                    }

                    gedroppt.sortOrder = sortOrderTo;

                    foreach (var item in allUntilSortOrderTo)
                    {
                        item.sortOrder++;
                    }
                }

                context.SaveChanges();
                repairSortOrder();

                //TODO: return the updated version.
            }
        }
        public static void changeSortOrder(List<int> projects_id, string gitlabPrivateToken, int sortOrderFrom, int sortOrderTo)
        {
            //1- Process the request
            using (jxIssuesContext context = efHelper.getConnection())
            {
                if (sortOrderFrom > 0)
                {
                    //Dragging UP in List
                    if (sortOrderTo < sortOrderFrom)
                    {
                        var alleAbZiel = from x in context.jxIssue
                                         where x.sortOrder >= sortOrderTo &&
                                               x.sortOrder != sortOrderFrom
                                         select x;

                        var gedroppt = (from x in context.jxIssue
                                        where x.sortOrder == sortOrderFrom
                                        select x).FirstOrDefault();

                        if (gedroppt is null)
                        {
                            return;
                        }

                        gedroppt.sortOrder = sortOrderTo;

                        foreach (var item in alleAbZiel)
                        {
                            item.sortOrder++;
                        }
                    }
                    //Dragging DOWN in list
                    else
                    {
                        var betweenSortOrderFromAndSortOrderTo = from x in context.jxIssue
                                                                 where x.sortOrder <= sortOrderTo &&
                                                                       x.sortOrder > sortOrderFrom
                                                                 select x;

                        var gedroppt = (from x in context.jxIssue
                                        where x.sortOrder == sortOrderFrom
                                        select x).FirstOrDefault();

                        if (gedroppt is null)
                        {
                            return;
                        }

                        gedroppt.sortOrder = sortOrderTo;

                        foreach (var item in betweenSortOrderFromAndSortOrderTo)
                        {
                            item.sortOrder--;
                        }
                    }
                }
                else
                {
                    var gedroppt = (from x in context.jxIssue
                                    where x.sortOrder == sortOrderFrom
                                    select x).FirstOrDefault();

                    if (gedroppt is null)
                    {
                        return;
                    }

                    gedroppt.sortOrder = sortOrderTo - 1;
                }

                context.SaveChanges();
                repairSortOrder();
            }

            //TODO: 2- Implement so the functions automatically returns the updated collection back to the client
        }
        public static List<jxIssue> ChangeSortOrder(int sortOrderFrom, int sortOrderTo)
        {
            if (sortOrderFrom < sortOrderTo && sortOrderFrom > 0)
            {
                sortOrderTo = sortOrderTo - 1;
            }

            List<jxIssue> updatedRecords = new List<jxIssue>();

            if (sortOrderFrom == sortOrderTo || sortOrderTo == 0) return updatedRecords;

            using (jxIssuesContext context = efHelper.getConnection())
            {
                var maxSortOrder = (from x in context.jxIssue where x.sortOrder != null select x.sortOrder).Max();
                int sortOrder = sortOrderTo;
                if (maxSortOrder.HasValue && sortOrder > maxSortOrder && sortOrderFrom < 0)
                {
                    sortOrder = maxSortOrder.Value + 1;
                }   
                else if (maxSortOrder.HasValue && sortOrder > maxSortOrder)
                {
                    sortOrder = maxSortOrder.Value;
                }
                
                if (sortOrderFrom < 0 && sortOrder > 0)
                {
                    //move all negative sortOrder values one step closer to zero.
                    var recordsLessThanSortOrderFrom = context.jxIssue.Where(x => x.sortOrder < sortOrderFrom).ToList();
                    foreach (var record in recordsLessThanSortOrderFrom)
                    {
                        record.sortOrder += 1;
                        updatedRecords.Add(record);
                    }

                    //increase all sortOrder values that are >= sortOrderTo
                    var recordsGreaterThanOrEqualToSortOrderTo = context.jxIssue.Where(x => x.sortOrder >= sortOrder).ToList();
                    foreach (var record in recordsGreaterThanOrEqualToSortOrderTo)
                    {
                        record.sortOrder += 1;
                        updatedRecords.Add(record);
                    }
                }
                else
                {
                    bool moveUpward = sortOrder < sortOrderFrom;
                    //get affected records between sortOrderFrom and sortOrderTo (on the positive side).
                    var affectedRecords = context.jxIssue
                        .Where(x => ((moveUpward && x.sortOrder >= sortOrder && x.sortOrder < sortOrderFrom) ||
                                     (!moveUpward && x.sortOrder > sortOrderFrom && x.sortOrder <= sortOrder)))
                        .ToList();

                    //shift the sortOrder of affected records on the positive side.
                    foreach (var record in affectedRecords)
                    {
                        if (moveUpward)
                        {
                            record.sortOrder += 1;
                        }
                        else
                        {
                            record.sortOrder -= 1;
                        }
                        updatedRecords.Add(record);
                    }
                }

                //update sortOrder of dragged item.
                var movedRecord = context.jxIssue.FirstOrDefault(x => x.sortOrder == sortOrderFrom);
                if (movedRecord != null)
                {
                    movedRecord.sortOrder = sortOrder;
                    updatedRecords.Add(movedRecord);
                }

                context.SaveChanges();

                return updatedRecords;
            }
        }
        public static List<clsJxIssue> movePrioPunkte(int id, bool isUpward)
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                var movedIssue = context.jxIssue.Where(issue => issue.id == id).FirstOrDefault();

                ArgumentNullException.ThrowIfNull(movedIssue);

                List<clsJxIssue> result = new List<clsJxIssue>();

                if (isUpward)
                {
                    var lowestPrioPunkte = context.jxIssue.Where(issue => issue.prioPunkte == movedIssue.prioPunkte && issue.sortOrder > 0)
                                                  .OrderBy(issue => issue.sortOrder).FirstOrDefault();

                    if (lowestPrioPunkte != null && movedIssue.sortOrder.HasValue && lowestPrioPunkte.sortOrder.HasValue)
                    {
                        var modifiedIssues = ChangeSortOrder(movedIssue.sortOrder.Value, lowestPrioPunkte.sortOrder.Value);

                        foreach (var issue in modifiedIssues)
                        {
                            var tmpIssue = new clsJxIssue();
                            db2cls(issue, tmpIssue);
                            result.Add(tmpIssue);
                        }
                    }
                }
                else
                {
                    var highestPrioPunkte = context.jxIssue.Where(issue => issue.prioPunkte == movedIssue.prioPunkte && issue.sortOrder > 0)
                                                           .OrderByDescending(issue => issue.sortOrder)
                                                           .FirstOrDefault();

                    if (highestPrioPunkte != null && movedIssue.sortOrder.HasValue && highestPrioPunkte.sortOrder.HasValue)
                    {
                        var modifiedIssues = ChangeSortOrder(movedIssue.sortOrder.Value, highestPrioPunkte.sortOrder.Value + 1);

                        foreach (var issue in modifiedIssues)
                        {
                            var tmpIssue = new clsJxIssue();
                            db2cls(issue, tmpIssue);
                            result.Add(tmpIssue);
                        }
                    }
                }

                return result;
            }
        }
        public static void movePrioPunkteList(IEnumerable<int> ids, bool isUpward)
        {
            using (jxIssuesContext context = efHelper.getConnection())
            {
                Dictionary<int, clsJxIssue> resultDict = new Dictionary<int, clsJxIssue>();

                foreach (int id in ids)
                {
                    if (id <= 0) continue;  // Ignore negative or zero IDs

                    var movedIssue = context.jxIssue.Where(issue => issue.id == id).FirstOrDefault();

                    ArgumentNullException.ThrowIfNull(movedIssue);

                    if (isUpward)
                    {
                        var topmostPrioPunkt = context.jxIssue.Where(issue => issue.prioPunkte == movedIssue.prioPunkte && issue.sortOrder > 0)
                                                              .OrderBy(issue => issue.sortOrder).FirstOrDefault();

                        if (topmostPrioPunkt != null && movedIssue.sortOrder.HasValue && topmostPrioPunkt.sortOrder.HasValue)
                        {
                            var modifiedIssues = ChangeSortOrder(movedIssue.sortOrder.Value, topmostPrioPunkt.sortOrder.Value);

                            foreach (var issue in modifiedIssues)
                            {
                                var tmpIssue = new clsJxIssue();
                                db2cls(issue, tmpIssue);
                                resultDict[tmpIssue.id] = tmpIssue;
                            }
                        }
                    }
                    else
                    {
                        var bottommostPrioPunkt = context.jxIssue.Where(issue => issue.prioPunkte == movedIssue.prioPunkte && issue.sortOrder > 0)
                                                               .OrderByDescending(issue => issue.sortOrder)
                                                               .FirstOrDefault();

                        if (bottommostPrioPunkt!.id == movedIssue.id)
                        {
                            bottommostPrioPunkt = context.jxIssue
                                 .Where(issue => issue.prioPunkte == movedIssue.prioPunkte && issue.sortOrder > 0)
                                 .OrderByDescending(issue => issue.sortOrder)
                                 .Skip(1)
                                 .FirstOrDefault();
                        }

                        if (bottommostPrioPunkt != null && movedIssue.sortOrder.HasValue && bottommostPrioPunkt.sortOrder.HasValue)
                        {
                            var modifiedIssues = ChangeSortOrder(movedIssue.sortOrder.Value, bottommostPrioPunkt.sortOrder.Value + 1);

                            foreach (var issue in modifiedIssues)
                            {
                                var tmpIssue = new clsJxIssue();
                                db2cls(issue, tmpIssue);
                                resultDict[tmpIssue.id] = tmpIssue;
                            }
                        }
                    }
                }
                //return resultDict.Values.ToList(); 
            }
        }
    }
}
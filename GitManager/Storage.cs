using System.Collections.Generic;
using System.Linq;
using GitManager.DAO;
using GitManager.Entity;

namespace GitManager
{
    public class Storage : IStorage
    {
        public bool StoreMergeRequests(List<MergeRequest> requests)
        {
            using (var ctx = new DataContext())
            {
                foreach (var mergeRequest in requests)
                    if (!ctx.MergeRequests.Any(item => item.RequestId == mergeRequest.Id))
                        ctx.MergeRequests.Add(new MergeRequestEntity
                        {
                            RequestId = mergeRequest.Id,
                            User = mergeRequest.Author?.UserName
                        });

                ctx.SaveChanges();
            }

            return true;
        }

        public List<MergeRequest> FilterNewRequests(List<MergeRequest> requests)
        {
            var filteredList = new List<MergeRequest>();

            using (var ctx = new DataContext())
            {
                foreach (var mergeRequest in requests)
                    if (!ctx.MergeRequests.Any(item => item.RequestId == mergeRequest.Id))
                        filteredList.Add(mergeRequest);
            }

            return filteredList;
        }
    }
}
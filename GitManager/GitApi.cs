using System.Collections.Generic;
using System.Threading.Tasks;
using GitManager.Connection;
using GitManager.DAO;

namespace GitManager
{
    public class GitApi : IGitApi
    {
        public List<MergeRequest> GetMergeRequests()
        {
            var task = Task.Run(() => WebApi.Instance.GetAsync<List<MergeRequest>>("merge_requests?scope=all", new Config()));
            task.Wait();
            return task.Result;
        }
    }
}
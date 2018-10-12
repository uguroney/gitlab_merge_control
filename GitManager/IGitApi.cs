using System.Collections.Generic;
using GitManager.DAO;

namespace GitManager
{
    public interface IGitApi
    {
        List<MergeRequest> GetMergeRequests();
    }
}
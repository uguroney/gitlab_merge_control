using System.Collections.Generic;
using GitManager.DAO;

namespace GitManager
{
    public interface IStorage
    {
        bool StoreMergeRequests(List<MergeRequest> requests);
        List<MergeRequest> FilterNewRequests(List<MergeRequest> requests);
    }
}
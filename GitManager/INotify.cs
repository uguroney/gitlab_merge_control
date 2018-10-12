using System.Collections.Generic;
using GitManager.DAO;

namespace GitManager
{
    public interface INotify
    {
        bool Notify(List<MergeRequest> info);
    }
}
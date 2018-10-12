using System.Collections.Generic;
using GitManager.DAO;

namespace GitManager
{
    public interface IReporter
    {
        bool WriteToCsv(IEnumerable<MergeRequest> line);
    }
}
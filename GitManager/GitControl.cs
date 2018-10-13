using System;
using System.Linq;
using System.Timers;

namespace GitManager
{
    public class GitControl
    {
        private readonly IGitApi _gitApi;
        private readonly INotify _notify;
        private readonly IReporter _reporter;
        private readonly IStorage _storage;

        private readonly Timer _timer;

        public GitControl(IGitApi gitApi, IReporter reporter, INotify notify, IStorage storage)
        {
            _gitApi = gitApi;
            _reporter = reporter;
            _notify = notify;
            _storage = storage;

            _timer = new Timer(60000 * 30) {AutoReset = true};
            _timer.Elapsed += (sender, eventArgs) => CheckSelfMergedRequests();
        }

        public bool CheckSelfMergedRequests()
        {
            var mergeRequests = _gitApi.GetMergeRequests();

            var filteredMergeRequests = mergeRequests.Where(request =>
                request.Assignee?.Id == request.Author?.Id && request.CreateAt >= DateTime.Now.AddDays(-30));

            var list = _storage.FilterNewRequests(mergeRequests);

            _storage.StoreMergeRequests(list);

            if (list.Count != 0) _notify.Notify(list);

            return _reporter.WriteToCsv(list);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
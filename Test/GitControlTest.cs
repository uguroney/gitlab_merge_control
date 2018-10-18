using System;
using System.Collections.Generic;
using GitManager.DAO;
using Moq;
using NUnit.Framework;

namespace GitManager.Test
{
    [TestFixture]
    public class GitControlTest
    {
        [SetUp]
        public void SetUp()
        {
            _reporter.Setup(obj => obj.WriteToCsv(It.IsAny<IEnumerable<MergeRequest>>())).Returns(true);
            _notify.Setup(obj => obj.Notify(It.IsAny<List<MergeRequest>>())).Returns(true);
            _storage.Setup(obj => obj.FilterNewRequests(It.IsAny<List<MergeRequest>>()))
                .Returns(new List<MergeRequest>());
            _storage.Setup(obj => obj.StoreMergeRequests(It.IsAny<List<MergeRequest>>())).Returns(true);
        }

        private readonly Mock<IGitApi> _gitApi;
        private readonly Mock<IReporter> _reporter;
        private readonly Mock<INotify> _notify;
        private readonly Mock<IStorage> _storage;
        
        
        private User _janeUser;
        private User _johnUser;
        private readonly DateTime _date;

        public GitControlTest()
        {
            _date = DateTime.Now;
            
            _gitApi = new Mock<IGitApi>();
            _reporter = new Mock<IReporter>();
            _notify = new Mock<INotify>();
            _storage = new Mock<IStorage>();


            _janeUser = new User
            {
                Id = 1,
                Name = "Jane Doe",
                UserName = "Jane.Doe"
            };

            _johnUser = new User
            {
                Id = 2,
                Name = "John Doe",
                UserName = "john.doe"
            };
     
        }
        
        private MergeRequest CreateDummyMergeRequest(User author, User assignee)
        {
            return new MergeRequest
            {
                Author = author, 
                Description = "Lorem ipsum",
                Assignee = assignee, 
                CreateAt = _date.AddDays(-2),
                Id = 500,
                SourceBranch = "develop",
                State = "Merged",
                Title = "Dummy Merge Req",
                TargetBranch = "master",
                UpdatedAt = _date
            };
        }

        [Test(Description = "If API return null should return false")]
        public void CheckSelfMergedRequestsIfGetMergeReturnNullTest()
        {
            _gitApi.Setup(obj => obj.GetMergeRequests()).Returns(() => null);
            var gitControl = new GitControl(_gitApi.Object, _reporter.Object, _notify.Object, _storage.Object);
    
            var actualResult = gitControl.CheckSelfMergedRequests();
            
            Assert.False(actualResult);
        }
        
        [Test(Description = "If API return empty should return false")]
        public void CheckSelfMergedRequestsIfGetMergeReturnEmptyTest()
        {
            _gitApi.Setup(obj => obj.GetMergeRequests()).Returns(() => new List<MergeRequest>());
            var gitControl = new GitControl(_gitApi.Object, _reporter.Object, _notify.Object, _storage.Object);
    
            var actualResult = gitControl.CheckSelfMergedRequests();
            
            Assert.False(actualResult);
        }

        [Test(Description = "If Merge Request assignee is null")]
        public void AssigneeIsNull()
        {
            
            _gitApi.Setup(obj => obj.GetMergeRequests()).Returns(() => new List<MergeRequest>{ CreateDummyMergeRequest(_janeUser, null) });
            var gitControl = new GitControl(_gitApi.Object, _reporter.Object, _notify.Object, _storage.Object);
    
            var actualResult = gitControl.CheckSelfMergedRequests();
            
            Assert.False(actualResult);
        }
        
        [Test(Description = "If Merge Request author is null")]
        public void AuthorIsNull()
        {
            
            _gitApi.Setup(obj => obj.GetMergeRequests()).Returns(() => new List<MergeRequest>{ CreateDummyMergeRequest(null, _janeUser) });
            var gitControl = new GitControl(_gitApi.Object, _reporter.Object, _notify.Object, _storage.Object);
    
            var actualResult = gitControl.CheckSelfMergedRequests();
            
            Assert.False(actualResult);
        }

        [Test]
        public void AuthorIsNotAssignee()
        {
            _gitApi.Setup(obj => obj.GetMergeRequests()).Returns(() => new List<MergeRequest>{ CreateDummyMergeRequest(_johnUser, _janeUser) });
            var gitControl = new GitControl(_gitApi.Object, _reporter.Object, _notify.Object, _storage.Object);
    
            var actualResult = gitControl.CheckSelfMergedRequests();
            
            Assert.False(actualResult);
        }

        [Test]
        public void RequestOlderThen30()
        {
            var mergeRequest = CreateDummyMergeRequest(_janeUser, _janeUser);
            mergeRequest.CreateAt = _date.AddDays(-45);
            
            _gitApi.Setup(obj => obj.GetMergeRequests()).Returns(() => new List<MergeRequest>{  });
            var gitControl = new GitControl(_gitApi.Object, _reporter.Object, _notify.Object, _storage.Object);
    
            var actualResult = gitControl.CheckSelfMergedRequests();
            
            Assert.False(actualResult);
        }
    }
}
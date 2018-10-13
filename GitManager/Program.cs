using System;
using GitManager.Connection;
using Topshelf;

namespace GitManager
{
    internal class Program
    {
        private static void Main()
        {
            var rc = HostFactory.Run(x =>
            {
                x.Service<GitControl>(s =>
                {
                    s.ConstructUsing(name => new GitControl(new GitApi(), new Reporter(),
                        new MailNotification(new Config()), new Storage()));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Git Self Approvement Management Service");
                x.SetDisplayName("Git Self Approvement");
                x.SetServiceName("GitSelfApprovement");
            });

            var exitCode = (int) Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
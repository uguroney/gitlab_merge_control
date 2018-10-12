using GitManager.DAO;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GitManager
{
    public class Reporter : IReporter
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public bool WriteToCsv(IEnumerable<MergeRequest> mergeRequests)
        {
            FileStream file = null;
            try
            {
                using (file = new FileStream("report.csv", FileMode.Append, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true))
                {

                    Parallel.ForEach(mergeRequests, request =>
                              {
                                  try
                                  {
                                      var line = $"{request.Id},{request.CreateAt},{request.Assignee.UserName},{request.Title}\n";
                                      var buffer = Encoding.Unicode.GetBytes(line);
                                      file.WriteAsync(buffer, 0, buffer.Length);
                                  }
                                  catch (Exception ex)
                                  {
                                      _logger.Error(ex, "Cannot process input.");
                                  }
                              });

                }
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(ex, "Cannot found the report file.");
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.Error(ex, "Not sufficent permissin to access to file.");
                return false;
            }
            finally
            {
                file?.Close();
            }

            return true;
        }
    }
}
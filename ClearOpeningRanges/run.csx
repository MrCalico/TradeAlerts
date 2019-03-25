#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public class Range : TableEntity
{
    public string Symbol { get; set; }
}

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, CloudTable outTable, IQueryable<Range> inputTable, TraceWriter log)
{
    dynamic data = await req.Content.ReadAsAsync<object>();

    int count = 0;

    var query = from range in inputTable select range;
    foreach (Range range in query) {
       outTable.Execute(TableOperation.Delete(range));
       count++;
    }
    return req.CreateResponse(HttpStatusCode.Created, $"{count} Records Deleted!");
}



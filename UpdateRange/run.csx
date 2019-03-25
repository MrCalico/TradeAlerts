#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, CloudTable outTable, IQueryable<OpeningRangeTable> inputTable, TraceWriter log)
{
    dynamic data = await req.Content.ReadAsAsync<object>();
    string symbol = data.symbol;


    if (symbol == null)
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a symbol in the request body");
    }


    //int count = 0;
    //IEnumerable<OpeningRangeTable> query = from range in inputTable select range;
    //foreach (OpeningRangeTable range in query) {
    //   outTable.Execute(TableOperation.Delete(range));
    //   count++;
    //}
    //TableOperation retrieveOperation = TableOperation.Retrieve<OpeningRangeTable>("Smith", "Ben");
    //TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace()
    OpeningRangeTable range = new OpeningRangeTable();

    IEnumerable<OpeningRangeTable> query = from r in inputTable select r;

    return req.CreateResponse(HttpStatusCode.Created, $"{count} Records Deleted!");
}

public class OpeningRangeTable : TableEntity
{
    public string Symbol  { get; set; }
    public DateTime Date  { get; set; }
    public double Low     { get; set; }
    public double High    { get; set; }
    public string AStatus { get; set; }
    public double APrice  { get; set; }
    public double AStop   { get; set; }
    public string CStatus { get; set; }
    public double CPrice  { get; set; }
}

public class OpeningRange
{
    public string symbol  { get; set; }
    public string date    { get; set; }
    public double low     { get; set; }
    public double high    { get; set; }
    public string astatus { get; set; }
    public double aprice  { get; set; }
    public double astop   { get; set; }
    public string cstatus { get; set; }
    public double cprice  { get; set; }
}
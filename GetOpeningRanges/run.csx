#r "Microsoft.WindowsAzure.Storage"
//#r "Newtonsoft.Json"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;
//using Newtonsoft.Json;
//using System.Net.Http.Headers;

public class OpeningRangeTable : TableEntity
{
    public string Symbol  { get; set; }
    public DateTime Date  { get; set; }
    public double Low     { get; set; }
    public double High    { get; set; }
    public string Status { get; set; }
    public string Trade { get; set; }
    public double Limit  { get; set; }
    public double Stop   { get; set; }
}

public class OpeningRange
{
    public string symbol  { get; set; }
    public string date    { get; set; }
    public double low     { get; set; }
    public double high    { get; set; }
    public string status { get; set; }
    public string trade { get; set; }
    public double limit  { get; set; }
    public double stop   { get; set; }
}

public static HttpResponseMessage Run(HttpRequestMessage req, IQueryable<OpeningRangeTable> inTable, TraceWriter log)
{
    List<OpeningRange> OpeningRanges = new List<OpeningRange>();
    var query = from range in inTable select range;
    foreach (OpeningRangeTable range in query)
    {
        log.Info($"Symbol:{range.Symbol} {range.Low}:{range.High}");
        OpeningRanges.Add( 
            new OpeningRange() {
                symbol= range.Symbol, 
                date  = range.Date.ToString(),
                low   = range.Low,
                high  = range.High,
                status = range.Status,
                trade = range.Trade,
                limit  = range.Limit,
                stop   = range.Stop,
        });

    }
    return req.CreateResponse(HttpStatusCode.OK, OpeningRanges);
}

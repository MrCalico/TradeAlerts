#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static HttpResponseMessage Run(Range range, CloudTable outTable, TraceWriter log)
{
    if (string.IsNullOrEmpty(range.Symbol))
    {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("A non-empty Symbol must be specified.")
        };
    };

    log.Info($"Symbol={range.Symbol}");
    range.PartitionKey = range.Symbol;
    range.RowKey = range.Symbol;
    range.Symbol = range.Symbol;
    range.ETag = "*";
    range.Low = 113.13;

    var Qop = TableOperation.Retrieve<Range>("Symbol",range.Symbol);
    var rsQop = outTable.Execute(Qop);
    Range updateEntity =  (Range)rsQop.Result;
    if (updateEntity != null)
    {
        updateEntity.Low = 1;
        updateEntity.High = 2;
        var Uop = TableOperation.Replace(range);
        var res = outTable.Execute(Uop);
    }
    else
        log.Info("Null Returned from Retrieve.");
    //log.Info(rsQop.ToString());

    //var Uop = TableOperation.Replace(range);
    //var res = outTable.Execute(Uop);
    //log.Info(res.ToString());

    return new HttpResponseMessage(HttpStatusCode.NoContent);

    /*
    int count = 0;
    var query = from ranges in outTable select ranges;
    foreach (range in ranges) {
        if (range.Symbol == Range.Symbol)
            count++;
    }
    return new HttpResponseMessage(HttpStatusCode.OK, new { count })
    //TableOperation updateOperation = TableOperation.InsertOrReplace(Range);
    //TableResult result = outTable.Execute(updateOperation);
    //return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
    */
}

public class Range : TableEntity
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
/*
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
*/
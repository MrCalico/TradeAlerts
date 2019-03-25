#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"

using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

public static async Task<object> Run(HttpRequestMessage req, ICollector<OpeningRangeTable> outTable, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    List<OpeningRange> OpeningRanges = JsonConvert.DeserializeObject<List<OpeningRange>>(jsonContent);

    string message = "";
    foreach (OpeningRange o in OpeningRanges) 
    { 
        message += $"{o.symbol},{o.low},{o.high}; ";
        
        outTable.Add(new OpeningRangeTable()
        {
            PartitionKey = "Functions",
            RowKey = Guid.NewGuid().ToString(),
            Symbol = o.symbol,
            Date = Convert.ToDateTime(o.date),
            Low = o.low,
            High = o.high,
            Status = o.status,
            Trade = o.trade,
            Limit = o.limit,
            Stop = o.stop,
        });
        
    }

    return req.CreateResponse(HttpStatusCode.OK, new
    {
        message
    });
}

public class OpeningRangeTable : TableEntity
{
    public string Symbol { get; set; }
    public DateTime Date { get; set; }
    public double Low    { get; set; }
    public double High   { get; set; }
    public string Status { get; set; }
    public string Trade  { get; set; }
    public string Quantity { get; set; }
    public double Limit  { get; set; }
    public double Stop   { get; set; }
}

public class OpeningRange
{
    public string symbol { get; set; }
    public string date   { get; set; } = DateTime.Now.ToString();
    public double low    { get; set; }
    public double high   { get; set; } 
    public string status { get; set; }
    public string trade { get; set; }
    public int quantity { get; set; }
    public double limit  { get; set; }
    public double stop   { get; set; }
}

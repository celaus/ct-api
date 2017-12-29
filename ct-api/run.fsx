#r "System.Net.Http"
#r "Newtonsoft.Json"

open System.Net
open System.Net.Http
open FSharp.Interop.Dynamic
open Newtonsoft.Json

type Ticker = {
    exchange: string
    /// UNIX timestamp in ms (when the response was received)
    timestamp: uint64
    /// The Pair corresponding to the Ticker returned (maybe useful later for asynchronous APIs)
    pair: string
    /// Last trade price found in the history
    last_trade_price: string
    /// Lowest ask price found in Orderbook
    lowest_ask: string
    /// Highest bid price found in Orderbook
    highest_bid: string
    // Bittrex does not support Volume for ticker so volume could be None
    /// Last 24 hours volume (quote-volume)
    volume: uint64
}

type Response = {
    message: string
}

let Run(req: HttpRequestMessage, newTicker : byref<obj>, log: TraceWriter) =
    try
        let data = req.Content.ReadAsStringAsync().Result
        let tickerdata = JsonConvert.DeserializeObject<Ticker>(data)
        newTicker <- tickerdata
        let response = req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject({message = "Ok"}))
        response.Headers.Add("ContentType", "application/json")
        response
    with e ->
        let response = req.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject({ message = e.ToString() } ))
        response.Headers.Add("ContentType", "application/json")
        response
#r "System.Net.Http"
#r "Newtonsoft.Json"

open System.Net
open System.Net.Http
open FSharp.Interop.Dynamic
open Newtonsoft.Json

type Ticker = {
    exchange: string
    timestamp: uint64
    pair: string
    last_trade_price: string
    lowest_ask: string
    highest_bid: string
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
        response.Headers.Add("content-type", "application/json")
        response
    with e ->
        let response = req.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject({ message = e.ToString() } ))
        response.Headers.Add("content-type", "application/json")
        response
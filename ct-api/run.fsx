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

let Run(req: HttpRequestMessage, newTicker : byref<obj>, log: TraceWriter) =
    match req.Method.Method with
        | "POST" -> 
            try
                let data = req.Content.ReadAsStringAsync().Result
                let tickerdata = JsonConvert.DeserializeObject<Ticker>(data)
                newTicker <- tickerdata
                req.CreateResponse(HttpStatusCode.OK)
            with e -> req.CreateErrorResponse(HttpStatusCode.BadRequest, "")
        | _ -> req.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "")
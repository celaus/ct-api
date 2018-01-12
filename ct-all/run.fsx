#r "System.Net.Http"
#r "Newtonsoft.Json"

open System.Net
open System.Net.Http
open System.Text
open Newtonsoft.Json

open FSharp.Interop.Dynamic

let Run(req: HttpRequestMessage, tickers: seq<obj>, log: TraceWriter) =
    async {
        let res = new HttpResponseMessage(HttpStatusCode.OK) 
        res.Content <- new StringContent(JsonConvert.SerializeObject(tickers), Encoding.UTF8, "application/json")
        return res
     } |> Async.RunSynchronously

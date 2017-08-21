module fableredux2

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import


module Redux =
    open System
    open Fable.Import
    open Fable.Core
    open Fable.Core.JsInterop

    type IStore<'TState, 'TAction> =
        abstract dispatch: 'TAction->unit
        abstract subscribe: (unit->unit)->unit
        abstract getState: unit->'TState

    let private createStore_: JsFunc = import "createStore" "redux"

    let createStore (reducer: 'TState->'TAction->'TState) (initState: 'TState): IStore<'TState, 'TAction> =
        // Check if the action is a lifecycle event dispatched by Redux before applying the reducer
        let jsReducer =
            fun state action ->
                match !!action?``type``: obj with
                | :?string as s when s.StartsWith "@@" -> state
                | _ -> reducer state action
        match !!Browser.window?devToolsExtension: JsFunc with
        | null -> !!createStore_.Invoke(jsReducer, initState)
        | ext -> !!createStore_.Invoke(jsReducer, initState, ext.Invoke())
        
module R = Fable.Helpers.React
open R.Props        

let init() =
    let canvas = Browser.document.getElementsByTagName_canvas().[0]
    canvas.width <- 1000.
    canvas.height <- 800.
    let ctx = canvas.getContext_2d()
    // The (!^) operator checks and casts a value to an Erased Union type
    // See http://fable.io/docs/interacting.html#Erase-attribute
    //ctx.fillStyle <- !^"rgb(200,0,0)"
    ctx.fillRect (10., 10., 55., 50.)
    //ctx.fillStyle <- !^"rgba(0, 0, 200, 0.5)"
    ctx.fillRect (30., 30., 55., 50.)

init()
namespace EpoxyHello.ViewModels

open Epoxy
open System

open EpoxyHello.Models

[<Sealed; ViewModel>]
type public MainWindowViewModel() as self =
    do
        // A handler for window loaded
        self.Ready <- Command.Factory.create(fun () -> async {
           self.Title <- "Hello Epoxy!"
        })
 
    member val Ready: Command = null with get, set
    member val Title: string = "" with get, set

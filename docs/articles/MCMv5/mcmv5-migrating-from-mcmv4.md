### Notable changes:
* MCM has moved a lot of namespaces to split it's codebase into 4 parts  
  * `MCM.Common` that contains the most basic MCM classes.  
  * `MCM.Abstractions` that contains core MCM code that is fully independent from the game's abstractions.  
  * `MCM.Implementation` that contains default implementation classes for MCM's abstractions.  
  * `MCM.Bannerlord` contains the code that glues together MCM and Bannerlord.  

  By making most of the MCM's code independent from the game's abstractions we ensure that MCM won't break in a way that we can't fix without breaking our public ABI and API.

* `DropdownDefault<>` was renamed to `Dropdown<>`

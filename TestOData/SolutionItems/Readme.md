# Coding Standards

The almost-too-concise document from coding style to design.

### Table of Contents
1. [Template Solution Layout](##Template_Solution_Layout)
   * [Layers](###Layers)
   * [Supporting Projects](##Supporting_Projects)
2. [Project Design Best Practices](##Best_Practices)
    * [General](###General)
    * [Api](###Api)
    * [Service](###Service)
    * [DataAccess](###DataAccess)
    * [Model](###Model)
    * [Interface](###Interface)
    * [UnitTest](###UnitTest)
3. [Coding Style](##Coding_Style)
    * [Case](###Case)
    * [Files](###Files)
    * [Types](###Types)
    * [Code Structure](###Code_Structure)
**
**
## Template Solution Layout <a name="Template_Solution_Layout">
### Layers <a name="Layers">
This template uses several different projects to accomplish specific tasks, following the pattern of Layered Architecture (a.k.a. [Multitier Architecture](https://en.wikipedia.org/wiki/Multitier_architecture)).
These are the following layers (projects) as well as their purpose:

* **Api**: The Api layer contains the information required for a client to reach the endpoints, such as controllers,
filters, and hosting information (e.g. setup, config, etc.). Endpoints are responsible for incoming data validation and HTTP responses
(e.g. OK, Created, Bad Request, Not Found, Conflict, etc.) It should be remain free of business logic, delegating all
of the work from the controller endpoint to the **Service** layer.

* **Service**: The Service layer contains all of the business logic for processing the incoming data. This layer is
 typically where the bulk of the work is done in the application. Typically, processing will require interaction with a 
datastore, such as a database. The Service layer will use the data access layer, **DataAccess**, to handle any database and
subsequent data-mapping calls.

* **DataAccess**: The DataAccess layer has the responsibility of making calls to data sources, including sending and receiving data.
 Like the Api, DataAccess should also be free of business logic. Data received must be parsed from its table form into its 
corresponding domain model, as defined in the Model project.

### Supporting Projects<a name="Supporting_Projects">
In an effort to support the main layers, there are some other projects which can almost be considered layers themselves.

* **Model**: The Model project contains all the POCOs used by the application. These are simple objects which do not contain logic,
 but are the vehicles which drive all of the processing in the API. Objects that are used as endpoint input and outputs should be 
defined here.

* **Interface**: The Interface project contains all of the interfaces used by the Service and DataAccess projects. The reason for 
 the separate project is so that clients can import a single assembly that is not hard-linked to the implementation layer. All 
of the classes in the Service and DataAccess project will have a corresponding interface.

* **UnitTests**: The UnitTests project, as implied, contains all the unit tests for the application.

Note: The remaining project, SolutionItems, is actually just a project that contains solution-level items. Due to the nature of
templates, loose files (unfortunately) cannot be packaged in a multi-project template.

## Best Practices <a name="Best_Practices">
This section contains several best practices, from general to specific implementation ideas. These are more guidelines than rules,
but care should be taken to avoid messy code.

### General <a name="General">
- Software is written for human understanding. Clear, consistent, maintainable code is of utmost importance.
  * Note that performant code and clear code are not against each other; both can be achieved.
- Adhere to [S.O.L.I.D](https://en.wikipedia.org/wiki/SOLID) design principles within reason.
- Interfaces should be used liberally, and favored over inheritance ([Composition over Inheritance)](https://en.wikipedia.org/wiki/Composition_over_inheritance).
    - This leads to cleaner code, as well as easy, straightforward unit testing.
- Coding is not finished until proven to be working via testing.
  - Unit testing is not only a measure of code quality, but also proof that the production requirements are being met. 
- Folder structure should match relatively closely among the different layers 
  * e.g. if there's a /Foo folder for handling Foos in the Api project, then there should be corresponding
         /Foo folders in the Model, Service, DataAccess, and Interface projects

### Api <a name="Api">
- Controllers should only contain Application logic (incoming data validation, HTTP Responses).
  - Business logic should be delegated to the service layer.
- Endpoints should have authentication and authorization.
   

### Service <a name="Service">
- Append "Service" to services classes, and "Helper" to helper classes.
- Several small, single purposes services should be used to keep in line with S.O.L.I.D principles (specifically, Single Responsibility Principle).
- Keep classes internal.
- Avoid static classes in favor of injectable interfaces.

### DataAccess <a name="DataAccess">
- Group similar business-line queries into a repository class for that business line 
  - e.g. a FooRepository for accessing table Foo, executing the `do_foo` and `process_foo_bar` SPROC (*Avoid god classes*).
- Models that represent database tables or SPROC return types should be appended with "Entity" to distinguish from application domain models.
- Unless mapping from an "Entity" model to a "Domain" model is trivial (e.g. `domain.foo = entity.foo`), a mapper should be used.
- Handle transactions and rollbacks at this level.
- Propigate errors to the service layer.
- Keep classes internal.

### Model <a name="Model">
- Models should only contain POCOs with no logic.

### Interface <a name="Interface">
- Interfaces should be created in the same namespace/folder structure path as their implementation counterparts.

### UnitTest <a name="UnitTest">
- Unit tests are written for developers. If a developer breaks something, the unit test should immediately make it clear what 
    broke and why.
- Unit tests should only test a unit of code (e.g. a method) and *not* the dependencies.
- The service, repository, etc. being tested should have its variable name be `sut` (Service Under Test).
  * This makes it immediately clear what service is being tested
- The `sut` should be defined with its interface, and set as its implementation.
  * e.g. `IFooService sut = new FooService(mockBarService.Object... )`.
- Mocks should always be used with `DefaultBehavior.Strict` and verified in the `Dispose()` method.
- The only test setup method should be the constructor.
- The *only* content in the constructor should be setting up the mocks and initializing the sut.
- Test methods should follow the name format: \<NameOfMethod>\_When\<Case>\_\<ExpectedResult>
  * e.g. `GetWeatherData_WhenWeatherDataFound_ReturnsWeeklyForecast()`
- All branches of a method should be tested.

## Coding Style  <a name="Coding_Style">
### Case  <a name="Case">
- Use `Pascal` casing for class names
    * Do: 
        ```c#
        public class FooBar
        {
            ...
        }
        ```
    * Don't:
        ```c#
        public class fooBar
        {
            ...
        }
        ```
- Use `Pascal` casing for method names
    * Do: 
        ```c#
        public void DoSomething()
        {
            ...
        }
        ```
    * Don't:
        ```c#
        public void doSomething()
        {
            ...
        }
        ```
- Use `camelCasing` for variables and parameters
     * Do: 
        ```c#
        public void DoSomething(int myVariable)
        {
            ...
        }
        ```
    * Don't:
        ```c#
        public void DoSomething(int MyVariable)
        {
            ...
        }
        ```
- Class variables should begin with an underscore.  A "_" makes it more obvious what is a global member vs a scoped member, etc.
    * Do:
        ```c#
        private int _myVariable;
        ```
    * Don't:
        ```c#
        private int myVariable;
        ```
- Constants should be all **UPPER** case and words separated with an "_"
    * Do:
        ```c#
        private int MY_VARIABLE = 42;
        ```
    * Don't:
        ```c#
        private int my_Variable = 42;
        ```
        or
        ```c#
        private int myVariable = 42;
        ```
### Files  <a name="Files">
- Place each `class`, `interface`, etc. in their own physical file.  This makes it simpler to find/identify on the file system and Solution Explorer
    * Do:
        ```c#
        public class Foo
        {
            ...
        }
        ```
    * Don't:
        ```c#
        public class Foo
        {
            ...
        }

        public class Bar
        {
            ...
        }
        ```
- Projects should have a logically grouped folder structure
    * Do:
        ```c#
        Project
            /Controllers
                MyController.cs
                MyOtherController.cs
            /Filters
                WhiteListFilter.cs
                AuthenticationFilter.cs
            /Exceptions
                MyControllerException.cs
                UnauthenticatedException.cs
        ```
    * Don't:
        ```c#
        Project
            MyController.cs
            MyOtherController.cs
            WhiteListFilter.cs
            AuthenticationFilter.cs
            MyControllerException.cs
            UnauthenticatedException.cs
        ```

- A files namespace should match the file path.
    * Do:
        ```c#
        MyProject/Service/Foo
        ---
        namespace MyProject.Service;

        public class Foo
        {
            ...
        }
        ```
    * Don't:
        ```c#
        MyProject/Service/Foo
        ---
        namespace MyProject;

        public class Foo
        {
            ...
        }
        ```
- Filename must match the name of the `class`, `interface`, etc.
    * Do:
        ```c#
        Foo.cs:
        
        public class Foo
        {
            ...
        }
        ```
    * Don't:
        ```c#
        Bar.cs:
        
        public class Foo
        {
            ...
        }}
        ```
### Types <a name="Types">
- Use implicitly typed variables (e.g. `var`)
    * Do:
        ```c#
        var foo = 1;
        ```
    * Don't:
        ```c#
        int foo = 1;
        ```
- [Never use dynamic.](https://softwareengineering.stackexchange.com/questions/256566/when-to-not-use-dynamic-in-c)
  Strongly typed data is one of the greatest strengths of C#. `dynamic` causes runtime bugs and
  rots codebases. If it doesn't work without `dynamic`, rewrite it to use types.
    * Do:
        ```c#
        Foo foo = fooService.GetFoo();
        ```
    * Don't:
        ```c#
        dynamic foo = fooService.GetFoo();
        ```
### Code Structure <a name="Code_Structure">
- Spaces, not tabs.
    * Do:
        ```c#
        if(foo)
        {
        ••••return 1;
        }
        ```
    * Don't:
        ```c#
        if(foo)
        {
        --->return 1;
        }
        ```

- Put curly braces on their own line
    * Do:
        ```c#
        if(foo)
        {
            ...
        }

        for(var i = 0; i < 10; i++)
        {
            ...
        }
        ```
    * Don't:
        ```c#
        if(foo){
            ...
        }

        for(var i = 0; i < 10; i++){
            ...
        }
        ```
- Always use curly braces. Consistency is important in a large codebase, and at-a-glance readability could mean
  the difference between working code and a [production error](https://www.imperialviolet.org/2014/02/22/applebug.html)
    * Do:
        ```c#
        if(foo)
        {
            return 1;
        }
        ```
    * Don't:
        ```c#
        if(foo)
            return 1;
        ```
- Always put an empty line after a closing `}`
    * Do:
        ```c#
        if(foo)
        {
            ...
        }

        var bar = 2;
        ```
    * Don't:
        ```c#
        if(foo)
        {
            ...
        }
        var bar = 2;
        ```

- Never use more than one newline in a row
    * Do:
        ```c#
        if(foo)
        {
            ...
        }

        var bar = 2;
        ``` 
    * Don't:
        ```c#
        if(foo)
        {
            ...
        }


        var bar = 2;
        ```
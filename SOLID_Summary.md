# SOLID Principles in IceCity

This document summarizes the SOLID principles and how they apply to the IceCity project.

## 1️⃣ S — Single Responsibility Principle (SRP)
**What does it mean?**
A class should have one, and only one, reason to change. This means that a class must have a single, well-defined responsibility within the system.

**Example from IceCity project:**
Initially, `Program.cs` or `CalculationService` might have handled calculating costs, fetching data from APIs, and printing reports to the console. By moving cost calculation into `CalculationService` and the calculation formulas into `StandardCostStrategy` / `EcoCostStrategy`, we ensured that `CalculationService` is only responsible for coordinating calculations, and the strategy classes are only responsible for the math.

**What happens when SRP is violated?**
If a class handles too much (e.g., UI, database, logic), a change in one requirement (like a new cost formula) might break unrelated features (like report generation). Code becomes tangled, fragile, and hard to test.

## 2️⃣ O — Open/Closed Principle (OCP)
**What does "open for extension, closed for modification" mean?**
Software entities (classes, modules, functions) should be open for extension (you can add new behavior) but closed for modification (you shouldn't change the existing source code to add that behavior).

**Why is it important for cost calculation logic?**
If the mayor introduces a new calculation method, we shouldn't have to rewrite the `CalculationService` or the `StandardCostStrategy`. Instead, we simply create a new class (e.g., `SolarCostStrategy` or `EcoCostStrategy`) that implements `ICostCalculationStrategy`. The existing logic remains untouched and safe from regressions.

## 3️⃣ L — Liskov Substitution Principle (LSP)
**What happens if child class breaks base behavior?**
LSP states that objects of a superclass shall be replaceable with objects of its subclasses without breaking the application. If a child class changes the expected behavior (e.g., throwing an unexpected `NotImplementedException` or returning fundamentally different units), the system will crash or behave incorrectly when treating it as the base class.

**Simple example with Heater types:**
If we have a base class `Heater`, and we create a `SolarHeater : Heater`, the `SolarHeater` must still be able to `Open()` and `Close()` properly and produce a valid `powerValue`. If `SolarHeater` threw an exception on `Open()` because "the sun is down," without the system knowing how to handle it generically, it would violate LSP.

## 4️⃣ I — Interface Segregation Principle (ISP)
**Why large interfaces are bad?**
A large, "fat" interface forces implementing classes to define methods they don't need. This leads to dummy implementations and tight coupling to features the class doesn't care about.

**Example in service layer:**
If we had an `IHeaterService` that included `CalculateCost()`, `ReplaceHeater()`, and `FetchWeather()`, a class that only calculates costs would be forced to implement weather fetching. Instead, we split them into smaller interfaces like `ICostCalculationStrategy`.

## 5️⃣ D — Dependency Inversion Principle (DIP)
**Why should services depend on abstractions?**
High-level modules should not depend on low-level modules; both should depend on abstractions (interfaces). This decouples components. If `CalculationService` depends on a specific `StandardCostStrategy` class, it's stuck with it. If it depends on `ICostCalculationStrategy`, we can swap strategies dynamically.

**How does ASP.NET Core DI container help?**
A Dependency Injection (DI) container acts as a central registry. Instead of classes instantiating their dependencies (e.g., `new StandardCostStrategy()`), the DI container creates and provides them automatically based on the registered interfaces. This makes the system loosely coupled and extremely easy to test using mocks.
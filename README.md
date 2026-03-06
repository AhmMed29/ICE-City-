# 🧊🔥 ICE-City — Your Home's Heater, Now With Brain Cells

> *"Because guessing your electricity bill at the end of the month and crying into your wallet is SO last season."* 😭💸

[![C#](https://img.shields.io/badge/language-C%23-blue?logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-8-purple?logo=dotnet)](https://dotnet.microsoft.com/)
[![Console App](https://img.shields.io/badge/type-Console%20App-green)]()
[![Built with ☕](https://img.shields.io/badge/built%20with-%E2%98%95%20coffee%20at%2012%3A32%20AM-brown)]()

---

## 🤔 What Is ICE-City?

**ICE-City** is a C# console application that helps homeowners track, analyze, and report on their **heater usage** throughout the month — and gives them a clear picture of how much that cozy warmth is actually costing them. 🏠🔥

Think of it as your personal **energy accountant**, but without the boring tie and briefcase.

---

## 😅 Why Does This Exist?

You know that feeling when the electricity bill arrives and you stare at it like:

> *"I KNOW I left that heater on... but for HOW LONG?"* 🤨

ICE-City was born to answer exactly that question. No more mystery bills. No more blaming the dog for leaving the heater on. Just cold, hard (ironic, we know) data. 🐕❌🔥

---

## ✨ Features

| Feature | What It Does |
|---|---|
| 👤 **Owner Management** | Register yourself as the home owner |
| 🔌 **Heater Profiling** | Define your heater (power in KW + type: Gas or Electric) |
| ⏱️ **Daily Usage Tracking** | Log how many hours your heater ran each day of the month |
| 📊 **Monthly Reports** | Get total working hours, median heater value, and average cost |
| 🧾 **Owner & Heater Reports** | Clean, printed summaries right in your terminal |

---

## 🚀 Real-World Use Cases

### 🏡 Use Case 1: "Ahmed, The Forgetful Engineer"

Ahmed runs his electric heater every morning before work. He *thinks* it runs about 2 hours a day. ICE-City tells him the **actual total is 79.5 hours** for the month.

Ahmed's reaction:

> *"That explains everything. And nothing. Mostly my electric bill."* 😂

**How Ahmed uses ICE-City:**

```
Owner Name : Ahmed
Heater Power (Kilowatt) : 2.5
Heater Type (Gas OR Electric) : Electric
```

Output:
```
-----------Owner Report-------------
Owner Name: Ahmed
Total Working Hours For This Month : 79.5
Median Heater Value : 13.77
Average Cost For This Month : 0.248
-----------------------------------------

-----------Heater Report------------
Owner Name Ahmed
Heater Type Electric
Heater Power 2.5 KW
-----------------------------------------
```

---

### 🏘️ Use Case 2: "Sara, The Budget Queen"

Sara manages a small house with a gas heater. She uses ICE-City to compare the **median heater value** month over month to see if her heating habits are changing. Spoiler: they are. Turns out she blasts the heater every Friday night for movie marathons. 🎬🍿

ICE-City gives Sara the data she needs to decide: *"Do I change my habits, or do I just stop watching scary movies that need blankets?"*

---

### 🏢 Use Case 3: "Mohamed, The Landlord"

Mohamed rents out multiple units and wants to generate heater usage reports for each tenant. ICE-City's `Owner` and `House` model lets him create separate profiles — one per tenant — and produce individual reports at the end of the month.

No more arguing about who used what. The data doesn't lie. (Unlike tenants.) 📋😤

---

## 🏗️ Project Structure

```
ICE-City/
├── 📄 Program.cs            — App entry point, user input & flow
├── 👤 Owner.cs              — Represents the homeowner
├── 🔥 Heater.cs             — Heater model (power + type)
├── 🏠 House.cs              — House containing owner & heaters
├── 🌡️  HeaterType.cs        — Enum: Gas | Electric
├── 📅 DailyUsage.cs         — Daily heater usage data model
├── 📊 Report.cs             — Prints owner & heater reports
└── Services/
    └── 🧮 Calculations.cs   — All the mathy goodness
```

---

## 🧮 How the Math Works

### Total Working Time
```
Sum of all daily working hours across the month
```

### Median Heater Value
```
Average of all daily heater readings
```

### Monthly Average Cost
```
(Sum of hours / count) × (Sum of hours / (24 × 30))
```

> *Yes, the math is still evolving. The developer was writing this at 12:32 AM. We respect the grind.* 😤🌙

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later
- A terminal (cmd, PowerShell, bash — your pick 🎯)
- Your heater's specs handy: power (in KW) and type (Gas or Electric) 🔌

### Run It

```bash
git clone https://github.com/AhmMed29/ICE-City-.git
cd ICE-City-/IceCity
dotnet run
```

### Example Session

```
Owner Name : Your Name Here
Heater Power (Kilowatt) : 3.0
Heater Type (Gas OR Electric) : Gas
```

Then sit back and let ICE-City do the math while you make tea. ☕

---

## 🛠️ Tech Stack

| Technology | Version |
|---|---|
| **Language** | C# |
| **.NET** | 8+ |
| **App Type** | Console Application |
| **Fuel** | ☕ Coffee (a lot of it) |

---

## 🗺️ Roadmap

- [x] Owner & Heater profiling
- [x] Monthly usage calculations
- [x] Owner and Heater reports
- [ ] Interactive daily input loops (coming soon™ 👀)
- [ ] Multi-house support per owner
- [ ] Cost comparison: Gas vs Electric
- [ ] Export reports to CSV or PDF
- [ ] Don't write code at 12:32 AM (debatable 🌙)

---

## 🤝 Contributing

Got ideas? Found bugs? Want to save people from crying at their utility bills?

1. Fork this repo 🍴
2. Create your feature branch: `git checkout -b feature/save-my-wallet`
3. Commit your changes: `git commit -m "💡 Add feature that makes bills smaller"`
4. Push and open a PR 🚀

---

## 👨‍💻 Author

Built with passion (and sleep deprivation) by **AhmMed29** 🧠

> *"I know the task is not completed but I will add other features as the clock now is 12:32 AM"*
> — the developer, in the actual source code 😂

---

## 📜 License

This project is open source. Feel free to use it, improve it, and never again be surprised by your heating bill. 🙌

---

*🧊 Stay cool. Track your heat. ICE-City's got you.* ❄️🔥

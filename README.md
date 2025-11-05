# ✂️ Cut Optimizer: Intelligent Rebar Cutting and Material Waste Reduction Add-in 🏗️  

![Revit](https://img.shields.io/badge/Revit-2022-blue?logo=autodesk&logoColor=white)
![C#](https://img.shields.io/badge/C%23-.NET%204.8-512BD4?logo=csharp)
![License](https://img.shields.io/badge/License-MIT-green)
![Status](https://img.shields.io/badge/Status-Active-success)
![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey?logo=windows)

---

## 💡 Project Summary  

**Cut Optimizer** is an intelligent **Autodesk Revit Add-in** designed to revolutionize **rebar fabrication workflows**.  
Developed in **C#** using the **Revit API**, this add-in optimizes **rebar cutting lengths** through advanced **value engineering algorithms** — minimizing material waste and maximizing efficiency in structural projects.

By integrating directly into the Revit environment, it automates complex rebar analysis and provides **optimized cutting schedules** ready for fabrication.

---

## ⚙️ Tech Stack  

| Category | Technology |
|-----------|-------------|
| **Platform** | Autodesk Revit 2022 |
| **Language** | C# |
| **Framework** | .NET Framework 4.8 |
| **API** | Autodesk Revit API |
| **IDE** | Visual Studio 2022 |

---

## 🚀 Key Features & Value Engineering  

The **Cut Optimizer** is essential for projects focused on **cost reduction**, **efficiency**, and **sustainability** in rebar construction.

✅ **Rebar Cutting Optimization**  
Automatically analyzes and calculates the most efficient rebar cutting patterns — minimizing scrap and maximizing stock utilization.  

♻️ **Waste Reduction**  
Supports value engineering efforts by optimizing available stock lengths against project requirements, achieving significant material savings.  

🏗️ **Seamless Revit Integration**  
Operates directly within Revit, analyzing and updating rebar geometry and parameters without the need for external tools.  

⚙️ **Automated Data Processing**  
Processes large volumes of rebar data quickly and precisely, outperforming manual workflows.  

📋 **Optimized Fabrication Schedules**  
Generates transparent and auditable cutting schedules that can be exported for fabrication use.  

---

## 🧱 How It Works  

1. Open your Revit project containing rebar elements.  
2. Navigate to the **EDECS Tools → Cut Optimizer** tab.  
3. The tool scans all rebar instances and computes optimized cut patterns.  
4. A report of optimized lengths and waste reduction statistics is displayed and can be exported.  

---

## 🧩 Add-in Interface  

> 📸 *Cut Optimizer in Action*  

![Cut Optimizer Screenshot](demo/cut_optimizer_screenshot.png)  
*(Replace the placeholder image with an actual screenshot once available.)*

---

## 🧮 Example of Value Engineering Output  

| Rebar Type | Required Lengths | Stock Length | Waste (%) | Optimized Cuts |
|-------------|------------------|--------------|------------|----------------|
| T12 | 800, 1200, 1500 | 12000 | 3.5% | 8 × 1500, 2 × 1200, 1 × 800 |
| T16 | 900, 1000, 1800 | 12000 | 2.8% | 6 × 1800, 2 × 1000, 1 × 900 |
| T20 | 2500, 2600 | 12000 | 4.1% | 4 × 2500, 1 × 2600 |

---

Cut Optimizer/
├── CutOptimizer/ # Source Code (C#, Revit API)
├── CutOptimizer.sln # Visual Studio Solution File
├── demo/ # Screenshots or Demo Videos
├── README.md # Project Documentation
├── .gitignore # Git Ignore Rules
└── .gitattributes # Git Attributes

---

## 🚀 Installation  

1. **Clone** or **download** this repository.  
2. Open `CutOptimizer.sln` in **Visual Studio**.  
3. Build the solution to generate the `.dll` and `.addin` files.  
4. Copy the files to your Revit Add-ins directory:  
   ```bash
   C:\ProgramData\Autodesk\Revit\Addins\202x\

🔮 Future Enhancements

📊 Integration with Rebar Weight & Cost Analysis

🧱 Support for Custom Stock Length Libraries

💾 Export to Excel and JSON formats

🪟 Enhanced WPF UI and Optimization Preview Chart

🌍 Extend Support to Revit 2024+

👤 Developer

Youssef Hesham
Senior AEC Software Developer | BIM Specialist | Automation Specialist






🪪 License

This project is licensed under the MIT License — you are free to use, modify, and distribute it with attribution.

💬 “Every millimeter saved in steel is a step closer to sustainable construction.”
— Youssef Hesham ✂️🏗️


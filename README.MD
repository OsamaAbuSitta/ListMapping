# 📚 **ListMapping**

**ListMapping** is a library designed to simplify and optimize the process of synchronizing two lists based on matching identifiers. Say goodbye to repetitive nested `for` loops when mapping lists!

---

## 🐞 **The Problem**

When mapping two lists in C#, developers often need to **nested loops** or repetitive code patterns:

```csharp
foreach (var source in sourceList)
{
    var destination = destinationList.FirstOrDefault(d => d.Id == source.Id);
    if (destination != null)
    {
        destination.Name = source.Name;
    }
    else
    {
        destinationList.Add(new Destination { Id = source.Id, Name = source.Name });
    }
}

// Remove unmatched items
destinationList.RemoveAll(d => !sourceList.Any(s => s.Id == d.Id));
```

### 🚨 **Issues with this approach:**
1. **Verbose and Repetitive:** The same mapping logic gets written repeatedly.  
2. **Error-Prone:** Manual handling increases the chance of bugs.  
3. **Difficult to Maintain:** Changes in mapping logic require updates in multiple places.  

---

## ✅ **The Solution**

**ListMapping** provides a cleaner and more reusable way to handle list mapping with just one line of code.

### **1. Mapping by Property Name**

Map two lists using property named ("Id"):

```csharp
using ListMapping;

destinationList.MapList(sourceList);
```

### **2. Mapping by Lambda Expressions**

Map two lists using lambda expressions for better type safety:

```csharp
using ListMapping;

destinationList.MapList(
    sourceList,
    src => src.Id,
    dest => dest.Id
);
```

### **3. Mapping by Property Name**

Map two lists using property names:

```csharp
using ListMapping;

destinationList.MapList(sourceList, "Id", "Id");
```



✅ **No more nested loops or manual mapping logic!**

---

## 🛠️ **Key Benefits**

- **Clean Code:** One line replaces multiple nested loops.  
- **Reusable Logic:** Abstracts the mapping logic for repeated use.  
- **Performance Optimized:** Cached property accessors for better runtime performance.  
- **Flexible:** Supports both **property names** and **lambda expressions**.  

---

## 📊 **Use Cases**

- Synchronizing data from external sources (e.g., API responses).  
- Mapping DTOs to domain models or vice versa.  
- Keeping two lists in sync in business workflows.

---

## 📦 **Installation**

Install via NuGet:

```sh
dotnet add package ListMapping --version 1.0.0
```

Or update your `.csproj`:

```xml
<PackageReference Include="ListMapping" Version="1.0.0" />
```

---

## 🧠 **How It Works**

1. Identify the **ID property** or selector used to match list items.  
2. Pass lists and property names (or lambda selectors) to `MapList`.  
3. The library handles:
   - Adding missing items.
   - Updating existing items.
   - Removing unmatched items.

---

## 📄 **License**

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 🤝 **Contributing**

1. Fork the repository.  
2. Create a new branch (`git checkout -b feature/your-feature`).  
3. Commit your changes (`git commit -am 'Add new feature'`).  
4. Push the branch (`git push origin feature/your-feature`).  
5. Create a pull request.

---

## 🧑‍💻 **Author**

- **Osama**  
- [![LinkedIn](https://img.shields.io/badge/LinkedIn-Profile-blue?style=for-the-badge&logo=linkedin)](https://www.linkedin.com/in/osama-abu-sitta-baa020135/)  
- [![GitHub](https://img.shields.io/badge/GitHub-Profile-black?style=for-the-badge&logo=github)](https://github.com/OsamaAbuSitta)
- [![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-Profile-orange?style=for-the-badge&logo=stackoverflow)](https://stackoverflow.com/users/3926461/osama-abusitta)


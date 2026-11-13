

// 2. الوراثة (Inheritance): الفئات الفرعية للأجهزة المختلفة
public class Refrigerator : Appliance
{
    public bool NeedsGasRefill { get; set; }

    public override decimal CalculateRepairCost()
    {
        return BaseFee +(NeedsGasRefill ? 40 : 0);
    }
}

public class WashingMachine : Appliance
{
    public bool HasMotorIssue { get; set; }

    public override decimal CalculateRepairCost()
    {
        return BaseFee +(HasMotorIssue ? 75 : 0);
    }
}

public class AirConditioner : Appliance
{
    public int FilterCleaningCount { get; set; }

    public override decimal CalculateRepairCost()
    {
        return BaseFee +(FilterCleaningCount * 15);
    }
}

// الفئة الرئيسية التي تحتوي على نقطة البداية Main
internal class Program
{
    static void Main(string[] args)
    {
        // 3. تعدد الأشكال (Polymorphism): قائمة تجمع الكائنات المشتقة
        List<Appliance> devicesList = new List<Appliance>
            {
                new Refrigerator
                {
                    DeviceName = "Samsung Refrigerator",
                    SerialNumber = "REF-101",
                    Status = "Repaired",
                    BaseFee = 50,
                    NeedsGasRefill = true
                },
                new WashingMachine
                {
                    DeviceName = "LG Washing Machine",
                    SerialNumber = "WM-202",
                    Status = "Under Repair",
                    BaseFee = 60,
                    HasMotorIssue = true
                },
                new AirConditioner
                {
                    DeviceName = "Gree Air Conditioner",
                    SerialNumber = "AC-303",
                    Status = "Repaired",
                    BaseFee = 80,
                    FilterCleaningCount = 2
                }
            };

        Decimal totalCosts = 0;

        Console.WriteLine("=== Home Appliance Maintenance Center System ===\n");

        foreach(var device in devicesList)
            {
            device.DisplayDetails();
            Decimal cost = device.CalculateRepairCost();
            Console.WriteLine($"Repair Cost: {cost} $");
            Console.WriteLine("--------------------------------------------------");
            totalCosts += cost;
        }

        Console.WriteLine($"\nTotal Repair Cost for All Devices: {totalCosts} $");
        Console.ReadKey();
    }
}
}



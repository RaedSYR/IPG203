using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ApplianceMaintenanceSystem
{
    public enum RepairStatus
    {
        UnderRepair,
        Repaired,
        Delivered
    }

    public static class ValidationUtility
    {
        // التحقق من أن الرقم التسلسلي يتبع نمطاً دقيقاً حسب نوع الجهاز
        public static bool IsValidSerialNumber(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber)) return false;
            return Regex.IsMatch(serialNumber, @"^(REF|WM|AC)-\d{3,}$");
        }

        // التحقق من أن الرسوم المدخلة قيمة موجبة وليست سالبة
        public static bool IsValidFee(decimal fee)
        {
            return fee >= 0;
        }

        // التحقق من صحة تواريخ الصيانة بحيث لا تكون في المستقبل البعيد جداً أو غير منطقية
        public static bool IsValidDate(DateTime date)
        {
            return date <= DateTime.Now && date >= new DateTime(2020, 1, 1);
        }
    }

    public static class MaintenanceStatistics
    {
        private static readonly List<Appliance> _allAppliances = new List<Appliance>();

        // خاصية للقراءة فقط لحماية العدائين ومنع التعديل الخارجي المباشر
        public static int TotalDevicesCount => _allAppliances.Count;

        internal static void RegisterAppliance(Appliance appliance)
        {
            if (appliance != null)
            {
                _allAppliances.Add(appliance);
            }
        }

        // حساب عدد الأجهزة بناءً على حالة صيانة معينة
        public static int GetCountByStatus(RepairStatus status)
        {
            int count = 0;
            foreach (var app in _allAppliances)
            {
                if (app.Status == status) count++;
            }
            return count;
        }
    }

    public abstract class Appliance
    {
        private readonly string _serialNumber;
        private decimal _repairFee;
        public RepairStatus Status { get; set; }
        public DateTime MaintenanceDate { get; set; }

        public string SerialNumber => _serialNumber;

        public decimal RepairFee
        {
            get => _repairFee;
            set
            {
                if (!ValidationUtility.IsValidFee(value))
                    raiseArgumentException("الرسوم لا يمكن أن تكون سالبة.");
                _repairFee = value;
            }
        }

        protected Appliance(string serialNumber, decimal repairFee, DateTime maintenanceDate, RepairStatus status)
        {
            if (!ValidationUtility.IsValidSerialNumber(serialNumber))
                throw new ArgumentException("الرقم التسلسلي غير مطابق للشروط.");

            if (!ValidationUtility.IsValidDate(maintenanceDate))
                throw new ArgumentException("تاريخ الصيانة غير صالح.");

            _serialNumber = serialNumber;
            RepairFee = repairFee;
            MaintenanceDate = maintenanceDate;
            Status = status;

            MaintenanceStatistics.RegisterAppliance(this);
        }

        private void raiseArgumentException(string message) => throw new ArgumentException(message);

        public abstract decimal CalculateTotalCost();
    }

    public class Refrigerator : Appliance
    {
        public Refrigerator(string serialNumber, decimal repairFee, DateTime maintenanceDate, RepairStatus status)
            : base(serialNumber, repairFee, maintenanceDate, status) { }

        // حساب تكلفة الإصلاح الخاصة بالثلاجة مع إضافة رسوم فحص إضافية
        public override decimal CalculateTotalCost()
        {
            return RepairFee + 50;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // اختبار حالة صحيحة
                Refrigerator fridge = new Refrigerator("REF-101", 150, DateTime.Now.AddDays(-2), RepairStatus.Repaired);
                Console.WriteLine($"تم تسجيل الثلاجة بنجاح. التكلفة الكلية: {fridge.CalculateTotalCost()}");

                // اختبار حالة خاطئة للتأكد من عمل أدوات التحقق
                Refrigerator badFridge = new Refrigerator("----x", 100, DateTime.Now, RepairStatus.UnderRepair);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطأ أثناء الإنشاء: {ex.Message}");
            }

            Console.WriteLine($"إجمالي الأجهزة المسجلة: {MaintenanceStatistics.TotalDevicesCount}");
            Console.WriteLine($"عدد الأجهزة المصلحة: {MaintenanceStatistics.GetCountByStatus(RepairStatus.Repaired)}");

            Console.ReadKey();
        }
    }
}

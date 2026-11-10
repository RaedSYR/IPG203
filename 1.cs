using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Xml.Linq;
namespace ConsoleApp1
{
    // 1. التجريد (Abstraction): واجهة العمليات الأساسية للأجهزة
    public interface Idevice
{
    void DisplayDetails();
}

// فئة مجردة تطبق الواجهة وتحتوي على خصائص ودالة مجردة لحساب تكلفة الإصلاح
public abstract class Appliance : Idevice
    {
    public string DeviceName { get; set; }
    public string SerialNumber { get; set; }
    public string Status { get; set; }
    public decimal BaseFee { get; set; }

    public abstract decimal CalculateRepairCost();

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"Device: {DeviceName} | Serial: {SerialNumber} | Status: {Status}");
    }
}

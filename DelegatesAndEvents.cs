using HomeApplianceRepairCenter.Models;
using HomeApplianceRepairCenter.Models.Enums;
using HomeApplianceRepairCenter.Services;



var notifier = new NotificationService();
var center = new RepairCenter(notifier);

DateTime today = DateTime.Today;

var fridge = new Refrigerator("FR-100234", "Ahmad Saleh", "Compressor not starting",
                              today.AddDays(-6), today.AddDays(-1), requiresGasRefill: true);
var washer = new WashingMachine("WM-200871", "Lina Haddad", "Drum does not rotate",
                                today.AddDays(-2), today.AddDays(5), drumReplaced: true);
var cooler = new AirConditioner("AC-300455", "Omar Khoury", "Water leaking from indoor unit",
                                today.AddDays(-1), today.AddDays(4), isOnSiteRepair: false);

center.Register(fridge);
center.Register(washer);
center.Register(cooler);

Console.WriteLine("=== 1. Status changes raise events ===");
washer.ChangeStatus(RepairStatus.Diagnosing);
washer.ChangeStatus(RepairStatus.UnderRepair);
washer.PartsCost = 60m;
washer.LaborHours = 2.5;
washer.ChangeStatus(RepairStatus.Completed);   // raises StatusChanged and RepairCompleted

Console.WriteLine();
Console.WriteLine("=== 2. Delay detection raises an event ===");
int overdue = center.CheckAllDelays(today);    // the refrigerator is overdue
Console.WriteLine($"Overdue appliances: {overdue}");

Console.WriteLine();
Console.WriteLine($"Notifications logged: {notifier.Log.Count}");



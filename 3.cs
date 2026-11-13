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


washer.ChangeStatus(RepairStatus.Diagnosing);
washer.ChangeStatus(RepairStatus.UnderRepair);
washer.PartsCost = 60m;
washer.LaborHours = 2.5;
washer.ChangeStatus(RepairStatus.Completed);   // raises StatusChanged and RepairCompleted



Console.WriteLine();
Console.WriteLine("===  Encapsulation rules are enforced ===");
TryAction("Skip a stage (Received -> Completed)", () => cooler.ChangeStatus(RepairStatus.Completed));
TryAction("Set a negative parts cost", () => cooler.PartsCost = -10m);
TryAction("Edit parts cost after completion", () => washer.PartsCost = 5m);
TryAction("Create appliance with invalid serial", () =>
    _ = new AirConditioner("12", "Test", "Test", today, today.AddDays(1), false));
// fridge.SerialNumber = "X";  // does not compile: the property is read-only


// Runs an action and prints the rejection reason when the object protects its state.
static void TryAction(string description, Action action)
{
    try
    {
        action();
        Console.WriteLine($"[ACCEPTED] {description}");
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"[REJECTED] {description} -> {ex.Message}");
    }
}

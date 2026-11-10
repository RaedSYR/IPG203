
    // 4. التفويض والأحداث (Delegates & Events): تعريف الـ Delegate للإشعار بالحالة
    public delegate void RepairNotificationHandler(string message);

    // فئة مجردة تطبق الواجهة وتحتوي على خصائص ودالة مجردة لحساب تكلفة الإصلاح
    public abstract class Appliance : IDevice
    
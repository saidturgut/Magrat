namespace Controller;
using Contracts;

public partial class Monitor(IHost Host)
{
    public string cpuName = "";

    public Dictionary<string, Action>? OneToken;
    public Dictionary<string, Action<string>>? TwoToken;
    public Dictionary<string, Action<string, string>>? ThreeToken;

    private readonly Models.x8Bit.Machine x8Bit = new ();
    private readonly Models.Pdp1170.Pdp1170 Pdp1170 = new ();
    
    private ICpu? Cpu;
    private ISudo? Bus;

    private byte step;
    private byte sleep;
    public bool exit;
    private bool debug;
    
    public void Init()
    {
        OneToken = new Dictionary<string, Action>()
        {
            ["help"] = Help, ["exit"] = Exit, ["clear"] = Clear, ["debug"] = Debug, ["run"] = Run,
        };
        TwoToken = new Dictionary<string, Action<string>>()
        {
            ["set"] = Set, ["disk"] = Disk, ["sleep"] = Sleep, ["step"] = Step,
        };
        ThreeToken = new Dictionary<string, Action<string, string>>()
        {
            ["load"] = Load, ["mem"] = Mem, ["reg"] = Reg, ["dump"] = Dump,
        };
    }

    private void Reset()
    {
        Cpu = null;
        Bus = null;
        cpuName = "";
    }
}

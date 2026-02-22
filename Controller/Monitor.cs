namespace Controller;
using Contracts;

public partial class Monitor(IHost Host)
{
    public string cpuName = "";

    public Dictionary<string, Action>? OneToken;
    public Dictionary<string, Action<string>>? TwoToken;
    public Dictionary<string, Action<string, string>>? ThreeToken;
    
    private Dictionary<string, IMachine> Machines;
    
    private IMachine? Machine = null;
    
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
        
        Machines = new Dictionary<string, IMachine>
        {
            {"pdp11", new Models.Pdp1170.Pdp1170() },
            {"x8bit", Models.x8Bit.Cpus.Table("z80", this)! },
        };
    }
    
    private void Reset()
    {
        Machine = null;
        cpuName = "";
    }
}

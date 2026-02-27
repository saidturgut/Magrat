namespace Models.Pdp1170.Cpu.Executing;

public class Register
{
    private ushort commit;
    private ushort value;

    public void Init(ushort input)
    {
        Set(input);
        Commit(false);
    }
    
    public void Set(ushort input)
        => value = input;

    public ushort Get()
        => value;

    public void Commit(bool abort)
    {
        if(!abort) commit = value;
        else value = commit;
    }
}
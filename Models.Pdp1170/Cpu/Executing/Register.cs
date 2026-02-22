namespace Models.Pdp1170;

public class Register
{
    private ushort commit;
    private ushort value;

    public void Init(ushort input)
    {
        Set(input);
        Commit();
    }
    
    public void Set(ushort input)
        => value = input;

    public ushort Get()
        => commit;

    public void Commit()
    {
        commit = value;
    }

    public void Update()
    {
        value = commit;
    }
}
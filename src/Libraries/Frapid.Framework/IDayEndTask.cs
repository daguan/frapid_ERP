namespace Frapid.Framework
{
    public interface IDayEndTask
    {
        string[] Catalogs { get; set; }
        string Description { get; set; }
        void Register();
    }
}
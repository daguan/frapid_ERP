namespace Frapid.Framework
{
    public interface IStartupRegistration
    {
        string Description { get; set; }
        void Register();
    }
}
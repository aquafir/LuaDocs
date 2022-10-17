namespace FakeProject
{
    public abstract class IAbstract
    {
        public virtual string Virtual() => "Virt";
        public abstract string TestAbstract(string a, string b);
    }
}

using dotnetcore;
using dotnetcore.Data;
using System;



namespace Tester
{
    class Program
    {
        
        static void Main(string[] args)
        {
            UnitOfWork uof = new UnitOfWork();

            var rep = uof.GetRepository<Award>();

            Award item = new Award { Logo = new byte[5], Number = 3, Title = "title" };

            rep.Add(item);
            uof.Save();
        }
    }
}

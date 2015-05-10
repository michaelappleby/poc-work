namespace EFServerMon
{
    class Program
    {
        static void Main(string[] args)
        {
            MasterServer.MasterServerQuery qry = new MasterServer.MasterServerQuery();
            qry.GetServerList();
        }
    }
}

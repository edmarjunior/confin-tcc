namespace ConFin.Common.Domain
{
    public class Parameters
    {
        public Parameters()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Ambiente = "D";
                UriWeb = "http://localhost:5001/Home/";
                UriApi = "http://localhost:5002/api/";
                ConnectionString = "Data Source=EDMAR-PC;Initial Catalog=ConFin;Integrated Security=True";
                //ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ConFin;Integrated Security=True";
            }
            else
            {
                Ambiente = "P";
                UriWeb = "http://confin.azurewebsites.net/";
                UriApi = "http://confinapi.azurewebsites.net/api/";
                ConnectionString = "Data Source=confinserver.database.windows.net, 1433;Initial Catalog=confin;User ID=Edmar;Password=ju1790!*";
                //ConnectionString = "c:/projetos/confin/connectionstring";
            }
        }

        public string Ambiente { get; set; }
        public string UriWeb { get; set; }
        public string UriApi { get; set; }
        public string ConnectionString { get; set; }
    }
}

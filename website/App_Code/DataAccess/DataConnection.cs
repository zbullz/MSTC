using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class DataConnection : IDataConnection
{
	public IDbConnection SqlConnection
	{
		get { return new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]); }
	}
}

public interface IDataConnection
{
	IDbConnection SqlConnection { get; }
}
﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Mapping;
using NUnit.Framework;

namespace Tests.Playground
{
	[TestFixture]
	public class Issue1925Tests : TestBase
	{

		[Table]
		class SampleClass
		{
			[Column] public int Id { get; set; }
			[Column] public string Value { get; set; }
		}

		[Test]
		[ActiveIssue(1925)]
		public void Issue1925Test([IncludeDataSources(ProviderName.Access, ProviderName.SqlServer, ProviderName.Sybase)]  string context)
		{
			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable<SampleClass>())
			{

				table.Insert(() => new SampleClass() { Id = 1, Value = "6" });
				Assert.AreEqual(1, table.ToList().Count());
				var asParam = "[0-9]";
				var asParamUnterm = "[0";

				Assert.AreEqual(0, table.Where(r => r.Value.Contains("[0")).ToList().Count());

				Assert.AreEqual(0, table.Where(r => r.Value.Contains(asParamUnterm)).ToList().Count());

				Assert.AreEqual(1, table.Where(r => r.Value.Contains("[0-9]")).ToList().Count());

				Assert.Throws<OleDbException>(() => table.Where(r => Sql.Like(r.Value, "[0")).ToList().Count());
				Assert.Throws<OleDbException>(() => table.Where(r => Sql.Like(r.Value, asParamUnterm)).ToList().Count());


				Assert.AreEqual(1, table.Where(r => Sql.Like(r.Value, "[0-9]")).ToList().Count());

				Assert.AreEqual(1, table.Where(r => Sql.Like(r.Value, asParam)).ToList().Count());

			}
		}
		
	}
}

using System.Data.Entity.Migrations;

namespace ModelChecker.DAL.Migrations
{
	public sealed class Configuration : DbMigrationsConfiguration<EF.ModelCheckerContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
			ContextKey = "ModelChecker.DAL.EF.ModelCheckerContext";

		}

		protected override void Seed(EF.ModelCheckerContext context)
		{

		}
	}
}

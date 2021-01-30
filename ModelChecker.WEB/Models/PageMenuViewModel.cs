using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public enum MenuItem { Statistic, Report, ClashDetective, Setting, Reference, License }
	public class PageMenuViewModel
	{
		public IEnumerable<LinqMenuViewModel> Items { get; set; }
	}

	public class GroupMenuViewModel
	{
		public string Title { get; set; }
		public ICollection<LinqMenuViewModel> Items { get; set; }
	}

	public class LinqMenuViewModel
	{
		public string Title { get; set; }
		public string Image { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
		public MenuItem MenuItem { get; set; }
	}
}
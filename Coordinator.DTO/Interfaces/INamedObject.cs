using System.Collections.Generic;

namespace Coordinator.DTO
{
	public interface INamedObject
	{
		int Id { get; set; }
		string Name { get; set; }
		IEnumerable<int> Parents { get; set; }
	}
}

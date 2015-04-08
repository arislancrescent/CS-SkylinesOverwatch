using ICities;

namespace SkylinesOverwatch
{
	public class Identity : IUserMod
	{
		public string Name
		{
			get { return Settings.Instance.Tag; }
		}

		public string Description
		{
			get	{ return "Efficient monitoring framework to monitor and categorize buildings, vehicles, citizens, and animals in the city. Pause the game to see a summary of everything currently tracked in the debug window."; }
		}
	}
}
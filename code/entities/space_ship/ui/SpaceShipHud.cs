using Sandbox;
using Sandbox.UI;

namespace TLC.SSP
{
	[Library]
	public partial class SpaceShipHud : HudEntity<RootPanel>
	{
		public SpaceShipHud()
		{
			if ( !IsClient )
				return;

			RootPanel.StyleSheet.Load( "entities/space_ship/ui/SpaceShipHud.scss" );

			RootPanel.AddChild<Velocity>();
		}
	}
}

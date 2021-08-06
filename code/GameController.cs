using Sandbox;

namespace TLC.SSP
{
	[Library( "ssp", Title = "Sausage Space Program" )]
	public partial class GameController : Game
	{
		public GameController()
		{
			if (IsServer)
			{
				_ = new SpaceShipHud();
			}
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );
			var player = new GroundPlayer();
			player.Respawn();

			cl.Pawn = player;
		}

		public override void DoPlayerNoclip( Client player )
		{
			if ( player.Pawn is Player basePlayer )
			{
				if ( basePlayer.DevController is NoclipController )
				{
					Log.Info( "Noclip Mode Off" );
					basePlayer.DevController = null;
				}
				else
				{
					Log.Info( "Noclip Mode On" );
					basePlayer.DevController = new NoclipController();
				}
			}
		}
	}
}

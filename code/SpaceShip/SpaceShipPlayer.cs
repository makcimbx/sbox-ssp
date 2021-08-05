using Sandbox;

namespace TLC.SSP
{
	partial class SpaceShipPlayer : Player
	{
		[Net] public bool EnginesStarted { get; set; }

		public SpaceShipPlayer()
		{

		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			Controller = new WalkController();

			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			Animator = new StandardPlayerAnimator();

			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();
		}

		public override void TakeDamage( DamageInfo info )
		{

		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( Input.ActiveChild != null )
			{
				ActiveChild = Input.ActiveChild;
			}

			if ( LifeState != LifeState.Alive )
				return;

			SimulateActiveChild( cl, ActiveChild );

			if ( EnginesStarted )
			{
				Velocity += ( Vector3.OneZ * 100);
				GroundEntity = null;
			}

			if ( Input.Pressed( InputButton.Jump ) )
			{
				EnginesStarted = !EnginesStarted;
			}
		}

		[ServerCmd( "start_engines" )]
		public static void StartEngines()
		{
			var target = ConsoleSystem.Caller.Pawn;
			if ( target == null ) return;

			if ( target is SpaceShipPlayer player )
			{
				player.EnginesStarted = true;
			}
		}
	}
}
